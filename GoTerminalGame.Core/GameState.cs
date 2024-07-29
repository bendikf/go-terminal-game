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
    public class GameState
    {
        private readonly int _boardSize;
        public char?[,] State { get; private set; }
        public string ColorToPlay { get; private set; }

        public int CapturedWhite { get; set; }
        public int CapturedBlack { get; set; }

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

        public void NextColorToPlay()
        {
            ColorToPlay = ColorToPlay == "black" ? "white" : "black";
        }

        public char? GetStoneColor(int row, int col) {
            return State[row, col];
        }

        public void UpdateState(char?[,] updatedState)
        {
            if (updatedState.GetLength(0) != _boardSize || updatedState.GetLength(1) != _boardSize)
            {
                throw new ArgumentException("State must be of size " + _boardSize + "x" + _boardSize + ".", nameof(updatedState));
            }
            State = updatedState;
        }

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

        public void RemoveStones(List<(int row, int col)> coordinates)
        {
            for (int i = 0; i < coordinates.Count(); i++)
            {
                RemoveStone(coordinates[i].row, coordinates[i].col);
            }
        }

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

        public override bool Equals(object? obj)
        {
            if (obj is null || GetType() != obj.GetType())
            {
                return false;
            }
            GameState other = (GameState)obj;
            return Equals(other);
        }

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