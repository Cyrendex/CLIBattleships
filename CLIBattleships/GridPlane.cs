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
            gridPlane = fillGridPlane();
        }

        private Grid[,] fillGridPlane() {
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
    }
}
