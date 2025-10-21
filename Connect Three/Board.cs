namespace Connect_Three
{
    public class Board
    {
        private readonly char[,] grid;
        private const int ROWS = 4;
        private const int COLS = 4;

        public Board()
        {
            grid = new char[ROWS, COLS];

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    grid[i, j] = '.';
                }
            }
        }

        public void Display()
        {
            Console.Clear();

            Console.WriteLine("\n  1 2 3 4");
            Console.WriteLine(" ─────────");

            for (int i = 0; i < ROWS; i++)
            {
                Console.Write("  ");

                for (int j = 0; j < COLS; j++)
                {
                    Console.Write(grid[i, j] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine(" ─────────");
        }

        public bool DropPiece(int column, char player)
        {
            if (column < 0 || column >= COLS)
            {
                return false;
            }

            for (int row = ROWS - 1; row >= 0; row--)
            {
                if (grid[row, column] == '.')
                {
                    grid[row, column] = player;
                    return true;
                }
            }

            return false;
        }

        public bool CheckWinner(char player)
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col <= COLS - 3; col++)
                {
                    if (grid[row, col] == player &&
                        grid[row, col + 1] == player &&
                        grid[row, col + 2] == player)
                    {
                        return true;
                    }
                }
            }

            for (int col = 0; col < COLS; col++)
            {
                for (int row = 0; row <= ROWS - 3; row++)
                {
                    if (grid[row, col] == player &&
                        grid[row + 1, col] == player &&
                        grid[row + 2, col] == player)
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row <= ROWS - 3; row++)
            {
                for (int col = 0; col <= COLS - 3; col++)
                {
                    if (grid[row, col] == player &&
                        grid[row + 1, col + 1] == player &&
                        grid[row + 2, col + 2] == player)
                    {
                        return true;
                    }
                }
            }

            for (int row = 2; row < ROWS; row++)
            {
                for (int col = 0; col <= COLS - 3; col++)
                {
                    if (grid[row, col] == player &&
                        grid[row - 1, col + 1] == player &&
                        grid[row - 2, col + 2] == player)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsFull()
        {
            for (int col = 0; col < COLS; col++)
            {
                if (grid[0, col] == '.')
                {
                    return false;
                }
            }

            return true;
        }

        public List<int> GetValidColumns()
        {
            List<int> validCols = new List<int>();

            for (int col = 0; col < COLS; col++)
            {
                if (grid[0, col] == '.')
                {
                    validCols.Add(col);
                }
            }

            return validCols;
        }

        public char[,] GetGrid()
        {
            return grid;
        }

        public Board Clone()
        {
            Board newBoard = new Board();

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    newBoard.grid[i, j] = this.grid[i, j];
                }
            }

            return newBoard;
        }
    }
}
