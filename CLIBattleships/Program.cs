using System;
using System.Linq;

namespace CLIBattleships
{
    class Program
    {
        static bool CoordinateHandler(string coordinate, out CoordinateLetter letter, out int number)
        {
            // Coordinates need a letter and a number ranging from A-J and 1-10 respectively, inclusive.
            coordinate.ToUpper();
            int length = coordinate.Length;
            if (length <= 3 && length > 1) // A healthy coordinate is either 2 or 3 characters long (A1, B10)
                if (coordinate.Count(char.IsLetter) == 1 && char.IsLetter(coordinate, 0)) // Checks if there is only one letter. Also checks if the first digit is a letter.
                {
                    int num = Int32.Parse(coordinate.Substring(1, length - 1).TrimStart('0').PadLeft(1, '0')); // Extract number from coordinate
                    bool pass = Enum.TryParse(coordinate.Substring(0, 1).ToUpper(), out CoordinateLetter let);
                    if (num > 0 && num <= 10 && pass)
                    {
                        number = num;                       
                        letter = let;
                        return pass;
                    }
                }
            number = -1; // Temp values, the method will loop until the if conditions are satisfied.
            letter = 0;
            return false;
        }
        static void CoordinateAsker(out CoordinateLetter letter, out int number)
        {
            bool valid;
            do
            {
                Console.Write("Please enter a coordinate: ");
                valid = CoordinateHandler(Console.ReadLine(), out letter, out number);
                if (!valid)
                    Console.WriteLine("\nInvalid coordinate.");
            } while (!valid);
        }
        static void Main(string[] args)
        {
            /*
            AskPlayerName(out string p1Name, out string p2Name);
            GridPlane gridOne = new GridPlane();
            GridPlane gridTwo = new GridPlane();
            
            Player playerOne;
            Player playerTwo;
            if (AskSalvo())
            {
                playerOne = InitPlayer(p1Name, gridOne, true);
                playerTwo = InitPlayer(p2Name, gridTwo, true);
            }
            else
            {
                playerOne = InitPlayer(p1Name, gridOne);
                playerTwo = InitPlayer(p2Name, gridTwo);
            }
            

            SetShipsOnGrid(playerOne);
            SetShipsOnGrid(playerTwo);

            Player winner = GameLoop(ref playerOne, ref playerTwo);
            Console.Clear();
            playerOne.GetGridInfo().DrawGrid(true);
            Console.WriteLine("==========================\n");
            playerTwo.GetGridInfo().DrawGrid(true);

            Console.WriteLine("Congratulations, " + winner.GetName() + "! You destroyed all the enemy ships!\n(Press any key to end the game)");
            Console.ReadKey();
            */
            Grid[][] p1GridPlane = GridPlaneHandler.MakeGridPlane();
            Player p1 = new Player("Eren", p1GridPlane);
            p1.DrawGridPlane();
            Console.ReadKey();
            Console.Clear();
            p1.SetShipsOnGridPlane();
            Console.Clear();
            p1.DrawGridPlane(true);
            p1GridPlane[0][0].Content.ReturnHitMessage();
            ShipContent temp = (ShipContent)p1GridPlane[0][0].Content;
            temp.ReduceHealth(); // Left here, fix accessibility
            Console.ReadKey();
        }
        static Player InitPlayer(string name, GridPlane grid, bool isSalvo = false)
        {
            Player player = new Player(name, grid, isSalvo);
            return player;
        }

        static void AskPlayerName(out string p1Name, out string p2Name)
        {
            bool valid;
            do
            {
                Console.Write("Enter a name for player one: ");
                p1Name = Console.ReadLine().Trim();
                if (p1Name.Length > 15)
                {
                    Console.WriteLine("Name too long, keep it shorter than 16 characters.");
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p1Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player One.\"");
                    p1Name = "Player One";
                    valid = true;
                }
                else
                    valid = true;
            } while (!valid);
            Console.WriteLine();

