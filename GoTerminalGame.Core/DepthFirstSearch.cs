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
    public class DepthFirstSearch
    {
        /// <summary>
        ///     Performs a depth-first search on the game board to identify groups of adjacent spaces with the same value.
        /// </summary>
        /// <param name="grid">A 2SD array representing the current state of the game board.</param>
        /// <param name="row">The row to start the search from.</param>
        /// <param name="col">The column to start the search from.</param>
        /// <param name="targetColor">The color to search for.</param>
        /// <param name="visited">An array to keep track of visited spaces.</param>
        /// <param name="coordinates">An array to store the coordinates of spaces in the group.</param>
        public static void DFS (char?[,] grid, int row, int col, char? targetColor, bool[,] visited, List<(int, int)> coordinates)
        {
            int boardSize = grid.GetLength(0);  

            if (row < 0 || row >= boardSize || col < 0 || col >= boardSize || visited[row, col] || grid[row, col] != targetColor)
            {
                return;
            }

            visited[row, col] = true;
            coordinates.Add((row, col));

            // Visit adjacent spots (up, down, left, right)
            DFS(grid, row - 1, col, targetColor, visited, coordinates);
            DFS(grid, row + 1, col, targetColor, visited, coordinates);
            DFS(grid, row, col - 1, targetColor, visited, coordinates);
            DFS(grid, row, col + 1, targetColor, visited, coordinates);
        }
    }
}