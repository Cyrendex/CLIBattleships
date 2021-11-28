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

        public bool IsSunk()
        {
            return Health < 1;
        }
        public void ReduceHealth()
        {
            Health--;
        }

        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a ...Ship?";
            return hitMessage += "!?";
        }
    }
}
