using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Three
{
    internal static class ComputerAI
    {
        public static int GetComputerMove(char player, Board board, int MAX_DEPTH)
        {
            MinimaxResult result = Minimax(board, MAX_DEPTH, true, player, int.MinValue, int.MaxValue);
            return result.Column;
        }

        private static MinimaxResult Minimax(Board board, int depth, bool maximizing, char player, int alpha, int beta)
        {
            char opponent = (player == 'X') ? 'O' : 'X';
            char currentPlayer = maximizing ? player : opponent;

            if (board.CheckWinner(player))
            {
                return new MinimaxResult(1000, -1);
            }

            if (board.CheckWinner(opponent))
            {
                return new MinimaxResult(-1000, -1);
            }

            if (board.IsFull() || depth == 0)
            {
                return new MinimaxResult(EvaluateBoard(board, player), -1);
            }

            List<int> validMoves = board.GetValidColumns();
            int bestColumn = validMoves[0];

            if (maximizing)
            {
                int maxScore = int.MinValue;

                foreach (int col in validMoves)
                {
                    Board newBoard = board.Clone();
                    newBoard.DropPiece(col, currentPlayer);

                    MinimaxResult result = Minimax(newBoard, depth - 1, false, player, alpha, beta);

                    if (result.Score > maxScore)
                    {
                        maxScore = result.Score;
                        bestColumn = col;
                    }

                    alpha = Math.Max(alpha, maxScore);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return new MinimaxResult(maxScore, bestColumn);
            }
            else
            {
                int minScore = int.MaxValue;

                foreach (int col in validMoves)
                {
                    Board newBoard = board.Clone();
                    newBoard.DropPiece(col, currentPlayer);

                    MinimaxResult result = Minimax(newBoard, depth - 1, true, player, alpha, beta);

                    if (result.Score < minScore)
                    {
                        minScore = result.Score;
                        bestColumn = col;
                    }

                    beta = Math.Min(beta, minScore);

                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                return new MinimaxResult(minScore, bestColumn);
            }
        }

        private static int EvaluateBoard(Board board, char player)
        {
            char opponent = (player == 'X') ? 'O' : 'X';
            int score = 0;

            score += EvaluateLines(board, player, opponent);

            score += EvaluateCenterControl(board, player, opponent);

            return score;
        }

        private static int EvaluateLines(Board board, char player, char opponent)
        {
            int score = 0;
            char[,] grid = board.GetGrid();

            List<List<(int, int)>> lines = GetAllLines();

            foreach (var line in lines)
            {
                int playerCount = 0;
                int opponentCount = 0;

                foreach (var pos in line)
                {
                    char cell = grid[pos.Item1, pos.Item2];

                    if (cell == player)
                    {
                        playerCount++;
                    }
                    else if (cell == opponent)
                    {
                        opponentCount++;
                    }
                }

                if (playerCount > 0 && opponentCount == 0)
                {
                    if (playerCount == 3)
                    {
                        score += 100;
                    }
                    else if (playerCount == 2)
                    {
                        score += 10;
                    }
                    else if (playerCount == 1)
                    {
                        score += 1;
                    }
                }
                else if (opponentCount > 0 && playerCount == 0)
                {
                    if (opponentCount == 3)
                    {
                        score -= 100;
                    }
                    else if (opponentCount == 2)
                    {
                        score -= 10;
                    }
                    else if (opponentCount == 1)
                    {
                        score -= 1;
                    }
                }
            }

            return score;
        }

        private static int EvaluateCenterControl(Board board, char player, char opponent)
        {
            char[,] grid = board.GetGrid();
            int score = 0;

            int[] columnWeights = [1, 2, 2, 1];

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row < 4; row++)
                {
                    if (grid[row, col] == player)
                    {
                        score += columnWeights[col];
                    }
                    else if (grid[row, col] == opponent)
                    {
                        score -= columnWeights[col];
                    }
                }
            }

            return score;
        }

        private static List<List<(int, int)>> GetAllLines()
        {
            var lines = new List<List<(int, int)>>();

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col <= 1; col++)
                {
                    lines.Add(
                    [
                        (row, col),
                        (row, col + 1),
                        (row, col + 2)
                    ]);
                }
            }

            for (int col = 0; col < 4; col++)
            {
                for (int row = 0; row <= 1; row++)
                {
                    lines.Add(
                    [
                        (row, col),
                        (row + 1, col),
                        (row + 2, col)
                    ]);
                }
            }

            for (int row = 2; row < 4; row++)
            {
                for (int col = 0; col <= 1; col++)
                {
                    lines.Add(
                    [
                        (row, col),
                        (row - 1, col + 1),
                        (row - 2, col + 2)
                    ]);
                }
            }

            for (int row = 0; row <= 1; row++)
            {
                for (int col = 0; col <= 1; col++)
                {
                    lines.Add(
                    [
                        (row, col),
                        (row + 1, col + 1),
                        (row + 2, col + 2)
                    ]);
                }
            }

            return lines;
        }
    }
}
