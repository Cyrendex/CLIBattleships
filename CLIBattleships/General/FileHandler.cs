using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLIBattleships
{
    public static class FileHandler
    {
        // Change this to your own full path of GameSettings.txt
        private static readonly string path = @"C:\Users\Aaron\Source\Repos\CLIBattleships\CLIBattleships\General\GameSettings.txt";
        private static string[] Settings { get; } = File.ReadAllLines(path);
        public static string ReturnValueAfterKeyword(string keyword)
        {
            foreach (string line in Settings)
            {
                if (line.Contains(keyword))
                    return line.Substring(line.IndexOf('=') + 1);
            }
            return null;
        }
    }
}
