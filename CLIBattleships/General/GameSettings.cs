using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class GameSettings
    {
        public static readonly int GridXSize = Convert.ToInt32(FileHandler.ReturnValueAfterKeyword("XSize")); // Length of the x-axis of the grid plane. (The letters) MAX: 26
        public static readonly int GridYSize = Convert.ToInt32(FileHandler.ReturnValueAfterKeyword("YSize")); // Length of the y-axis of the grid plane. (The numbers) MAX: Be reasonable. Don't pass the integer limit.
        public static bool salvoMode = false; // Decides the game mode | false: default, true: salvo
        public static readonly int DefaultNumberOfShots = Convert.ToInt32(FileHandler.ReturnValueAfterKeyword("DefaultNumberOfShots")); // Should be one normally, giving the player the freedom to change it.
        public static readonly int NameCharacterLimit = Convert.ToInt32(FileHandler.ReturnValueAfterKeyword("NameCharacterLimit")); // Upper limit for the length of a player's name;
    }
}
