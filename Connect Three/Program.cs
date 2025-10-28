using Connect_Three;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectThree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== CONNECT THREE ===");
            Console.WriteLine("Цел: Подредете 3 жетона в линия (хоризонтално, вертикално или диагонално)\n");

            Console.WriteLine("Изберете режим на игра:");
            Console.WriteLine("1. Човек срещу Компютър");
            Console.WriteLine("2. Компютър срещу Компютър");
            Console.Write("Избор (1 или 2): ");

            bool humanVsComputer;

            while (true)
            {
                string choice = Console.ReadLine() ?? string.Empty;

                if (choice == "1" || choice == "2")
                {
                    humanVsComputer = choice == "1";
                    break;
                }

                Console.WriteLine("Моля изберете валидна опция!");
            }

            Game game = new(humanVsComputer);
            game.Play();
        }
    }
}