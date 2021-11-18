using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class GridPlane
    {
        Grid[,] gridPlane = new Grid[10, 10];
        public GridPlane()
        {
            gridPlane = FillGridPlane();
        }
        public Grid[,] GetGridPlane() {
            return gridPlane;
        }
        
        public void SetGridPlane(int let, int num, Grid grid)
        {
            gridPlane[let, num] = grid;
        }
        private Grid[,] FillGridPlane() {
            Grid[,] plane = new Grid[10, 10];
            for (int i = 0; i < plane.GetLength(0); i++)
            {
                for (int j = 0; j < plane.GetLength(1); j++)
                {
                    plane[i, j] = new Grid((CoordinateLetter)i, j+1);
                }
            }
            return plane;
        }

        public void DrawGrid(bool ownGrid)
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int i = 0; i < gridPlane.GetLength(0); i++)
            {
                    Console.Write((CoordinateLetter)i);               
                for (int j = 0; j < gridPlane.GetLength(1); j++)
                {
                    switch (gridPlane[i, j].GetType())
                    {
                        case GridType.Empty:
                            Console.Write(" O");
                            break;
                        case GridType.Attacked:
                            Console.Write(" /");
                            break;
                        case GridType.AircraftCarrier:
                        case GridType.Battleship:
                        case GridType.Destroyer:
                        case GridType.Submarine:
                        case GridType.Patrol:
                            if(ownGrid)
                                Console.Write(" =");
                            else
                                Console.Write(" O");
                            break;
                        case GridType.Hit:
                            Console.Write(" X");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
