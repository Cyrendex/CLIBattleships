using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    class Grid
    {
        CoordinateLetter letter;
        int number;
        GridType type;
        public Grid(CoordinateLetter letter, int number) {
            this.letter = letter;
            this.number = number;
            type = GridType.Empty;
        }

        public GridType getType() 
        {
            return type;
        }
        
    }
}
