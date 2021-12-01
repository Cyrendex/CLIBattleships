using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public static class GameHandler
    {
        public static void InitializePlayers(out Player p1, out Player p2)
        {
            Grid[][] p1GridPlane = GridPlaneHandler.MakeGridPlane();
            Grid[][] p2GridPlane = GridPlaneHandler.MakeGridPlane();
            p1 = new Player(p1GridPlane);
            p2 = new Player(p2GridPlane);
            Player[] playerList = new Player[] {p1, p2};
            playerList[0].Name = AskPlayerName(playerList);
            playerList[1].Name = AskPlayerName(playerList);
            
            playerList[0].SetShipsOnGridPlane();
            Console.Clear();
            Console.WriteLine(playerList[1].Name + ", press any key to proceed.");
            Console.ReadKey();
            Console.Clear();
            playerList[1].SetShipsOnGridPlane();
        }
        private static string AskPlayerName(Player[] playerList)
        {
            string name = "";
            bool valid = false;
            foreach (Player player in playerList)
            {
                int playerNumber = Array.IndexOf(playerList, player) + 1;
                if (valid)
                    break;
                if (string.IsNullOrEmpty(player.Name))
                {
                    do
                    {
                        Console.Write("Enter a name for player {0}: ", playerNumber);
                        name = Console.ReadLine().Trim();
                        if (name.Length > GameSettings.NameCharacterLimit)
                        {
                            Console.WriteLine("Name too long, can't be longer than {0} characters.", GameSettings.NameCharacterLimit);
                            valid = false;
                        }
                        else if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("Name can't be left empty, defaulting to \"Player {0}.\"", playerNumber);
                            name = "Player " + playerNumber;
                            valid = true;
                        }
                        else if (playerList[0].Name.Equals(playerList[1].Name))
                        {
                            Console.WriteLine("Names of the players can't be the same.");
                            valid = false;
                        }
                        else
                            valid = true;
                    } while (!valid);
                }
            }
            System.Threading.Thread.Sleep(1000);
            Console.Clear();
            return name;
        }
        public static void AskSalvo()
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
                    report += currentPlayer.ShootAndReturnStatus(p2);
                else
                    report += currentPlayer.ShootAndReturnStatus(p1);
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
            p1.DrawGridPlane(true, true);
            Console.WriteLine(p2.Name + ":");
            p2.DrawGridPlane(true, true);
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
