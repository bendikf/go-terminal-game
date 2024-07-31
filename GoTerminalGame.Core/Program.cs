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
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    /// <remarks>
    ///     This class contains the Main method, which is the entry point for the application.
    ///     It initializes the application and calls necessary methods to start the process.
    /// </remarks>
    public class Program
    {
        /// <summary>
        ///     The main entry point for the Go Terminal Game application. Prompts the user for a board size and starts a new game on a board of that size.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            char choice = SelectMenu.SelectGameMode();

            switch (choice)
            {
                case 'a':
                case '\r':
                    Game game19x19 = new Game(19);
                    game19x19.Play();
                    break;
                case 'b':
                    Game game13x13 = new Game(13);
                    game13x13.Play();
                    break;
                case 'c':
                    Game game9x9 = new Game(9);
                    game9x9.Play();
                    break;
                case 'd':
                    break;
                default:
                    throw new ArgumentException("Error selecting board size.");
            }
        }
    }
}   