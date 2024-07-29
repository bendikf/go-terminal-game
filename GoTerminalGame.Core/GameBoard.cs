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
    public class GameBoard
    {
        public int Size { get; init; }
        public string[,] EmptyBoardLayout { get; init; }
        public string[,] PopulatedBoard { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameBoard"/> class.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        /// <exception cref="ArgumentException">Throws when board size is not 9, 13 or 19.</exception>
        public GameBoard(int size)
        {
            if (size != 19 && size != 13 && size != 9)
            {
                throw new ArgumentException("Size must be 9, 13 or 19.", nameof(size));
            }
            Size = size;
            EmptyBoardLayout = SetupBoard(size);
            PopulatedBoard = (string[,])EmptyBoardLayout.Clone();
        }

        /// <summary>
        ///     Populates the game board with the playing pieces from the game state.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        public void Populate(GameState gameState)
        {
            PopulatedBoard = (string[,])EmptyBoardLayout.Clone();

            for (int i = 0; i < Size; i++)
            {   
                for (int j = 0; j < Size; j++)
                {
                    if (gameState.State[i, j] is not null)
                    {
                        PopulatedBoard[i, j] = gameState.State[i, j].ToString() ?? string.Empty;
                    }
                }
            }   
        }

        /// <summary>
        ///     Sets up the layout of the game board according to the board size.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        /// <returns>A 2D array representing the game board layout.</returns>
        private string[,] SetupBoard(int size)
        {
            string[,] layout = new string[size, size];
            
            // Add grid lines:
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    switch (i, j)
                    {
                        // Corners:
                        case (0, 0):
                            layout[i,j] = "┌";
                            break;
                        case (0, var col) when col == size - 1:
                            layout[i,j] = "┐";
                            break;
                        case (var row, 0) when row == size - 1:
                            layout[i,j] = "└";
                            break;
                        case (var row, var col) when row == size - 1 && col == size - 1:
                            layout[i,j] = "┘";
                            break;

                        // Edges:
                        case (0, _):
                            layout[i,j] = "┬";
                            break;
                        case (var row, _) when row == size - 1:
                            layout[i,j] = "┴";
                            break;
                        case (_, 0):
                            layout[i,j] = "├";
                            break;
                        case (_, var col) when col == size - 1:
                            layout[i,j] = "┤";
                            break;

                        // Center star point:
                        case (var col, var row) when col == size / 2 && row == size / 2: 
                            layout[i,j] = "┿";
                            break;  

                        // Default:
                        default:
                            layout[i,j] = "┼";
                            break;
                    }
                }
            }

            // Add board size specific star points:    
            if (size == 9)  // 9x9 board
            {
                // Star points on the 3-3 points:
                layout[2, 2] = "┿";
                layout[2, size - 3] = "┿";
                layout[size - 3, 2] = "┿";
                layout[size - 3, size - 3] = "┿";
            }
            else if (size != 9)  // 13x13 or 19x19 board
            {
                // Star points on the 4-4 points:
                layout[3, 3] = "┿";
                layout[3, size - 4] = "┿";
                layout[size - 4, 3] = "┿";
                layout[size - 4, size - 4] = "┿";

                if (size == 19)  // 19x19 board
                {
                    // Star points between the 4-4 points:
                    layout[3, size / 2] = "┿";
                    layout[size / 2, 3] = "┿";
                    layout[size - 4, size / 2] = "┿";
                    layout[size / 2, size - 4] = "┿";
                }
            }

            return layout;
        }
    }
}