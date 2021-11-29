using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class ShipContent : GridContent
    {
        public virtual string Name { get; } = "Ship";
        public virtual int Size { get; }
        public virtual int Health { get; set; } = 1;

        public override char Symbol => ' ';

        public override int Score { get; set; } = 0;

        public ShipContent(Player p1)
        {
            p1.TotalHealth += Size;
        }
        public bool IsSunk()
        {
            return Health < 1;
           
        }

        public override string ReturnHitMessage()
        {
            string hitMessage = " hit";
            if (GameSettings.salvoMode)
                hitMessage += " a ...Ship?";
            return hitMessage += "!?";
        }
        public string ReturnSunkMessage()
        {
            return "You sunk my " + (GameSettings.salvoMode == true ? Name : "battleship") + "!"; // In salvo mode, you must provide more information.
        }
    }
}
