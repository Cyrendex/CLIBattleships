using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    static class GridPlaneHandler
    {
        public static Grid[][] MakeGridPlane()
        {
            Grid[][] tempGridPlane = new Grid[GlobalConstant.GRID_XSIZE][];
            for (int coordinateNumber = 0; coordinateNumber < GlobalConstant.GRID_XSIZE; coordinateNumber++)
            {
                tempGridPlane[coordinateNumber] = new Grid[GlobalConstant.GRID_YSIZE];
                for (int coordinateLetter = 0; coordinateLetter < GlobalConstant.GRID_YSIZE; coordinateLetter++)
                {
                    tempGridPlane[coordinateNumber][coordinateLetter] = new Grid(letter: (CoordinateLetter)coordinateLetter, number: coordinateNumber + 1, content: new EmptyContent());         
                }
            }
            return tempGridPlane;
        }


    }
}