            do
            {
                Console.Write("Enter a name for player two: ");
                p2Name = Console.ReadLine().Trim();
                if (p2Name.Length > 15)
                {
                    Console.WriteLine("Name too long, keep it shorter than 16 characters.");
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p2Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player Two.\"");
                    p2Name = "Player Two";
                    valid = true;
                }
                else if(p1Name.Equals(p2Name))
                {
                    Console.WriteLine("Name can't be the same as player one, try again.");
                    valid = false;
                }
                else
                    valid = true;
            } while (!valid);
            System.Threading.Thread.Sleep(1000);
            Console.Clear();           
        }
        static bool BoundChecker(CoordinateLetter cL1, CoordinateLetter cL2, int cN1, int cN2, int bound)
        {
            if (cL1 == cL2 || cN1 == cN2) // Prevents ships from being placed diagonally.
                if (Math.Abs(cL1 - cL2) + 1 == bound || Math.Abs(cN1 - cN2) + 1 == bound)
                    return true;
            return false;
        }
        static bool CollisionChecker(GridPlane plane, CoordinateLetter cL1, CoordinateLetter cL2, int cN1, int cN2)
        {
            bool lettersEqual = cL1 == cL2; 
            bool cL1Bigger = cL1 > cL2;      
            bool cN1Bigger = cN1 > cN2;
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    if (!(GetType(plane.GetGridPlane(), (int)cL1, num - 1) == GridType.Empty))
                        return false;
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    if (!(GetType(plane.GetGridPlane(), (int)cL1, num - 1) == GridType.Empty))
                        return false;
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    if (!(GetType(plane.GetGridPlane(), num, cN1 - 1) == GridType.Empty))
                        return false;
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    if (!(GetType(plane.GetGridPlane(), num, cN1 - 1) == GridType.Empty))
                        return false;
                }
            }
            return true;
        }
        static void SetShipsOnGrid(Player player)
        {
            SetAircraftCarrier(player.GetGridInfo(), player);
            SetBattleship(player.GetGridInfo(), player);
            SetDestroyer(player.GetGridInfo(), player);
            SetSubmarine(player.GetGridInfo(), player);
            SetPatrol(player.GetGridInfo(), player);
        }
        static GridType GetType(Grid[,] grid, int pos1, int pos2)
        {
            return grid[pos1, pos2].GetType();
        }
        static void SetAircraftCarrier(GridPlane plane, Player player)
        {
            Console.Clear();
            plane.DrawGrid(true);
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound;
            bool noCollision;
            Console.WriteLine(player.GetName() + ", please enter the desired coordinates to place your Aircraft Carrier. (Length: 5)");
            do
            {
                Console.WriteLine("Where should be the starting point of your Aircraft Carrier?");
                CoordinateAsker(out cL1, out cN1);
                Console.WriteLine("Where should be the ending point of your Aircraft Carrier?");
                CoordinateAsker(out cL2, out cN2);
                inBound = BoundChecker(cL1, cL2, cN1, cN2, bound: 5); // Bound is the length of the ship
                noCollision = CollisionChecker(plane, cL1, cL2, cN1, cN2);
                if (!inBound)
                    Console.WriteLine("Aircraft Carrier doesn't fit in the given range. Please re-enter the points.\n");
                else if (!noCollision) // Impossible to enter this if condition as the aircraft carrier is the first ship to be placed.
                    Console.WriteLine("Aircraft Carrier collided with another ship! Please re-enter the points.\n");
            }
            while (!inBound || !noCollision);

            bool lettersEqual = cL1 == cL2;  // These are decisive factors on how to
            bool cL1Bigger = cL1 > cL2;      // set up the loop for each ship. For example,
            bool cN1Bigger = cN1 > cN2;      // which coordinate to use as the lower bound will cause out of bound exceptions. Open to improvement.
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.AircraftCarrier);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.AircraftCarrier);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.AircraftCarrier);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.AircraftCarrier);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            RedrawGrid(plane, true);
        }
        static void SetBattleship(GridPlane plane, Player player)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound;
            bool noCollision;
            Console.WriteLine(player.GetName() + ", please enter the desired coordinates to place your Battleship. (Length: 4)");
            do
            {
                Console.WriteLine("Where should be the starting point of your Battleship?");
                CoordinateAsker(out cL1, out cN1);
                Console.WriteLine("Where should be the ending point of your Battleship?");
                CoordinateAsker(out cL2, out cN2);
                inBound = BoundChecker(cL1, cL2, cN1, cN2, bound: 4); // Bound is the length of the ship
                noCollision = CollisionChecker(plane, cL1, cL2, cN1, cN2);
                if (!inBound)
                    Console.WriteLine("Battleship doesn't fit in the given range. Please re-enter the points.\n");
                else if (!noCollision) 
                    Console.WriteLine("Battleship collided with another ship! Please re-enter the points.\n");
            }
            while (!inBound || !noCollision);

            bool lettersEqual = cL1 == cL2;
            bool cL1Bigger = cL1 > cL2;
            bool cN1Bigger = cN1 > cN2;
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Battleship);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Battleship);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Battleship);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Battleship);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            RedrawGrid(plane, true);
        }
        static void SetDestroyer(GridPlane plane, Player player)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound;
            bool noCollision;
            Console.WriteLine(player.GetName() + ", please enter the desired coordinates to place your Destroyer. (Length: 3)");
            do
            {
                Console.WriteLine("Where should be the starting point of your Destroyer?");
                CoordinateAsker(out cL1, out cN1);
                Console.WriteLine("Where should be the ending point of your Destroyer?");
                CoordinateAsker(out cL2, out cN2);
                inBound = BoundChecker(cL1, cL2, cN1, cN2, bound: 3); // Bound is the length of the ship
                noCollision = CollisionChecker(plane, cL1, cL2, cN1, cN2);
                if (!inBound)
                    Console.WriteLine("Destroyer doesn't fit in the given range. Please re-enter the points.\n");
                else if (!noCollision)
                    Console.WriteLine("Destroyer collided with another ship! Please re-enter the points.\n");
            }
            while (!inBound || !noCollision);

            bool lettersEqual = cL1 == cL2;
            bool cL1Bigger = cL1 > cL2;
            bool cN1Bigger = cN1 > cN2;
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Destroyer);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Destroyer);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Destroyer);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Destroyer);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            RedrawGrid(plane, true);
        }
        static void SetSubmarine(GridPlane plane, Player player)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound;
            bool noCollision;
            Console.WriteLine(player.GetName() + ", please enter the desired coordinates to place your Submarine. (Length: 3)");
            do
            {
                Console.WriteLine("Where should be the starting point of your Submarine?");
                CoordinateAsker(out cL1, out cN1);
                Console.WriteLine("Where should be the ending point of your Submarine?");
                CoordinateAsker(out cL2, out cN2);
                inBound = BoundChecker(cL1, cL2, cN1, cN2, bound: 3); // Bound is the length of the ship
                noCollision = CollisionChecker(plane, cL1, cL2, cN1, cN2);
                if (!inBound)
                    Console.WriteLine("Submarine doesn't fit in the given range. Please re-enter the points.\n");
                else if (!noCollision)
                    Console.WriteLine("Submarine collided with another ship! Please re-enter the points.\n");
            }
            while (!inBound || !noCollision);

            bool lettersEqual = cL1 == cL2;
            bool cL1Bigger = cL1 > cL2;
            bool cN1Bigger = cN1 > cN2;
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Submarine);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Submarine);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Submarine);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Submarine);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            RedrawGrid(plane, true);
        }
        static void SetPatrol(GridPlane plane, Player player)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound;
            bool noCollision;
            Console.WriteLine(player.GetName() + ", please enter the desired coordinates to place your Patrol. (Length: 2)");
            do
            {
                Console.WriteLine("Where should be the starting point of your Patrol?");
                CoordinateAsker(out cL1, out cN1);
                Console.WriteLine("Where should be the ending point of your Patrol?");
                CoordinateAsker(out cL2, out cN2);
                inBound = BoundChecker(cL1, cL2, cN1, cN2, bound: 2); // Bound is the length of the ship
                noCollision = CollisionChecker(plane, cL1, cL2, cN1, cN2);
                if (!inBound)
                    Console.WriteLine("Patrol doesn't fit in the given range. Please re-enter the points.\n");
                else if (!noCollision)
                    Console.WriteLine("Patrol collided with another ship! Please re-enter the points.\n");
            }
            while (!inBound || !noCollision);

            bool lettersEqual = cL1 == cL2;
            bool cL1Bigger = cL1 > cL2;
            bool cN1Bigger = cN1 > cN2;
            if (lettersEqual && cN1Bigger)
                for (int num = cN2; num <= cN1; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Patrol);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            else if (lettersEqual && !cN1Bigger)
            {
                for (int num = cN1; num <= cN2; num++)
                {
                    Grid grid = new Grid(cL1, num, GridType.Patrol);
                    plane.SetGridPlane((int)cL1, num - 1, grid);
                }
            }
            else if (!lettersEqual && cL1Bigger)
            {
                for (int num = (int)cL2; num <= (int)cL1; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Patrol);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            else
            {
                for (int num = (int)cL1; num <= (int)cL2; num++)
                {
                    Grid grid = new Grid((CoordinateLetter)num, cN1, GridType.Patrol);
                    plane.SetGridPlane(num, cN1 - 1, grid);
                }
            }
            RedrawGrid(plane, true);
        }
        static void RedrawGrid(GridPlane plane, bool ownGrid)
        {
            Console.Clear();
            plane.DrawGrid(ownGrid);
        }
        static Player CoinFlip(Player p1, Player p2)
        {
            Console.WriteLine("A coin flip will commence to decide who will start. (Press any key to continue)");
            Console.ReadKey();
            Random rnd = new Random();
            int num = rnd.Next(1, 4);
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("...");
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("...");
            System.Threading.Thread.Sleep(800);
            if (num % 2 == 0)
            {
                Console.Write(p1.GetName() + " won the coin flip! Press any key to continue when you're ready.");
                Console.ReadKey();
                return p1;
            }
            else
            {
                Console.Write(p2.GetName() + " won the coin flip! Press any key to continue when you're ready.");
                Console.ReadKey();
                return p2;
            }
        }
        static string Shoot(Player currentPlayer, Player enemyPlayer, int shotsLeft)
        {
            string message;
            bool shot = false;
            CoordinateLetter cL1;
            int cN1;
            while (!shot)
            {
                enemyPlayer.GetGridInfo().DrawGrid(false);
                Console.WriteLine("Please enter the coordinate you want to shoot. ("+ shotsLeft +" shot(s) left)\n(If you want to view your own grid, type 'grid')");
                string prompt = Console.ReadLine();
                if(prompt.ToLower().Equals("grid"))
                {
                    Console.Clear();
                    currentPlayer.GetGridInfo().DrawGrid(true);
                    Console.WriteLine("(Press any key to go back)");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    bool valid = CoordinateHandler(prompt, out cL1, out cN1);
                    if (valid)
                    {
                        Grid[,] temp = enemyPlayer.GetGridInfo().GetGridPlane();
                        GridType type = temp[(int)cL1,cN1-1].GetType();
                        bool isSalvo = currentPlayer.GetSalvo();
                        ShipStatus status;
                        switch (type)
                        {
                            case GridType.Empty:
                                message = cL1 + (cN1 + " missed!\n"); // () to avoid any wacky enum synergies with addition                               
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Attacked);
                                return message;
                            case GridType.Attacked:
                                Console.WriteLine("You already attacked this coordinate! Try again.");
                                System.Threading.Thread.Sleep(1000);
                                Console.Clear();
                                break;
                            case GridType.AircraftCarrier:
                                status = enemyPlayer.ReduceHealth(type);
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Hit);
                                message = cL1 + (cN1 + " hit");
                                if (isSalvo)
                                    message += " an Aircraft Carrier!\n";                      
                                else
                                    message += "!\n";
                                if (status == ShipStatus.aircraftCarrierSunk)
                                    message += "You sunk my battleship!\n";
                                return message;
                            case GridType.Battleship:
                                status = enemyPlayer.ReduceHealth(type);
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Hit);
                                message = cL1 + (cN1 + " hit");
                                if (isSalvo)
                                    message += " a Battleship!\n";
                                else
                                    message += "!\n";
                                if (status == ShipStatus.battleshipSunk)
                                    message += "You sunk my battleship!\n";
                                return message;
                            case GridType.Destroyer:
                                status = enemyPlayer.ReduceHealth(type);
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Hit);
                                message = cL1 + (cN1 + " hit");
                                if (isSalvo)
                                    message += " a Destroyer!\n";
                                else
                                    message += "!\n";
                                if (status == ShipStatus.destroyerSunk)
                                    message += "You sunk my battleship!\n";
                                return message;
                            case GridType.Submarine:
                                status = enemyPlayer.ReduceHealth(type);
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Hit);
                                message = cL1 + (cN1 + " hit");
                                if (isSalvo)
                                    message += " a Submarine!\n";
                                else
                                    message += "!\n";
                                if (status == ShipStatus.submarineSunk)
                                    message += "You sunk my battleship!\n";
                                return message;
                            case GridType.Patrol:
                                status = enemyPlayer.ReduceHealth(type);
                                enemyPlayer.GetGridInfo().SetGridType(cL1, cN1, GridType.Hit);
                                message = cL1 + (cN1 + " hit");
                                if (isSalvo)
                                    message += " a Patrol!\n";
                                else
                                    message += "!\n";
                                if (status == ShipStatus.patrolSunk)
                                    message += "You sunk my battleship!\n";
                                return message;
                            case GridType.Hit:
                                Console.WriteLine("You already hit a ship here! Try again.");
                                System.Threading.Thread.Sleep(1000);        
                                break;
                            default:
                                break;
                        }                        
                    }
                }
                Console.Clear();
            }
            return "";
        }
        static Player GameLoop(ref Player p1, ref Player p2)
        {
            Console.Clear();
            Player currentPlayer = CoinFlip(p1, p2);
            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                Console.WriteLine(currentPlayer.GetName() + ", it's your turn! Press any key to proceed.");
                Console.ReadKey();
                Console.Clear();
                string tempMessage = "----------------------------------------\n";
                for (int shots = 0; shots < currentPlayer.GetNumberOfShots(); shots++)
                {
                    if (currentPlayer == p1)
                        tempMessage += Shoot(p1, p2, p1.GetNumberOfShots() - shots);
                    else
                        tempMessage += Shoot(p2, p1, p2.GetNumberOfShots() - shots);
                    Console.Clear();
                }                              
                Console.WriteLine(tempMessage + "----------------------------------------");
                Console.WriteLine("Once you're ready to proceed, press any key.");
                Console.ReadKey();
                if (p1.GetTotalHealth() <= 0 || p2.GetTotalHealth() <= 0)
                    gameOver = true;
                else
                {
                    if (currentPlayer == p1)
                        currentPlayer = p2;
                    else
                        currentPlayer = p1;
                }
            }
            return currentPlayer;
        }

        static bool AskSalvo()
        {
            Console.WriteLine("Which variaton would you like to play? (1 or 2)\n(1) Classic (Default)\n(2) Salvo Variation (Advanced)");
            string input = Console.ReadLine();
            if (input.Equals("1")) 
            {
                Console.WriteLine("You chose the Classic variation, have fun!");
                System.Threading.Thread.Sleep(2500);
                return false;                
            }
            else if (input.Equals("2"))
            {
                Console.WriteLine("You chose the Salvo variation, good luck!");
                System.Threading.Thread.Sleep(2500);
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input. Choosing Classic variation.");
                System.Threading.Thread.Sleep(2500);
                return false;
            }
        }
    }
}
