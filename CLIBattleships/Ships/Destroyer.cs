using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Destroyer : ShipContent
    {
        public override string Name => "Destroyer";
        public override char Symbol => Symbols.DESTROYER_SYMBOL;
        public override int Size { get; } = 3;
        public override int Score { get; set; } = 1000;
        public Destroyer(Player owner) : base(owner)
        {
        }
    }
}
