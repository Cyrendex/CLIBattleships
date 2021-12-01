using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Submarine : ShipContent
    {
        public override string Name => "Submarine";
        public override char Symbol => Symbols.SUBMARINE_SYMBOL;
        public override int Size { get; } = 3;
        public override int Score { get; set; } = 1000;
        public Submarine(Player owner) : base(owner)
        {
        }
    }
}
