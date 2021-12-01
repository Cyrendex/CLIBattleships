using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class TestShip : ShipContent
    {
        public override string Name => "Test Ship";
        public override char Symbol => Symbols.TEST_SHIP_SYMBOL;
        public override int Size { get; } = 8;
        public override int Score { get; set; } = 500;
        public TestShip(Player owner) : base(owner)
        {
        }
    }
}
