using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public abstract class ShipContent : GridContent
    {
        public abstract int Size { get; }
        public abstract int Health { get; set; }
        public bool IsSunk()
        {
            return Health < 1;
        }
        public void ReduceHealth()
        {
            Health--;
        }
    }
}
