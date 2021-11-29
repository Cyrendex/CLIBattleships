using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class GameSettings
    {
        public const int GRID_XSIZE = 20; // Length of the x-axis of the grid plane. (The letters) MAX: 26
        public const int GRID_YSIZE = 20; // Length of the y-axis of the grid plane. (The numbers) MAX: Be reasonable. Don't pass the integer limit.
        public static bool salvoMode = false; // Decides the game mode | false: default, true: salvo
        public const int DEFAULT_NUMBER_OF_SHOTS = 10; // Should be one normally, giving the player the freedom to change it.
        public const int NAME_CHARACTER_LIMIT = 15; // Upper limit for the length of a player's name;
    }
}
