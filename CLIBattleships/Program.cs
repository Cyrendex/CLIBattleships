using System;

namespace CLIBattleships
{
    class Program
    {
        static bool coordinateChecker(string coordinate)
        {
            return true;
        }
        static void Main(string[] args)
        {
            GridPlane gridOne = new GridPlane();
            gridOne.drawGrid(true);
            Console.ReadKey();
        }
    }
}
