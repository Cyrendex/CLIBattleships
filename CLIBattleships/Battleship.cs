using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Battleship : ShipContent
    {
        public override int size { get; } = 4;
        public override int health { get; set; } = 4;
        public override int score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Battleship";
            return hitMessage += "!";
        }
    }
}
