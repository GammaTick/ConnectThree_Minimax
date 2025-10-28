using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect_Three
{
    internal static class ComputerAI
    {
        private const int WIN_SCORE = 100_000;
        private const int LOSS_SCORE = -100_000;
        private const char EMPTY = '.';

        public static int GetComputerMove(char player, Board board, int maxDepth)
        {
            List<int> validColumns = board.GetValidColumns();

            if (validColumns.Count == 0)
            {
                return -1;
            }

            if (validColumns.Count == 1)
            {
                return validColumns[0];
            }

            int emptyCells = GetEmptyCellsCount(board);
            int searchDepth = Math.Min(maxDepth, emptyCells);

            char opponent = (player == 'X') ? 'O' : 'X';

            foreach (int column in validColumns)
            {
                Board childBoard = board.Clone();
                childBoard.DropPiece(column, player);

                if (childBoard.CheckWinner(player))
                {
                    return column;
                }
            }

            foreach (int column in validColumns)
            {
                Board childBoard = board.Clone();
                childBoard.DropPiece(column, opponent);

                if (childBoard.CheckWinner(opponent))
                {
                    return column;
                }
            }

            MinimaxResult bestResult = Minimax(board, searchDepth, true, player, int.MinValue, int.MaxValue);
            return bestResult.Column;
        }

        private static MinimaxResult Minimax(Board board, int depth, bool maximizing, char me, int bestScoreAI, int bestScoreOpponent)
        {
            char opponent = (me == 'X') ? 'O' : 'X';

            if (board.CheckWinner(me))
            {
                return new MinimaxResult(WIN_SCORE + depth, -1);
            }

            if (board.CheckWinner(opponent))
            {
                return new MinimaxResult(LOSS_SCORE - depth, -1);
            }

            if (board.IsFull() || depth == 0)
            {
                return new MinimaxResult(EvaluateBoard(board, me), -1);
            }

            List<int> orderedMoves = [.. OrderByCenter(board.GetValidColumns(), board.GetGrid().GetLength(1))];
            int bestColumn = orderedMoves[0];

            if (maximizing)
            {
                int bestScore = int.MinValue;

                foreach (int column in orderedMoves)
                {
                    Board childBoard = board.Clone();
                    childBoard.DropPiece(column, me);

                    MinimaxResult result = Minimax(childBoard, depth - 1, false, me, bestScoreAI, bestScoreOpponent);

                    if (result.Score > bestScore)
                    {
                        bestScore = result.Score;
                        bestColumn = column;
                    }

                    bestScoreAI = Math.Max(bestScoreAI, bestScore);

                    if (bestScoreAI >= bestScoreOpponent)
                    {
                        break;
                    }
                }

                return new MinimaxResult(bestScore, bestColumn);
            }
            else
            {
                int bestScore = int.MaxValue;

                foreach (int column in orderedMoves)
                {
                    Board childBoard = board.Clone();
                    childBoard.DropPiece(column, opponent);

                    MinimaxResult result = Minimax(childBoard, depth - 1, true, me, bestScoreAI, bestScoreOpponent);

                    if (result.Score < bestScore)
                    {
                        bestScore = result.Score;
                        bestColumn = column;
                    }

                    bestScoreOpponent = Math.Min(bestScoreOpponent, bestScore);

                    if (bestScoreAI >= bestScoreOpponent)
                    {
                        break;
                    }
                }

                return new MinimaxResult(bestScore, bestColumn);
            }
        }

        private static int EvaluateBoard(Board board, char me)
        {
            char[,] grid = board.GetGrid();
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            char opponent = (me == 'X') ? 'O' : 'X';

            int score = 0;

            // Mild center preference
            double center = (columns - 1) / 2.0; // for 4x4 = 1.5

            // Goes through each cell, if the cell has my symbol add 1 to my score,
            // otherwise subtract 1 
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < columns; currentColumn++)
                {
                    // First I get distance from center, example:
                    // If current column is 0 then the distance is 1.5 and the bonus is 0.5
                    // if current column is 1 then the distance is 0.5 and the bonus is 1.5 
                    // This is because it is better to choose moves in the center of the board in connect three
                    int bonus = (int)(2 - Math.Abs(currentColumn - center));

                    if (grid[currentRow, currentColumn] == me)
                    {
                        score += bonus;
                    }

                    if (grid[currentRow, currentColumn] == opponent)
                    {
                        score -= bonus;
                    }
                }
            }

            // In here I get the score by checking 3 consecutive symbols horizonally.
            // Example:
            // O X X O O
            // From here I will check: O X X then X X O and so on
            // the score depends on the conditions in the EvaluateWindow method
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn <= columns - 3; currentColumn++)
                {
                    score += EvaluateWindow(
                        grid[currentRow, currentColumn],
                        grid[currentRow, currentColumn + 1],
                        grid[currentRow, currentColumn + 2],
                        me,
                        opponent
                    );
                }
            }

            // In here I get the score by checking 3 consecutive symbols vertically.
            // Same logic as horizontal BUT I check vertically, thats it
            for (int currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                for (int currentRow = 0; currentRow <= rows - 3; currentRow++)
                {
                    score += EvaluateWindow(
                        grid[currentRow, currentColumn],
                        grid[currentRow + 1, currentColumn],
                        grid[currentRow + 2, currentColumn],
                        me,
                        opponent
                    );
                }
            }

            // In here I get the score by checking 3 consecutive symbols diagonally
            // from top-left to bottom-right
            for (int currentRow = 0; currentRow <= rows - 3; currentRow++)
            {
                for (int currentColumn = 0; currentColumn <= columns - 3; currentColumn++)
                {
                    score += EvaluateWindow(
                        grid[currentRow, currentColumn],
                        grid[currentRow + 1, currentColumn + 1],
                        grid[currentRow + 2, currentColumn + 2],
                        me,
                        opponent
                    );
                }
            }

            // In here I get the score by checking 3 consecutive symbols diagonally
            // from top-left to bottom-right
            for (int currentRow = 2; currentRow < rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn <= columns - 3; currentColumn++)
                {
                    score += EvaluateWindow(
                        grid[currentRow, currentColumn],
                        grid[currentRow - 1, currentColumn + 1],
                        grid[currentRow - 2, currentColumn + 2],
                        me,
                        opponent
                    );
                }
            }

            return score;
        }

        private static int EvaluateWindow(char a, char b, char c, char me, char opponent)
        {
            int countMe = 0;
            int countOpponent = 0;
            int countEmpty = 0;

            void Count(char symbol)
            {
                if (symbol == me)
                {
                    countMe++;
                }
                else if (symbol == opponent)
                {
                    countOpponent++;
                }
                else
                {
                    countEmpty++;
                }
            }

            Count(a);
            Count(b);
            Count(c);

            // If all symbols are equal and are mine I won
            if (countMe == 3)
            {
                return 1000;
            }

            // If all symbols are equal and are NOT mine I lost
            if (countOpponent == 3)
            {
                return -1000;
            }

            // If there are 2 consecutive symbols and 1 empty cell then
            // I am at an advantage because I might be able to win
            // that's why I give myself good score because this a good board
            // for me and in my favour
            if (countMe == 2 && countEmpty == 1)
            {
                return 12;
            }

            // In here It is the oposite. It is bad if the opponent has 2 
            // consecutive symbols with empty cell because they might win
            // negative score for me because this is a bad board for me
            if (countOpponent == 2 && countEmpty == 1)
            {
                return -12;
            }

            // If there is 1 symbol with 2 consecutive emtpty cells then
            // there is a small chance I might be able to use these cells
            // to win so giving myself a small positive just to 
            // "keep them in mind" that they are an option
            if (countMe == 1 && countEmpty == 2)
            {
                return 2;
            }

            // If the opponent has 1 symbol and 2 consecutive emtpty cells
            // then they might use these cells to win, it is not my number 1
            // priority now but I still give myself a negative score because
            // I should still "keep them in mind" and be alerted
            if (countOpponent == 1 && countEmpty == 2)
            {
                return -2;
            }

            return 0;
        }

        private static IEnumerable<int> OrderByCenter(List<int> columns, int totalColumns)
        {
            double center = (totalColumns - 1) / 2.0;

            return columns
                .OrderBy(column => Math.Abs(column - center))
                .ThenBy(column => column);
        }

        private static int GetEmptyCellsCount(Board board)
        {
            char[,] grid = board.GetGrid();
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            int emptyCellsCount = 0;

            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                for (int currentColumn = 0; currentColumn < columns; currentColumn++)
                {
                    if (grid[currentRow, currentColumn] == EMPTY)
                    {
                        emptyCellsCount++;
                    }
                }
            }

            return emptyCellsCount;
        }
    }
}
