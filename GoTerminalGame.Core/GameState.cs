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
    ///     Represents the state of a Go game, including the board, the current player's turn, and the captured stones.
    /// </summary>
    public class GameState
    {
        /// <summary>
        ///     Gets or sets the state of the game board.
        /// </summary>
        public char?[,] State { get; private set; }
        
        /// <summary>
        ///     Gets or sets the color (player) to play the next move.
        /// </summary>
        public string ColorToPlay { get; private set; }

        /// <summary>
        ///     Gets or sets the number of captured black stones.
        /// </summary>        
        public int CapturedBlack { get; set; }

        /// <summary>
        ///     Gets or sets the number of captured white stones.
        /// </summary>
        public int CapturedWhite { get; set; }

        // Private fields:
        private readonly int _boardSize;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        /// <exception cref="ArgumentException">Throws when board size is not 9, 13 or 19.</exception>
        public GameState(int size)
        {
            if (size != 19 && size != 13 && size != 9)
            {
                throw new ArgumentException("Size must be 9, 13 or 19.", nameof(size));
            }

            _boardSize = size;
            State = new char?[size, size];

            CapturedWhite = 0;
            CapturedBlack = 0;

            ColorToPlay = "black";

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    State[i, j] = null;
                }
            }   
        }

        /// <summary>
        ///     Advances the color to play to the next player's color.
        /// </summary>
        public void NextColorToPlay()
        {
            ColorToPlay = ColorToPlay == "black" ? "white" : "black";
        }

        /// <summary>
        ///     Gets the color of the stone at the given coordinates.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        /// <returns>A character representing the color of the stone or null if there is no stone there.</returns>
        public char? GetStoneColor(int row, int col) {
            return State[row, col];
        }

        /// <summary>
        ///     Updates the state of the game.
        /// </summary>
        /// <param name="updatedState">A 2D array representing the updated game state.</param>
        /// <exception cref="ArgumentException">Throws when the updated state uses the wrong board size.</exception>
        public void UpdateState(char?[,] updatedState)
        {
            if (updatedState.GetLength(0) != _boardSize || updatedState.GetLength(1) != _boardSize)
            {
                throw new ArgumentException("State must be of size " + _boardSize + "x" + _boardSize + ".", nameof(updatedState));
            }
            State = updatedState;
        }

        /// <summary>
        ///     Adds a stone of the specified color to the game board.
        /// </summary>
        /// <param name="row">The row to place the stone in.</param>
        /// <param name="col">The column to place the stone in.</param>
        /// <exception cref="ArgumentException">Throws when the coordinates are out of bounds.</exception>
        /// <exception cref="ArgumentException">TThrows when the space is already occupied by another stone.</exception>
        public void AddStone(int row, int col)
        {
            if (row < 0 || row >= _boardSize || col < 0 || col >= _boardSize)
            {
                throw new ArgumentException("Coordinates must be between 0 and " + (_boardSize - 1) + ".", nameof(row) + " or " + nameof(col));
            }

            if (State[row, col] is not null)
            {
                throw new ArgumentException("There is already a stone there.", nameof(row) + " or " + nameof(col));
            }
            else
            {
                State[row, col] = ColorToPlay[0];
            }
        }

        /// <summary>
        ///     Removes a stone from the game board.
        /// </summary>
        /// <param name="row">The row of the stone to be removed.</param>
        /// <param name="col">The column of the stone to be removed.</param>
        /// <exception cref="ArgumentException">Throws when the coordinates are out of bounds.</exception>
        /// <exception cref="ArgumentException">Throws when space is empty.</exception>
        public void RemoveStone(int row, int col)
        {
            if (col < 0 || col >= _boardSize || row < 0 || row >= _boardSize)
            {
                throw new ArgumentException("Coordinates must be between 0 and " + (_boardSize - 1) + ".", nameof(col) + " or " + nameof(row));
            }
            if (State[row, col] is null)
            {
                throw new ArgumentException("There is no stone there.", nameof(col) + " or " + nameof(row));
            }
            else
            {
                State[row, col] = null;
            }
        }

        /// <summary>
        ///     Removes a list of stones from the game board by calling <see cref="RemoveStone(int, int)"/>
        /// </summary>
        /// <param name="coordinates">A list of coordinates representing the stones to be removed.</param>
        public void RemoveStones(List<(int row, int col)> coordinates)
        {
            for (int i = 0; i < coordinates.Count(); i++)
            {
                RemoveStone(coordinates[i].row, coordinates[i].col);
            }
        }

        /// <summary>
        ///     Gets the current state of the game as a 2D array. 
        /// </summary>
        /// <returns>A 2D array representing the current state of the game.</returns>
        public string?[] GetGameState () 
        {
            // Flatten the array
            char?[] state = State.Cast<char?>().ToArray();

            string?[] gameStateSummary = new string[state.Length + 1];
            
            gameStateSummary[0] = $"{ColorToPlay}:";

            for (int i = 1; i < state.Length; i++)
            {
                gameStateSummary[i] =   state[i].ToString();
            }
            return gameStateSummary;
        }

        /// <summary>
        ///     Determines if two game states are equal.
        /// </summary>
        /// <param name="obj">An object to compare to.</param>
        /// <returns>A boolean indicating if the objects are considered equal.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }
            GameState other = (GameState)obj;
            return Equals(other);
        }

        /// <summary>
        ///     Determines if two game states are equal by comparing the stones on the board and the active player's color.
        /// </summary>
        /// <param name="other">The other game state to compare to.</param>
        /// <returns>A boolean indicating if the objects are considered equal.</returns>
        public bool Equals (GameState other)
        {
            if (other is null)
            {
                return false;
            }
            if (_boardSize != other._boardSize || ColorToPlay != other.ColorToPlay)
            {
                return false;
            }

            for (int i = 0; i < _boardSize; i++)
            {
                for (int j = 0; j < _boardSize; j++)
                {
                    if (State[i, j] != other.State[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Gets the hash code of the game state, represented by the stones on the board and the active player's color.
        /// </summary>
        /// <returns>A hash code of the game state.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(_boardSize);
            hash.Add(ColorToPlay);

            foreach (var element in State)
            {
                hash.Add(element);
            }

            return hash.ToHashCode();
        }
    }
}   