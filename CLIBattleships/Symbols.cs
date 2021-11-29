using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class Symbols
    {
        public const char AIRCRAFT_CARRIER_SYMBOL = 'O';
        public const char BATTLESHIP_SYMBOL = 'O';
        public const char DESTROYER_SYMBOL = 'O';
        public const char SUBMARINE_SYMBOL = 'O';
        public const char PATROL_SYMBOL = 'O';
        public const char EMPTY_SYMBOL = '-';
        public const char HIT_SYMBOL = 'X';      // If attacked and the grid had a ShipContent
        public const char ATTACKED_SYMBOL = '/'; // If attacked and the grid had an EmptyContent
        public const char TEST_SHIP_SYMBOL = 'O';
    }
}
