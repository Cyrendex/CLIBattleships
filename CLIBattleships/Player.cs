using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class Player
    {
        string name;
        GridPlane plane;
        
        public Player(string name, GridPlane plane)
        {
            this.name = name;
            this.plane = plane;
        }

        public GridPlane GetGridInfo()
        {
            return plane;
        }

        public string GetName()
        {
            return name;
        }
    }
}
