using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Battleship : ShipContent
    {
        public override string Name => "Battleship";
        public override char Symbol => Symbols.BATTLESHIP_SYMBOL;
        public override int Size { get; } = 4;
        public override int Score { get; set; } = 1000;
        public Battleship(Player owner) : base(owner)
        {
        }
    }
}
