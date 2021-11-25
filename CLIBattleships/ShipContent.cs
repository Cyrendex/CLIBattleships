using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public abstract class ShipContent : GridContent
    {
        public abstract int size { get; }
        public abstract int health { get; set; }
        public bool IsSunk()
        {
            return health < 1;
        }
        public void ReduceHealth()
        {
            health--;
        }
    }
}
