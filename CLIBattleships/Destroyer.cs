using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Destroyer : ShipContent
    {
        public override char Symbol => Symbols.DESTROYER_SYMBOL;
        public override int Size { get; } = 3;
        public override int Health { get; set; } = 3;
        public override int Score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Destroyer";
            return hitMessage += "!";
        }
    }
}
