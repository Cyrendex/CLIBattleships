using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Submarine : ShipContent
    {
        public override int size { get; } = 3;
        public override int health { get; set; } = 3;
        public override int score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Submarine";
            return hitMessage += "!";
        }
    }
}
