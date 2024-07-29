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
    public class Game
    {
        // Properties
        public int BoardSize { get; init; }
        public GameBoard Board { get; init; }
        public GameState State { get; set; }
        public GameBoardRenderer BoardRenderer { get; init; }

        // Private fields
        private char?[,] _stateBackup;
        private List<int> _history;
        private int _capturedThisTurn;
        private bool _passEndsGame;
        
        // Game constants
        private readonly string _inputPrompt;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="size">The size of the game board.</param>
        public Game(int size = 19)
        {
            BoardSize = size;
            Board = new GameBoard(size);
            State = new GameState(size);
            BoardRenderer = new GameBoardRenderer();

            _stateBackup = (char?[,])State.State.Clone();
            _history = new List<int>();
            _capturedThisTurn = 0;
            _passEndsGame = false;

            _inputPrompt = $"Enter a combination of a letter (A-{(char)('A' + BoardSize - 1)}) and a number (1-{BoardSize}) or \"pass\": ";

            // Play();
        }

        /// <summary>
        ///     Starts the game.
        /// </summary>
        public void Play()  
        {
            // Render initial empty board
            BoardRenderer.Render(Board, State);

            string input;

            // Game loop
            while (true)
            {
                _stateBackup = (char?[,])State.State.Clone();
                _capturedThisTurn = 0;

                Console.Write(_inputPrompt);
                input = InputParser.ReadInput();
                
                if (input == "EXIT")
                {
                    return;
                }
                
                if (input == "PASS")
                {
                    if (_passEndsGame)
                    {
                        break;                        
                    }
                    else
                    {
                        State.NextColorToPlay();
                        _passEndsGame = true;
                        BoardRenderer.Render(Board, State);
                        continue;
                    }
                }
                else
                {               
                    _passEndsGame = false;
                }

                try
                {
                    (int row, int col) = InputParser.ParseInput(input);

                    row = BoardSize - (row + 1); // For descending row order.
                    
                    if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
                    {
                        throw new ArgumentOutOfRangeException();
                    }

                    if (!GameLogic.IsSpaceFree(State, row, col))
                    {
                        throw new ArgumentException("That spot is already taken. Play somewhere else.");
                    }

                    State.AddStone(row, col);

                    List<List<(int, int)>> adjacentGroups = GameLogic.GetAdjacentGroups(State, row, col);

                    foreach (List<(int, int)> group in adjacentGroups)
                    {
                        if (GameLogic.IsGroupCaptured(State, group))
                        {
                            State.RemoveStones(group);
                            _capturedThisTurn += group.Count;                         
                        }
                    }
                    if (GameLogic.IsGroupCaptured(State, row, col))
                    {
                        throw new ArgumentException("That would be self-capture. Play somewhere else.");          
                    }

                    State.NextColorToPlay();

                    foreach (int hash in _history)
                    {
                        if (State.GetHashCode() == hash)
                        {
                            throw new InvalidOperationException("This would lead to a repeated board position. Play somewhere else.");
                        }
                    }

                    if (State.ColorToPlay == "black")
                    {
                        State.CapturedWhite += _capturedThisTurn;
                    }
                    else
                    {
                        State.CapturedBlack += _capturedThisTurn;                        
                    }   
                    
                    Board.Populate(State);
                    BoardRenderer.Render(Board, State);

                    _history.Add(State.GetHashCode());
                }
                catch (ArgumentOutOfRangeException e)   
                {
                    Console.WriteLine($"{e.Message}");
                    // Revert changes
                    State.UpdateState(_stateBackup);
                    continue;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"{e.Message}");
                    // Revert changes
                    State.UpdateState(_stateBackup);
                    continue;
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine($"{e.Message}");
                    // Revert changes
                    State.UpdateState(_stateBackup);
                    State.NextColorToPlay();
                    continue;
                }
            }

            (int blackScore, int whiteScore) = ScoreCalculator.CalculateScore(State);

            BoardRenderer.Render(Board, State);
            BoardRenderer.RenderScores(blackScore, whiteScore);
        }
    }   
}