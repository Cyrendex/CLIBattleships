using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public abstract class ShipContent : GridContent
    {
        public abstract string Name { get; }
        public abstract int Size { get; }
        public int Health { get; set; }

        public override char Symbol => ' ';

        public override int Score { get; set; } = 0;

        public ShipContent(Player owner)
        {
            owner.TotalHealth += Size;
            Health = Size;
        }
        public bool IsSunk()
        {
            return Health < 1;
           
        }

        public override sealed string ReturnHitMessage()
        {
            string hitMessage = " hit";
            if (GameSettings.salvoMode)
                hitMessage += $" a(n) {Name}";
            return hitMessage += "!";
        }
        public string ReturnSunkMessage()
        {
            return "You sunk my " + (GameSettings.salvoMode == true ? Name : "battleship") + "!"; // In salvo mode, you must provide more information.
        }
    }
}
