using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class GameHandler
    {
        public static void InitializePlayers(out Player p1, out Player p2)
        {
            string p1Name, p2Name;
            Grid[][] p1GridPlane = GridPlaneHandler.MakeGridPlane();
            Grid[][] p2GridPlane = GridPlaneHandler.MakeGridPlane();
            AskPlayerName(out p1Name, out p2Name);
            p1 = new Player(p1Name, p1GridPlane);
            p2 = new Player(p2Name, p2GridPlane);
            p1.SetShipsOnGridPlane();
            Console.Clear();
            Console.WriteLine(p2.Name + ", press any key to proceed.");
            Console.ReadKey();
            Console.Clear();
            p2.SetShipsOnGridPlane();
        }
        static void AskPlayerName(out string p1Name, out string p2Name)
        {
            bool valid;
            do
            {
                Console.Write("Enter a name for player one: ");
                p1Name = Console.ReadLine().Trim();
                if (p1Name.Length > GameSettings.NAME_CHARACTER_LIMIT)
                {
                    Console.WriteLine("Name too long, can't be longer than {0} characters.", GameSettings.NAME_CHARACTER_LIMIT);
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p1Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player One.\"");
                    p1Name = "Player One";
                    valid = true;
                }
                else
                    valid = true;
            } while (!valid);
            Console.WriteLine();

            do
            {
                Console.Write("Enter a name for player two: ");
                p2Name = Console.ReadLine().Trim();
                if (p2Name.Length > GameSettings.NAME_CHARACTER_LIMIT)
                {
                    Console.WriteLine("Name too long, can't be longer than {0} characters.", GameSettings.NAME_CHARACTER_LIMIT);
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p2Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player Two.\"");
                    p2Name = "Player Two";
                    valid = true;
                }
                else if (p1Name.Equals(p2Name))
                {
                    Console.WriteLine("Name can't be the same as player one, try again.");
                    valid = false;
                }
                else
                    valid = true;
            } while (!valid);
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
        }
        public static void  AskSalvo()
        {
            Console.WriteLine("Which variaton would you like to play? (1 or 2)\n(1) Classic (Default)\n(2) Salvo Variation (Advanced)");
            Console.Write("Selection: ");
            string input = Console.ReadLine();
            Console.WriteLine();
            if (input.Equals("1"))
            {
                Console.WriteLine("You chose the Classic variation, have fun!");
                System.Threading.Thread.Sleep(2500);
                GameSettings.salvoMode = false;
            }
            else if (input.Equals("2"))
            {
                Console.WriteLine("You chose the Salvo variation, good luck!");
                System.Threading.Thread.Sleep(2500);
                GameSettings.salvoMode = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Defaulting to Classic variation.");
                System.Threading.Thread.Sleep(2500);
                GameSettings.salvoMode = false;
            }
            Console.Clear();
        }
        public static void GameLoop (Player p1, Player p2)
        {
            Console.Clear();
            Player currentPlayer = CoinFlip(p1, p2);
            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                Console.WriteLine(currentPlayer.Name + ", it's your turn! Press any key to proceed.");
                Console.ReadKey();
                Console.Clear();
                string report = "";
                
                report += "-----------------------------------\n";
                if(currentPlayer == p1)
                    report += currentPlayer.ShootAndReportStatus(p2);
                else
                    report += currentPlayer.ShootAndReportStatus(p1);
                report += "-----------------------------------";
                Console.WriteLine(report);
                Console.WriteLine("\nOnce you're ready to proceed, press any key.");
                Console.ReadKey();

                if (p1.TotalHealth <= 0 || p2.TotalHealth <= 0)
                    gameOver = true;
                else
                    SwitchTurns(ref currentPlayer, p1, p2);
            }
            PrintEndScreen(currentPlayer, p1, p2);
        }
        private static void PrintEndScreen(Player winner, Player p1, Player p2)
        {
            Console.Clear();
            Console.WriteLine(p1.Name + ":");
            p1.DrawGridPlane(gameEnded: true, true);
            Console.WriteLine(p2.Name + ":");
            p2.DrawGridPlane(gameEnded: true, true);
            Console.WriteLine();
            Console.WriteLine("Congratulations, " + winner.Name + "! You destroyed all the enemy ships!\n(Press any key to end the game)");
            Console.ReadKey();
        }
        private static void SwitchTurns(ref Player currentPlayer, Player p1, Player p2)
        {
            if (currentPlayer == p1)
                currentPlayer = p2;
            else
                currentPlayer = p1;
        }
        private static Player CoinFlip(Player p1, Player p2)
        {
            Console.WriteLine("A coin flip will commence to decide who will start. (Press any key to continue)");
            Console.ReadKey();
            Random rnd = new Random();
            int num = rnd.Next(1, 10);
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("...");
            System.Threading.Thread.Sleep(800);
            Console.WriteLine("...");
            System.Threading.Thread.Sleep(800);
            if (num % 2 == 0)
            {
                Console.Write(p1.Name + " won the coin flip! Press any key to continue when you're ready.");
                Console.ReadKey();
                return p1;
            }
            else
            {
                Console.Write(p2.Name + " won the coin flip! Press any key to continue when you're ready.");
                Console.ReadKey();
                return p2;
            }
        }
    }
}
