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
    ///     Provides methods to render the game board and the heads-up display to the console.
    /// </summary>
    public class GameBoardRenderer
    {
        private readonly ConsoleColor _originalBGColor = Console.BackgroundColor;
        private readonly ConsoleColor _originalFGColor = Console.ForegroundColor;

        private readonly ConsoleColor _backgroundColor = ConsoleColor.Yellow;
        private readonly ConsoleColor _gridLineColor = ConsoleColor.DarkGray;
        private readonly ConsoleColor _blackStoneColor = ConsoleColor.Black;
        private readonly ConsoleColor _whiteStoneColor = ConsoleColor.White;

        /// <summary>
        ///     Renders the heads-up display and the game board.
        /// </summary>
        /// <param name="gameBoard">The game board to render.</param>
        /// <param name="gameState">The current game state.</param>
        public void Render(GameBoard gameBoard, GameState gameState)
        {
            Console.Clear();
            RenderHeadsUpDisplay(gameBoard.Size, gameState.ColorToPlay, gameState.CapturedBlack, gameState.CapturedWhite);
            RenderGameBoard(gameBoard);
        }

        /// <summary>
        ///     Renders the game board.
        /// </summary>
        /// <param name="gameBoard">The game board to render.</param>
        public void RenderGameBoard(GameBoard gameBoard)
        {
            Console.Write("   ");
            for (char letter = 'A'; letter <= 'A' + gameBoard.Size - 1; letter++)
            {
                Console.Write(letter);
            }
            Console.WriteLine();

            for (int i = 0; i < gameBoard.Size; i++)
            {
                Console.Write(string.Format("{0, 3  }", gameBoard.Size - i + " "));

                for (int j = 0; j < gameBoard.Size; j++)
                {
                    Console.BackgroundColor = _backgroundColor;
                    Console.ForegroundColor = _gridLineColor;
                    
                    if (gameBoard.PopulatedBoard[i, j] == "w")
                    {
                        Console.ForegroundColor = _whiteStoneColor;
                        Console.Write("●");
                    }
                    else if (gameBoard.PopulatedBoard[i, j] == "b")
                    {
                        Console.ForegroundColor = _blackStoneColor;
                        Console.Write("●");
                    }
                    else
                    {
                        Console.Write(gameBoard.PopulatedBoard[i, j]);
                    }
                    ResetToOriginalColors();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        ///     Renders the heads-up display.
        /// </summary>
        /// <param name="boardSize">The size of the board.</param>
        /// <param name="nextColorToPlay">The active player's color.</param>
        /// <param name="capturedWhite">The number of captured white stones.</param>
        /// <param name="capturedBlack">THe number of captured black stones.</param>
        public void RenderHeadsUpDisplay(int boardSize, string nextColorToPlay, int capturedWhite, int capturedBlack)
        {
            // Game name and board size:
            string gameType = $"Go {boardSize}×{boardSize} (Komi: {ScoreCalculator.GetKomi()})";
            Console.Write(string.Format("{0, -20}", gameType));

            // Captured black stones:
            Console.Write("Captured:");
            Console.WriteLine();

            Console.Write(string.Format("{0, 21}", " "));
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _blackStoneColor;
            Console.Write(" ● ");
            ResetToOriginalColors();
            Console.Write(string.Format(": {0, 2} ", capturedBlack));
            Console.WriteLine();

            // Color to play:
            Console.Write($"{(nextColorToPlay == "black" ? "Black " : "White ")}");
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = nextColorToPlay == "black" ? _blackStoneColor : _whiteStoneColor;
            Console.Write(" ● ");
            ResetToOriginalColors();
            Console.Write(" to play.");

            // Captured white stones:
            Console.Write(string.Format("{0, 3}", " "));
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _whiteStoneColor;
            Console.Write(" ● ");
            ResetToOriginalColors();
            Console.Write(string.Format(": {0, 2} ", capturedWhite));
            Console.WriteLine(Environment.NewLine);
        }

        /// <summary>
        ///     Renders the game over screen and displays the winner.
        /// </summary>
        /// <param name="blackScore">The black player's score.</param>
        /// <param name="whiteScore">The white player's score.</param>
        public void RenderScores(int blackScore, int whiteScore)
        {
            Console.WriteLine("GAME OVER!".PadLeft(20) + Environment.NewLine);
            
            Console.Write(string.Format("{0, 8}", " "));
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _blackStoneColor;
            Console.Write(" ● ");
            ResetToOriginalColors();
            Console.WriteLine($" Black: {blackScore, 3}");

            Console.Write(string.Format("{0, 8}", " "));
            Console.BackgroundColor = _backgroundColor;
            Console.ForegroundColor = _whiteStoneColor;
            Console.Write(" ● ");
            ResetToOriginalColors();
            Console.WriteLine($" White: {whiteScore, 3}");
            Console.WriteLine();

            if (blackScore == whiteScore)
            {
                Console.WriteLine("  Draws are awarded to white.");
            }
            if (blackScore > whiteScore)
            {
                Console.WriteLine(" Black wins. Congratulations!");
            }
            else
            {
                Console.WriteLine(" White wins. Congratulations!");
            }
            
            Console.WriteLine();
        }

        /// <summary>
        ///     Resets the console colors to their original values.
        /// </summary>
        public void ResetToOriginalColors()
        {
            Console.BackgroundColor = _originalBGColor;
            Console.ForegroundColor = _originalFGColor;
        }
    }
}           