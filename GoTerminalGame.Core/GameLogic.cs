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
    public class GameLogic
    {
        /// <summary>
        ///     Determines if the specified space is free.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>A value indicating whether the space is free.</returns>
        public static bool IsSpaceFree(GameState gameState, int row, int col)
        {
            return gameState.State[row, col] is null;
        }

        /// <summary>
        ///     Identifies the groups of stones which are adjacent to the specified space.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <param name="row">The row from which to check for adjacent groups.</param>
        /// <param name="col">The column from which to check for adjacent groups.</param>
        /// <returns>A list of adjacent groups.</returns>
        public static List<List<(int, int)>> GetAdjacentGroups(GameState gameState, int row, int col)
        {
            char activeColor = gameState.ColorToPlay[0];
            char otherColor = activeColor == 'b' ? 'w' : 'b';

            // Stores coordinates of adjacent groups
            List<List<(int, int)>> groups = new List<List<(int, int)>>();
            
            // Stores whether the spot has been visited
            bool[,] visited = new bool[gameState.State.GetLength(0), gameState.State.GetLength(1)];
        
            var adjacentSpaces = new (int, int)[]
            {
                (row - 1, col),
                (row + 1, col),
                (row, col - 1),
                (row, col + 1)
            };

            foreach ((int r, int c) in adjacentSpaces)
            {
                List<(int, int)> group = new List<(int, int)>();
                DFS(gameState.State, r, c, otherColor, visited, group);

                if (group.Count > 0)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

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

        /// <summary>
        ///     Determines if the specified group is captured.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <param name="coordinates">A list of coordinates representing the group.</param>
        /// <returns>A boolean value indicating whether the group is captured.</returns>
        public static bool IsGroupCaptured(GameState gameState, List<(int, int)> coordinates)
        {   
            var adjacentSpaces = new (int, int)[4];

            foreach ((int row, int col) in coordinates)
            {
                adjacentSpaces = new (int, int)[]
                {
                    (row - 1, col),
                    (row + 1, col),
                    (row, col - 1),
                    (row, col + 1)
                };

                foreach ((int r, int c) in adjacentSpaces)
                {
                    if (r < 0 || r >= gameState.State.GetLength(0) || c < 0 || c >= gameState.State.GetLength(1))
                    {
                        continue;
                    }
                    if (gameState.State[r, c] is null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Determines if the specified stone belongs to a captured group.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>A boolean value indicating whether the stone belongs to a captured group.</returns>
        public static bool IsGroupCaptured(GameState gameState, int row, int col)
        {
            List<(int, int)> group = new List<(int, int)>();
            bool[,] visited = new bool[gameState.State.GetLength(0), gameState.State.GetLength(1)];

            DFS(gameState.State, row, col, gameState.State[row, col], visited, group);

            return IsGroupCaptured(gameState, group);
        }
    }
}