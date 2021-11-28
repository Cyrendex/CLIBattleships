﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    internal class Submarine : ShipContent
    {
        public override string Name => "Submarine";
        public override char Symbol => Symbols.SUBMARINE_SYMBOL;
        public override int Size { get; } = 3;
        public override int Health { get; set; } = 3;
        public override int Score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " a Submarine";
            return hitMessage += "!";
        }
    }
}
