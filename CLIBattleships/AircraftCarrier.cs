using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class AircraftCarrier : ShipContent
    {
        public override string Name => "Aircraft Carrier";
        public override char Symbol => Symbols.AIRCRAFT_CARRIER_SYMBOL;
        public override int Size { get; } = 5;
        public override int Health { get; set; } = 5;
        public override int Score { get; set; } = 1000;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            string hitMessage = " hit";
            if (isSalvoVariation)
                hitMessage += " an Aircraft Carrier";
            return hitMessage += "!";
        }
    }
}
