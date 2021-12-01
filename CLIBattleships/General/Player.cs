using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIBattleships
{
    public class Player
    {
        public string Name { get; set; }
        public Grid[][] GridPlane { get; set; }
        public ShipContent[] ShipList { get; set; }
        public int NumberOfShots { get; set; }
        public int Score { get; set; }
        public int TotalHealth { get; set; }

        // In order to add/remove a ship, initialize it in the constructor and add it to the ShipList
        public Player(Grid[][] plane)
        {
            GridPlane = plane;
            Score = 0;  
            
            AircraftCarrier aircraftCarrier = new AircraftCarrier(this);
            Battleship battleship = new Battleship(this);
            Destroyer destroyer = new Destroyer(this);
            Submarine submarine = new Submarine(this);
            Patrol patrol = new Patrol(this);
            TestShip testShip = new TestShip(this);
            ShipList = new ShipContent[] { aircraftCarrier, battleship, destroyer, submarine, patrol, testShip };

            if (GameSettings.salvoMode)
                NumberOfShots = ShipList.Length;
            else
                NumberOfShots = GameSettings.DefaultNumberOfShots;
        }

        /* If it's the player's own grid, the ships will be drawn with their respective symbols. If not, they will be drawn with the empty symbol. */
        public void DrawGridPlane(bool ownGrid = false, bool gameEnded = false)
        {
            int maxIndent = (int)Math.Floor(Math.Log10(GameSettings.GridYSize)); // How many times you'd need to indent at most.
            int indent = 0;

            // A function that decides how many spaces the indentation should be for each number or letter.
            var myswitch = new Dictionary<Func<int, bool>, Action> // Function to decide how many times I need to indent for each case
                {
                    { x => x < 10 ,          () => indent = maxIndent     },
                    { x => x < 100 ,         () => indent = maxIndent - 1 },
                    { x => x < 1000 ,        () => indent = maxIndent - 2 },
                    { x => x < 10000 ,       () => indent = maxIndent - 3 },
                    { x => x < 100000 ,      () => indent = maxIndent - 4 },
                    { x => x < 1000000 ,     () => indent = maxIndent - 5 },
                    { x => x < 10000000 ,    () => indent = maxIndent - 6 },
                    { x => x < 100000000 ,   () => indent = maxIndent - 7 },
                    { x => x < 1000000000 ,  () => indent = maxIndent - 8 }
                };

            // Indenting the letters
            for (int i = 0; i < maxIndent + 2; i++) // +2 for the initial space
            {
                Console.Write(" ");
            }
            for (int coordinateLetter = 0; coordinateLetter < GameSettings.GridXSize; coordinateLetter++) // Loop for setting the letters in.
            {
                Console.Write((CoordinateLetter)coordinateLetter + " ");

            }
            Console.WriteLine();
            
            
            for (int coordinateNumber = 1; coordinateNumber <= GameSettings.GridYSize; coordinateNumber++)
            {
                myswitch.First(sw => sw.Key(coordinateNumber)).Value();
                // Indenting the numbers
                for (int i = 0; i < indent; i++)
                {
                    Console.Write(" ");
                }
                Console.Write(coordinateNumber);

                for (int coordinateLetter = 0; coordinateLetter < GameSettings.GridXSize; coordinateLetter++)
                {
                    Console.Write(" " + GridPlane[coordinateNumber - 1][coordinateLetter].GetSymbol(ownGrid));
                   
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            // Display the score and health if the game ended.
            if(gameEnded)
                Console.WriteLine("Score: {0}\nHealth Left: {1}\n", Score, TotalHealth);

            // Puts '='s longer than the x axis of the grid... *3 because formatting.
            for (int length = 0; length < GameSettings.GridXSize * 3; length++)
            {
                Console.Write("=");
            }
            Console.WriteLine("\n");
        }

        public void SetShipsOnGridPlane()
        {
            int coordinateNumber1, coordinateNumber2;
            CoordinateLetter coordinateLetter1, coordinateLetter2;
            bool valid;
            foreach (ShipContent ship in ShipList)
	        {                
                do
                {
                    DrawGridPlane(true);
                    Console.WriteLine("{0}, what should be the starting point of your {1}? (Length: {2})", Name, ship.Name, ship.Size);
                    CoordinateHandler.CoordinateAsker(out coordinateLetter1, out coordinateNumber1);
                    Console.WriteLine();
                    Console.WriteLine("{0}, what should be the ending point of your {1}?", Name, ship.Name);
                    CoordinateHandler.CoordinateAsker(out coordinateLetter2, out coordinateNumber2);
                    Console.WriteLine();
                    valid = GridPlaneHandler.SizeAndCollisionChecker(ship.Name, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2, ship.Size);
                    if (!valid)
                        System.Threading.Thread.Sleep(1500);
                    Console.Clear();
                } while (!valid);

                GridPlaneHandler.ShipSetter(ship, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2);
                Console.Clear();
	        }
        }
        private string ShootShipReturnMessage(Player enemyPlayer, CoordinateLetter coordinateLetter, int coordinateNumber)
        {
            Grid grid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
            grid.State = GridState.Attacked;

            ShipContent ship = (ShipContent)grid.Content;
            ship.Health--;
            enemyPlayer.TotalHealth--;
            Score += ship.Score;

            if (ship.IsSunk())
            {
                if (GameSettings.salvoMode)
                    enemyPlayer.NumberOfShots--;
                return "" + coordinateLetter + coordinateNumber + ship.ReturnHitMessage() + "\n" + ship.ReturnSunkMessage() + "\n";
            }

            return "" + coordinateLetter + coordinateNumber + ship.ReturnHitMessage() + "\n";
        }
        private string ShootEmptyReturnMessage(Player enemyPlayer, CoordinateLetter coordinateLetter, int coordinateNumber)
        {
            Grid grid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
            grid.State = GridState.Attacked;
            EmptyContent empty = (EmptyContent)grid.Content;
            Score += empty.Score;
            return "" + coordinateLetter + coordinateNumber + empty.ReturnHitMessage() + "\n";

        }
        public string ShootAndReturnStatus(Player enemyPlayer)
        {
            string message = "";
            bool shotFired;
            CoordinateLetter coordinateLetter;
            int coordinateNumber;
            for (int shotsLeft = NumberOfShots; shotsLeft > 0; shotsLeft--)
            {
                if (enemyPlayer.TotalHealth <= 0)
                    break;
                shotFired = false;
                while (!shotFired)
                {
                    enemyPlayer.DrawGridPlane();
                    Console.WriteLine("Where would you like to shoot? (" + shotsLeft + " shot(s) left)\n(If you want to view your own grid, type 'grid')\n");
                    Console.Write("Please enter a coordinate: ");
                    string prompt = Console.ReadLine();

                    if (WantsToSeeTheirGrid(prompt))
                    {
                        ShowOwnGrid();
                    }
                    else // Wants to shoot
                    {
                        CoordinateHandler.CoordinateSplitter(prompt, out coordinateLetter, out coordinateNumber);
                        if (CoordinateIsValid(coordinateNumber))
                        {
                            Grid enemyGrid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
                            if (EnemyGridNotAttacked(enemyGrid))
                            {
                                if (EnemyGridHasShip(enemyGrid))
                                    message += ShootShipReturnMessage(enemyPlayer, coordinateLetter, coordinateNumber);
                                else // It's EmptyContent or an unprecedented error.
                                    message += ShootEmptyReturnMessage(enemyPlayer, coordinateLetter, coordinateNumber);
                                shotFired = true;
                            }
                            else // Enemy Grid was already attacked.
                            {
                                enemyGrid.PrintAttackedMessage();
                                System.Threading.Thread.Sleep(1000);
                            }

                        }
                        else // If the coordinate isn't valid
                        {
                            Console.WriteLine("Please enter a valid coordinate.");
                            System.Threading.Thread.Sleep(1000);
                        }
                        Console.Clear();
                    }
                }
            }
            return message;
        }
        private bool WantsToSeeTheirGrid(string prompt)
        {
            return prompt.ToLower().Equals("grid");
        }
        private void ShowOwnGrid()
        {
            Console.Clear();
            DrawGridPlane(ownGrid: true);
            Console.WriteLine("(Press any key to go back)");
            Console.ReadKey();
            Console.Clear();
        }

        // Coordinate is valid if the coordinate number isn't -1
        private bool CoordinateIsValid(int coordinateNumber)
        {
            return coordinateNumber != -1;
        }

        private bool EnemyGridNotAttacked(Grid enemyGrid)
        {
            return enemyGrid.State == GridState.NotAttacked;
        }

        private bool EnemyGridHasShip(Grid enemyGrid)
        {
            return enemyGrid.Content is ShipContent;
        }
    }
}
