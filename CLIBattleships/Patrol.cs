using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Patrol : ShipContent
    {
        public override char Symbol => Symbols.PATROL_SYMBOL;
        public override int Size { get; } = 2;
        public override int Health { get; set; } = 2;
        public override int Score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Patrol";
            return hitMessage += "!";
        }
    }
}
