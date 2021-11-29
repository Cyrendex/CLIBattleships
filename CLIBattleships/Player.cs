using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Player
    {
        public string Name { get; set; }
        public Grid[][] GridPlane { get; set; }
        public ShipContent[] ShipList { get; set; }
        public int NumberOfShots { get; set; }
        public int Score { get; set; }
        public int TotalHealth { get; set; }

        public Player(string name, Grid[][] plane)
        {
            Name = name;
            GridPlane = plane;
            Score = 0;            
            AircraftCarrier aircraftCarrier = new AircraftCarrier(this);
            Battleship battleship = new Battleship(this);
            Destroyer destroyer = new Destroyer(this);
            Submarine submarine = new Submarine(this);
            Patrol patrol = new Patrol(this);
            ShipList = new ShipContent[] { aircraftCarrier, battleship, destroyer, submarine, patrol };
            if (GameSettings.salvoMode)
                NumberOfShots = ShipList.Length;
            else
                NumberOfShots = GameSettings.DEFAULT_NUMBER_OF_SHOTS;
        }

        /* If it's the player's own grid, the ships will be drawn with their respective symbols. If not, they will be drawn with the empty symbol. */
        public void DrawGridPlane(bool ownGrid = false)
        {
            Console.Write("   "); // Initial space to allign letters
            for (int coordinateLetter = 0; coordinateLetter < GameSettings.GRID_YSIZE; coordinateLetter++)
            {
                Console.Write((CoordinateLetter)coordinateLetter + " ");

            }
            Console.WriteLine();
            for (int coordinateNumber = 1; coordinateNumber <= GameSettings.GRID_XSIZE; coordinateNumber++)
            {
                if (coordinateNumber < 10)
                    Console.Write(" " + coordinateNumber);
                else
                    Console.Write(coordinateNumber);

                for (int coordinateLetter = 0; coordinateLetter < GameSettings.GRID_YSIZE; coordinateLetter++)
                {
                    Console.Write(" " + GridPlane[coordinateNumber - 1][coordinateLetter].GetSymbol(ownGrid));
                   
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            // Puts '='s longer than the x axis of the grid... *3 because formatting.
            for (int length = 0; length < GameSettings.GRID_XSIZE * 3; length++)
            {
                Console.Write("=");
            }
            Console.WriteLine("\n");
        }
        /* Overloaded for end game formatting */
        public void DrawGridPlane(bool gameEnded, bool ownGrid = false)
        {
            Console.Write("   "); // Initial space to allign letters
            for (int coordinateLetter = 0; coordinateLetter < GameSettings.GRID_YSIZE; coordinateLetter++)
            {
                Console.Write((CoordinateLetter)coordinateLetter + " ");

            }
            Console.WriteLine();
            for (int coordinateNumber = 1; coordinateNumber <= GameSettings.GRID_XSIZE; coordinateNumber++)
            {
                if (coordinateNumber < 10)
                    Console.Write(" " + coordinateNumber);
                else
                    Console.Write(coordinateNumber);

                for (int coordinateLetter = 0; coordinateLetter < GameSettings.GRID_YSIZE; coordinateLetter++)
                {
                    Console.Write(" " + GridPlane[coordinateNumber - 1][coordinateLetter].GetSymbol(ownGrid));

                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Score: {0}\nHealth Left: {1}\n", Score, TotalHealth);
            // Puts '='s longer than the x axis of the grid... *3 because formatting.
            for (int length = 0; length < GameSettings.GRID_XSIZE * 3; length++)
            {
                Console.Write("=");
            }
            Console.WriteLine("\n");
        }
        public void SetShipsOnGridPlane()
        {
            int coordinateNumber1, coordinateNumber2;
            CoordinateLetter coordinateLetter1, coordinateLetter2;
            bool valid;
            foreach (ShipContent ship in ShipList)
	        {                
                do
                {
                    DrawGridPlane(true);
                    Console.WriteLine("{0}, what should be the starting point of your {1}? (Length: {2})",Name, ship.Name, ship.Size);
                    CoordinateHandler.CoordinateAsker(out coordinateLetter1, out coordinateNumber1);
                    Console.WriteLine();
                    Console.WriteLine("{0}, what should be the ending point of your {1}?",Name, ship.Name);
                    CoordinateHandler.CoordinateAsker(out coordinateLetter2, out coordinateNumber2);
                    Console.WriteLine();
                    valid = GridPlaneHandler.SizeAndCollisionChecker(ship.Name, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2, ship.Size);
                    if (!valid)
                        System.Threading.Thread.Sleep(1500);
                    Console.Clear();
                } while (!valid);

                GridPlaneHandler.ShipSetter(ship, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2);
                Console.Clear();
	        }
        }
        private string ShootShipReturnMessage(Player enemyPlayer, CoordinateLetter coordinateLetter, int coordinateNumber)
        {
            Grid grid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
            grid.State = GridState.Attacked;
            ShipContent ship = (ShipContent)grid.Content;
            ship.Health--;
            enemyPlayer.TotalHealth--;
            Score += ship.Score;
            if (ship.IsSunk())
            {
                if (GameSettings.salvoMode)
                    enemyPlayer.NumberOfShots--;
                return "" + coordinateLetter + coordinateNumber + ship.ReturnHitMessage() + "\n" + ship.ReturnSunkMessage() + "\n";
            }

            return "" + coordinateLetter + coordinateNumber + ship.ReturnHitMessage() + "\n";
        }
        private string ShootEmptyReturnMessage(Player enemyPlayer, CoordinateLetter coordinateLetter, int coordinateNumber)
        {
            Grid grid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
            grid.State = GridState.Attacked;
            EmptyContent empty = (EmptyContent)grid.Content;
            Score += empty.Score;
            return "" + coordinateLetter + coordinateNumber + empty.ReturnHitMessage() + "\n";

        }
        public string ShootAndReportStatus(Player enemyPlayer)
        {
            string message = "";
            bool shot = false;
            CoordinateLetter coordinateLetter;
            int coordinateNumber;
            for (int shotsLeft = NumberOfShots; shotsLeft > 0; shotsLeft--)
            {
                if (enemyPlayer.TotalHealth <= 0)
                    break;
                shot = false;
                while (!shot)
                {
                    enemyPlayer.DrawGridPlane();
                    Console.WriteLine("Where would you like to shoot? (" + shotsLeft + " shot(s) left)\n(If you want to view your own grid, type 'grid')\n");
                    Console.Write("Please enter a coordinate: ");
                    string prompt = Console.ReadLine();
                    if (prompt.ToLower().Equals("grid"))
                    {
                        Console.Clear();
                        DrawGridPlane(ownGrid: true);
                        Console.WriteLine("(Press any key to go back)");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        CoordinateHandler.CoordinateSplitter(prompt, out coordinateLetter, out coordinateNumber);
                        if (coordinateNumber != -1) // If the coordinate is valid
                        {
                            Grid enemyGrid = enemyPlayer.GridPlane[coordinateNumber - 1][(int)coordinateLetter];
                            if (enemyGrid.State == GridState.NotAttacked)
                            {
                                if (enemyGrid.Content is ShipContent)
                                    message += ShootShipReturnMessage(enemyPlayer, coordinateLetter, coordinateNumber);
                                else // It's EmptyContent or an unprecedented error.
                                    message += ShootEmptyReturnMessage(enemyPlayer, coordinateLetter, coordinateNumber);
                                shot = true;
                            }
                            else
                            {
                                enemyGrid.PrintAttackedMessage();
                                System.Threading.Thread.Sleep(1000);
                            }

                        }
                        else // If the coordinate isn't valid
                        {
                            Console.WriteLine("Please enter a valid coordinate.");
                            System.Threading.Thread.Sleep(1000);
                        }
                        Console.Clear();
                    }
                }
            }
            return message;
        }
    }
}
