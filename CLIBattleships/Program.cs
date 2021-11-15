using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CLIBattleships
{
    class Program
    {
        static bool CoordinateHandler(string coordinate, out CoordinateLetter letter, out int number)
        {
            // Coordinates need a letter and a number ranging from A-J and 1-10 respectively, inclusive.
            coordinate.ToUpper();
            int length = coordinate.Length;
            if (length <= 3 && length > 1) // A healthy coordinate is either 2 or 3 characters long (A1, B10)
                if (coordinate.Count(char.IsLetter) == 1 && char.IsLetter(coordinate, 0)) // Checks if there is only one letter. Also checks if the first digit is a letter.
                {
                    int num = Int32.Parse(coordinate.Substring(1, length - 1).TrimStart('0').PadLeft(1, '0')); // Extract number from coordinate
                    if (num > 0 || num <= 10)
                    {
                        number = num;
                        bool pass = Enum.TryParse(coordinate.Substring(0, 1), out CoordinateLetter let);
                        letter = let;
                        return pass;
                    }
                }
            number = -1; // Temp values, the method will loop until the if conditions are satisfied.
            letter = 0;
            return false;
        }
        static void Main(string[] args)
        {
            AskPlayerName(out string p1Name, out string p2Name);
            GridPlane gridOne = new GridPlane();
            GridPlane gridTwo = new GridPlane();

            InitPlayer(p1Name, gridOne);
            InitPlayer(p2Name, gridTwo);

            Console.ReadKey();
        }
        static Player InitPlayer(string name, GridPlane grid)
        {
            Player player = new Player(name, grid);
            return player;
        }

        static void AskPlayerName(out string p1Name, out string p2Name)
        {
            bool valid = true;
            do
            {
                Console.Write("Enter a name for player one: ");
                p1Name = Console.ReadLine().Trim();
                if (p1Name.Length > 15)
                {
                    Console.WriteLine("Name too long, keep it shorter than 16 characters.");
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p1Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player One\"");
                    p1Name = "Player One";
                    valid = true;
                }
                else
                    valid = true;
            } while (!valid);
            Console.WriteLine("\n\n");

            do
            {
                Console.Write("Enter a name for player two: ");
                p2Name = Console.ReadLine().Trim();
                if (p2Name.Length > 15)
                {
                    Console.WriteLine("Name too long, keep it shorter than 16 characters.");
                    valid = false;
                }
                else if (string.IsNullOrEmpty(p2Name))
                {
                    Console.WriteLine("Name can't be left empty, defaulting to \"Player One\"");
                    p2Name = "Player Two";
                    valid = true;
                }
                else if(p1Name.Equals(p2Name))
                {
                    Console.WriteLine("Name can't be the same as player one, try again.");
                    valid = false;
                }
                else
                    valid = true;
            } while (!valid);
            Console.Clear();           
        }

    }
}
