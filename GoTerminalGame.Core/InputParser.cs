/*
 * This file is part of "Go Terminal Game".
 *
 * Go Terminal Game is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Go Terminal Game is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Go Terminal Game. If not, see <https://www.gnu.org/licenses/>.
 *
 * Author: Bendik J. Ferkingstad
 * Date: 29.07.2024
 */

using System.Text.RegularExpressions;

namespace GoTerminalGame.Core
{
    /// <summary>
    ///     Provides methods to read and parse user inputs.
    /// </summary>
    /// <remarks>
    ///     The <see cref="InputParser"/> class includes methods to validate 
    ///     and convert user inputs into usable formats for the application.
    /// </remarks>
    public class InputParser
    {
        /// <summary>
        ///     Reads the player's input from the console.
        /// </summary>
        /// <returns>The player's input as an uppercase string.</returns>
        public static string ReadInput()
        {
            string? input;
            do
            {
                input = Console.ReadLine();
            } 
            while (string.IsNullOrEmpty(input));
            return input!.ToUpper();
        }

        /// <summary>
        ///     Parses the input string into a row and column number.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        /// <returns>A tuple containing the row and column number.</returns>
        public static (int row, int col) ParseInput(string input)
        {
            string pattern =  @"^(?=.*[A-S])(?=.*[1-9]|1[0-9]).*$"; 

            if (!Regex.IsMatch(input, pattern))
            {
                throw new ArgumentOutOfRangeException();
            }

            char letter = '\0';     
            int number = -1;

            // Parse the letter and number from input
            foreach (char ch in input)
            {
                if (char.IsLetter(ch))
                {
                    letter = ch;
                }
                else if (char.IsDigit(ch))
                {
                    number = number == -1 ? int.Parse(ch.ToString()) : number * 10 + int.Parse(ch.ToString());
                }
            }

            // Convert letter and number to row and column number
            int col = letter - 'A';
            int row = number - 1;
            
            return (row, col);
        }
    }
}