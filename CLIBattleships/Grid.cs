using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class Grid
    {
        public CoordinateLetter Letter { get; set; }
        public int Number { get; set; }
        public GridContent Content { get; set; }
        public GridState State { get; set; }

        public Grid(CoordinateLetter letter, int number, GridContent content)
        {
            Letter = letter;
            Number = number;
            Content = content;
            State = GridState.NotAttacked;
        }

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
                Console.WriteLine("You already attacked this grid! Try again.");
            else
                Console.WriteLine("You already hit a ship here! Try again.");           
        }
    }
}
