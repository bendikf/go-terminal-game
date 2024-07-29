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

namespace GoTerminalGame.Core
{
    public class SelectMenu
    {
        /// <summary>
        ///     Welcomes the user to the game and prompts them to select a board size.
        /// </summary>
        /// <returns>A character representing the player's choice.</returns>
        public static char SelectGameMode()
        {
            Console.WriteLine(Environment.NewLine + "Welcome to the game of Go!".PadLeft(30) + Environment.NewLine);

            Console.WriteLine("Select a board size to play on:".PadLeft(33) + Environment.NewLine);
            Console.WriteLine("a) 19×19 (default)".PadLeft(29));
            Console.WriteLine("b) 13×13".PadLeft(19));
            Console.WriteLine("c)".PadLeft(13) + string.Format("{0, 5}", "9×9"));
            Console.WriteLine();
            Console.WriteLine("d) Exit".PadLeft(18));
            Console.WriteLine();

            char choice = '\0';
            bool validChoice = false;
            
            while (!validChoice)
            {
                choice = Console.ReadKey(intercept: true).KeyChar;
                if (choice == 'a' || choice == 'b' || choice == 'c' || choice == 'd' || choice == '\r')
                {
                    validChoice = true;
                }
            }
            return choice;  
        }
    }
}