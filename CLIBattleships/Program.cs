using System;
using System.Linq;
using System.Text.RegularExpressions;

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
                    bool pass = Enum.TryParse(coordinate.Substring(0, 1), out CoordinateLetter let);
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
            bool valid = false;
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
            AskPlayerName(out string p1Name, out string p2Name);
            GridPlane gridOne = new GridPlane();
            GridPlane gridTwo = new GridPlane();

            Player playerOne = InitPlayer(p1Name, gridOne);
            Player playerTwo = InitPlayer(p2Name, gridTwo);

            SetShipsOnGrid(playerOne.GetGridInfo());
            Console.ReadKey();
        }
        static Player InitPlayer(string name, GridPlane grid)
        {
            Player player = new Player(name, grid);
            return player;
        }

        static void AskPlayerName(out string p1Name, out string p2Name)
        {
            bool valid = true;
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
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player One\"");
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
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player Two\"");
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
        static void SetShipsOnGrid(GridPlane plane)
        {
            SetAircraftCarrier(plane);
            SetBattleship(plane);
            SetDestroyer(plane);
            SetSubmarine(plane);
            SetPatrol(plane);
        }
        static GridType GetType(Grid[,] grid, int pos1, int pos2)
        {
            return grid[pos1, pos2].GetType();
        }
        static void SetAircraftCarrier(GridPlane plane)
        {
            plane.DrawGrid(true);
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound = false;
            bool noCollision = false;
            Console.WriteLine("Please enter the desired coordinates to place your Aircraft Carrier. (Length: 5)");
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
        static void SetBattleship(GridPlane plane)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound = false;
            bool noCollision = false;
            Console.WriteLine("Please enter the desired coordinates to place your Battleship. (Length: 4)");
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
        static void SetDestroyer(GridPlane plane)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound = false;
            bool noCollision = false;
            Console.WriteLine("Please enter the desired coordinates to place your Destroyer. (Length: 3)");
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
        static void SetSubmarine(GridPlane plane)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound = false;
            bool noCollision = false;
            Console.WriteLine("Please enter the desired coordinates to place your Submarine. (Length: 3)");
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
        static void SetPatrol(GridPlane plane)
        {
            int cN1, cN2;
            CoordinateLetter cL1, cL2;
            bool inBound = false;
            bool noCollision = false;
            Console.WriteLine("Please enter the desired coordinates to place your Patrol. (Length: 2)");
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
    }
}
