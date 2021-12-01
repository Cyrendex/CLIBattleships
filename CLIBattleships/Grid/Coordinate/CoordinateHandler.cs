using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIBattleships
{
    public static class CoordinateHandler
    {
        /* Takes in a string prompt, splits it into two usable values. Returns temp values if the coordinate given isn't valid */
        public static void CoordinateSplitter(string coordinate, out CoordinateLetter coordinateLetter, out int coordinateNumber)
        {
            coordinateNumber = -1; // Temp value
            coordinateLetter = 0; // Temp value
            coordinate.ToUpper();
            int length = coordinate.Length;
            if (length < 12 && length > 1) // A healthy coordinate can now be very long. Letter + 11 digits from integer limit. Please don't try this.
                if (coordinate.Count(char.IsLetter) == 1 && char.IsLetter(coordinate, 0)) // Checks if there is only one letter. Also checks if the first digit is a letter.
                {
                    int num = Int32.Parse(coordinate.Substring(1, length - 1).TrimStart('0').PadLeft(1, '0')); // Extract number from coordinate
                    Enum.TryParse(coordinate.Substring(0, 1).ToUpper(), out CoordinateLetter letter);
                    bool letterValid = (int)letter <= GameSettings.GridXSize; // Checks if the letter is in bound of global size. MAX: 26
                    if (num > 0 && num <= GameSettings.GridYSize && letterValid) // Seriously, don't try this. MAX: int limit
                    {
                        coordinateNumber = num;
                        coordinateLetter = letter;
                    }
                }
        }
        /* Keeps prompting until a valid coordinate is given */
        public static void CoordinateAsker(out CoordinateLetter coordinateLetter, out int coordinateNumber)
        {
            do
            {
                Console.Write("Please enter a coordinate: ");
                CoordinateHandler.CoordinateSplitter(Console.ReadLine(), out coordinateLetter, out coordinateNumber);
                if (coordinateNumber == -1)
                    Console.WriteLine("\nInvalid coordinate.");
            } while (coordinateNumber == -1);
        }
    }
}
