using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class Grid
    {
        public CoordinateLetter Letter { get; set; }
        public int Number { get; set; }
        GridType type; // To be deleted after implementing GridContent
        public GridContent Content { get; set; }
        public GridState State { get; set; }

        public Grid(CoordinateLetter letter, int number) {
            Letter = letter;
            Number = number;
            type = GridType.Empty;
        }
        public Grid(CoordinateLetter letter, int number, GridType type)
        {
            Letter = letter;
            Number = number;
            this.type = type;
        }
        public Grid(CoordinateLetter letter, int number, GridContent content)
        {
            Letter = letter;
            Number = number;
            Content = content;
            State = GridState.NotAttacked;
        }

        public GridType GetType() 
        {
            return type;
        }

        public void SetType(GridType type)
        {
            this.type = type;
        }

        // New methods from this point on.

        public char GetSymbol(bool ownGrid)
        {
            if (State == GridState.Attacked)
            {
                if (Content is EmptyContent)
                    return Symbols.ATTACKED_SYMBOL;
                else
                    return Symbols.HIT_SYMBOL;
            }

            if (ownGrid && Content is ShipContent)
                    return Content.Symbol;
            else
                return Symbols.EMPTY_SYMBOL;
        }

        public void PrintAttackedMessage()
        {
            if (Content is EmptyContent)
                Console.WriteLine("You already attacked this grid!");
            else
                Console.WriteLine("You already hit a ship here!");
            
        }
    }
}
