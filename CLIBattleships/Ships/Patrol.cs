using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Patrol : ShipContent
    {        
        public override string Name => "Patrol";
        public override char Symbol => Symbols.PATROL_SYMBOL;
        public override int Size { get; } = 2;
        public override int Score { get; set; } = 1000;
        public Patrol(Player owner) : base(owner)
        {
        }
    }
}
