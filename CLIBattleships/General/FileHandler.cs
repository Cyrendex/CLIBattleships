using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLIBattleships
{
    public static class FileHandler
    {
        private static string[] Settings { get; set; }
        private static string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string file = dir + @"\General\GameSettings.txt";
        private static StreamReader sr = new StreamReader(file);
        static FileHandler()
        {
            string settings = "", line;
            while((line = sr.ReadLine()) != null)
            {
                settings += line + "\n"; 
            }
            Settings = settings.Split("\n");
        }
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
