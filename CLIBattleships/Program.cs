using System;
using System.Linq;

namespace CLIBattleships
{
    class Program
    {
        static void Main(string[] args)
        {
            Player p1, p2;
            GameHandler.AskSalvo();
            GameHandler.InitializePlayers(out p1, out p2);
            GameHandler.GameLoop(p1, p2);
        }
    }
}
