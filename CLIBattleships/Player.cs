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
    }
}
