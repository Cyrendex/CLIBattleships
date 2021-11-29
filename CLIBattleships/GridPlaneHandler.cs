using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class GridPlaneHandler
    {
        public static Grid[][] MakeGridPlane()
        {
            Grid[][] tempGridPlane = new Grid[GameSettings.GRID_XSIZE][];
            for (int coordinateNumber = 0; coordinateNumber < GameSettings.GRID_XSIZE; coordinateNumber++)
            {
                tempGridPlane[coordinateNumber] = new Grid[GameSettings.GRID_YSIZE];
                for (int coordinateLetter = 0; coordinateLetter < GameSettings.GRID_YSIZE; coordinateLetter++)
                {
                    tempGridPlane[coordinateNumber][coordinateLetter] = new Grid(letter: (CoordinateLetter)coordinateLetter, number: coordinateNumber + 1, content: new EmptyContent());         
                }
            }
            return tempGridPlane;
        }

        /* Checks if a ship collides with an already placed ship when setting it on a grid plane */
        static bool CollisionChecker(string shipName, Grid[][] gridPlane, CoordinateLetter coordinateLetter1, CoordinateLetter coordinateLetter2, int coordinateNumber1, int coordinateNumber2)
        {
            bool valid = true;
            bool sameRow = (coordinateLetter1 == coordinateLetter2);

            if (sameRow)
            {
                int row = (int)coordinateLetter1;
                int start = Math.Min(coordinateNumber1, coordinateNumber2);
                int end = Math.Max(coordinateNumber1, coordinateNumber2);

                for (int i = start; i <= end; i++)
                {
                    if (gridPlane[i - 1][row].Content is ShipContent)
                        valid = false;
                }
            }
            else //Same column
            {
                int col = coordinateNumber1 - 1; // Index reasons
                int start = Math.Min((int)coordinateLetter1, (int)coordinateLetter2);
                int end = Math.Max((int)coordinateLetter1, (int)coordinateLetter2);

                for (int i = start; i <= end; i++)
                {
                    if (gridPlane[col][i].Content is ShipContent)
                        valid = false;
                }
            }

            if (!valid)
                Console.WriteLine(shipName + " collided with another ship!");
            return valid;
        }

        /* Checks if the given coordinates are suitable to fit the ship in. Returns true if valid. */
        public static bool SizeChecker(string shipName, CoordinateLetter coordinateLetter1, CoordinateLetter coordinateLetter2, int coordinateNumber1, int coordinateNumber2, int size)
        {
            if (coordinateLetter1 == coordinateLetter2 || coordinateNumber1 == coordinateNumber2) // Prevents ships from being placed diagonally.
                if (Math.Abs(coordinateLetter1 - coordinateLetter2) + 1 == size || Math.Abs(coordinateNumber1 - coordinateNumber2) + 1 == size)
                    return true;
            Console.WriteLine(shipName + " doesn't fit in the given range.");
            return false;
        }

        /* Calls both SizeChecker() and CollisionChecker(), returns true if both are valid. */
        public static bool SizeAndCollisionChecker(string shipName, Grid[][] gridPlane, CoordinateLetter coordinateLetter1, CoordinateLetter coordinateLetter2, int coordinateNumber1, int coordinateNumber2, int size)
        {
            if (SizeChecker(shipName, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2, size) && CollisionChecker(shipName, gridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2))
                return true;
            else
                return false;
        }

        /* Sets ships onto the grid */
        public static void ShipSetter(ShipContent ship, Grid[][] gridPlane, CoordinateLetter coordinateLetter1, CoordinateLetter coordinateLetter2, int coordinateNumber1, int coordinateNumber2) 
        {
            bool sameRow = (coordinateLetter1 == coordinateLetter2);

            if (sameRow)
            {
                int row = (int)coordinateLetter1;
                int start = Math.Min(coordinateNumber1, coordinateNumber2);
                int end = Math.Max(coordinateNumber1, coordinateNumber2);

                for (int i = start; i <= end; i++)
                {
                    gridPlane[i - 1][row].Content = ship;                   
                }
            }
            else //Same column
            {
                int col = coordinateNumber1 - 1; // Index reasons.
                int start = Math.Min((int)coordinateLetter1, (int)coordinateLetter2);
                int end = Math.Max((int)coordinateLetter1, (int)coordinateLetter2);

                for (int i = start; i <= end; i++)
                {
                    gridPlane[col][i].Content = ship;
                }
            }
        }
    }
}
