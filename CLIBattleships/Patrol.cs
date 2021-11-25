using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Patrol : ShipContent
    {
        public override int size { get; } = 2;
        public override int health { get; set; } = 2;
        public override int score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Patrol";
            return hitMessage += "!";
        }
    }
}
