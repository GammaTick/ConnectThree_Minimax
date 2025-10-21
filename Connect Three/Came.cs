using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_Three
{
    public class Game
    {
        private readonly Board board;
        private readonly bool humanVsComputer;
        private const int MAX_DEPTH = 4;

        public Game(bool humanVsComputer)
        {
            board = new Board();
            this.humanVsComputer = humanVsComputer;
        }

        public void Play()
        {
            bool gameOver = false;
            char currentPlayer = 'X';

            while (!gameOver)
            {
                if (currentPlayer == 'X')
                { 
                    board.Display();
                }

                Console.WriteLine($"\nРед на играч {currentPlayer}");

                int column;

                if (humanVsComputer && currentPlayer == 'X')
                {
                    column = GetHumanMove();
                }
                else
                {
                    column = ComputerAI.GetComputerMove(currentPlayer, board, MAX_DEPTH);
                }

                if (!board.DropPiece(column, currentPlayer))
                {
                    Console.WriteLine("Невалиден ход! Опитайте отново.");
                    continue;
                }

                if (board.CheckWinner(currentPlayer))
                {
                    board.Display();
                    Console.WriteLine($"\nИграч {currentPlayer} печели!");
                    gameOver = true;
                }
                else if (board.IsFull())
                {
                    board.Display();
                    Console.WriteLine("\nРавенство! Дъската е пълна.");
                    gameOver = true;
                }
                else
                {
                    currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                }
            }

            Console.WriteLine("\nИграта приключи!");
        }

        private static int GetHumanMove()
        {
            while (true)
            {
                Console.Write("Изберете колона (1-4): ");

                if (int.TryParse(Console.ReadLine(), out int col) && col >= 1 && col <= 4)
                {
                    return col - 1;
                }

                Console.WriteLine("Невалидна колона! Въведете число от 1 до 4.");
            }
        }
    }
}