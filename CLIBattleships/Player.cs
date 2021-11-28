using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class Player
    {
        string name;
        GridPlane plane;
        int aircraftCarrierHealth = 5;
        int battleshipHealth = 4;
        int destroyerHealth = 3;
        int submarineHealth = 3;
        int patrolHealth = 2;
        int totalHealth = 17;
        int numberOfShots = 1;
        bool isSalvo = false;
        // New properties from here and onwards
        string Name { get; set; }
        Grid[][] GridPlane { get; set; }
        ShipContent[] ShipList { get; set; }


        public Player(string name, GridPlane plane, bool isSalvo = false)
        {
            this.name = name;
            this.plane = plane;
            if (isSalvo)
            {
                this.isSalvo = true;
                numberOfShots = 5;
            }

        }
        public Player(string name, Grid[][] plane)
        {
            Name = name;
            GridPlane = plane;
            AircraftCarrier aircraftCarrier = new AircraftCarrier();
            Battleship battleship = new Battleship();
            Destroyer destroyer = new Destroyer();
            Submarine submarine = new Submarine();
            Patrol patrol = new Patrol();
            ShipList = new ShipContent[] { aircraftCarrier, battleship, destroyer, submarine, patrol };
        }

        public bool GetSalvo()
        {
            return isSalvo;
        }
        public int GetTotalHealth()
        {
            return totalHealth;
        }
        public int GetNumberOfShots()
        {
            return numberOfShots;
        }
        public ShipStatus ReduceHealth(GridType type)
        {
            switch (type)
            {
                case GridType.AircraftCarrier:
                    aircraftCarrierHealth--;
                    totalHealth--;
                    if (aircraftCarrierHealth == 0)
                    {
                        if (isSalvo)
                            ReduceShots();
                        return ShipStatus.aircraftCarrierSunk;                       
                    }
                    break;
                case GridType.Battleship:
                    battleshipHealth--;
                    totalHealth--;
                    if (battleshipHealth == 0)
                    {
                        if (isSalvo)
                            ReduceShots();
                        return ShipStatus.battleshipSunk;
                    }
                    break;
                case GridType.Destroyer:
                    destroyerHealth--;
                    totalHealth--;
                    if (destroyerHealth == 0)
                    {
                        if (isSalvo)
                            ReduceShots();
                        return ShipStatus.destroyerSunk;
                    }                       
                    break;
                case GridType.Submarine:
                    submarineHealth--;
                    totalHealth--;
                    if (submarineHealth == 0)
                    {
                        if (isSalvo)
                            ReduceShots();
                        return ShipStatus.submarineSunk;
                    }                       
                    break;
                case GridType.Patrol:
                    patrolHealth--;
                    totalHealth--;
                    if (patrolHealth == 0)
                    {
                        if (isSalvo)
                            ReduceShots();
                        return ShipStatus.patrolSunk;
                    }
                    break;
                default:
                    break;
            }
            return ShipStatus.allIsWell;
        }
        // Only for salvo variation
        public void ReduceShots()
        {
            numberOfShots--;
        }
        public GridPlane GetGridInfo()
        {
            return plane;
        }

        public string GetName()
        {
            return name;
        }

        // New methods from this point and onwards

        /* If it's the player's own grid, the ships will be drawn with their respective symbols. If not, they will be drawn with the empty symbol. */
        public void DrawGridPlane(bool ownGrid = false)
        {
            Console.Write("   "); // Initial space to allign letters
            for (int coordinateLetter = 0; coordinateLetter < GlobalConstant.GRID_YSIZE; coordinateLetter++)
            {
                Console.Write((CoordinateLetter)coordinateLetter + " ");

            }
            Console.WriteLine();
            for (int coordinateNumber = 1; coordinateNumber <= GlobalConstant.GRID_XSIZE; coordinateNumber++)
            {
                if (coordinateNumber < 10)
                    Console.Write(" " + coordinateNumber);
                else
                    Console.Write(coordinateNumber);

                for (int coordinateLetter = 0; coordinateLetter < GlobalConstant.GRID_YSIZE; coordinateLetter++)
                {
                    Console.Write(" " + GridPlane[coordinateNumber - 1][coordinateLetter].GetSymbol(ownGrid));
                   
                }
                Console.WriteLine();
            }
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
                    Console.WriteLine(Name + ", what should be the starting point of your " + ship.Name + "?");
                    CoordinateHandler.CoordinateAsker(out coordinateLetter1, out coordinateNumber1);
                    Console.WriteLine(Name + ", what should be the ending point of your " + ship.Name + "?");
                    CoordinateHandler.CoordinateAsker(out coordinateLetter2, out coordinateNumber2);
                    valid = GridPlaneHandler.SizeAndCollisionChecker(ship.Name, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2, ship.Size);
                } while (!valid);

                GridPlaneHandler.ShipSetter(ship, GridPlane, coordinateLetter1, coordinateLetter2, coordinateNumber1, coordinateNumber2);
	        }
        }
        
        public void Shoot(Player enemyPlayer)
        {
            string message;
            bool shot = false;
            CoordinateLetter coordinateLetter1;
            int coordinateNumber1;
        }
    }
}
