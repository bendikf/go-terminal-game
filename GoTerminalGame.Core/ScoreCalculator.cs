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
    public class ScoreCalculator
    {
        private static int _komi = 7;  // Default komi is 7 for area scoring.

        /// <summary>
        ///     Calculates the player's scores from the given game state.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <returns>The player's scores as a tuple.</returns>
        public static (int, int) CalculateScore(GameState gameState)
        {
            // Initialize scores, accounting for komi
            int blackScore = 0;
            int whiteScore = _komi;

            // Count the captured pieces
            blackScore += gameState.CapturedBlack;
            whiteScore += gameState.CapturedWhite;

            List<List<(int, int)>> areas = FindAllAreas(gameState);

            foreach (List<(int, int)> area in areas)
            {
                ScoreArea(area, gameState, ref blackScore, ref whiteScore);
            }

            return (blackScore, whiteScore);
        }

        /// <summary>
        ///     Finds all areas to be scored on the board.
        /// </summary>
        /// <param name="gameState">The current game state.</param>
        /// <returns>A list of areas to be scored.</returns>
        private static List<List<(int, int)>> FindAllAreas(GameState gameState)
        {
            List<List<(int, int)>> areas = new List<List<(int, int)>>();
            bool[,] visited = new bool[gameState.State.GetLength(0), gameState.State.GetLength(1)];

            for (int row = 0; row < gameState.State.GetLength(0); row++)
            {
                for (int col = 0; col < gameState.State.GetLength(1); col++)
                {
                    List<(int, int)> area = new List<(int, int)>();
                    GameLogic.DFS(gameState.State, row, col, null, visited, area);

                    if (area.Count > 0)
                    {
                        areas.Add(area);
                    }
                }
            }

            Console.WriteLine($"Areas: {areas.Count}");

            return areas;
        }

        /// <summary>
        ///     Scores an area on the board if it is completely enclosed by one player's color or the edge of the game board.
        /// </summary>
        /// <param name="area">The coordinates of the empty spaces in area to be scored.</param>
        /// <param name="gameState">The current game state.</param>
        /// <param name="blackScore">A reference to the black player's score.</param>
        /// <param name="whiteScore">A reference to the white player's score.</param>
        private static void ScoreArea(List<(int, int)> area, GameState gameState, ref int blackScore, ref int whiteScore)
        {   
            char? colorToScore = null;
            
            foreach ((int row, int col) in area)
            {
                var adjacentSpaces = new (int, int)[]
                {
                    (row - 1, col),
                    (row + 1, col),
                    (row, col - 1),
                    (row, col + 1)
                };
                
                foreach ((int r, int c) in adjacentSpaces)
                {
                    if (r < 0 || r >= gameState.State.GetLength(0) || c < 0 || c >= gameState.State.GetLength(1) || gameState.State[r, c] is null)
                    {
                        continue;
                    }
                    colorToScore ??= gameState.State[r, c];

                    if (gameState.State[r, c] != colorToScore)
                    {
                        return;
                    }
                }
            }

            // Score the area. 
            // colorToScore is either 'b', 'w' or null. Must check both.
            if (colorToScore == 'b')
            {
                blackScore += area.Count;
            }
            else if (colorToScore == 'w')
            {
                whiteScore += area.Count;
            }
        }

        /// <summary>
        ///     Gets the komi value.
        /// </summary>
        /// <returns>An integer representing the komi value.</returns>
        public static int GetKomi()
        {
            return _komi;
        }
    }
}