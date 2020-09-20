using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Game
    {
        public const string GameTitle = "Roguelice";

        private readonly Graphics render;
        private readonly UI ui;
        private bool isRunning;
        private Stack<GameState> gameStates;

        public Game()
        {
            SetUpConsole();
            render = new Graphics();
            ui = new UI();
            
            gameStates = new Stack<GameState>();
            gameStates.Push(new GameActionState());

            isRunning = true;
            Run();
        }

        private static void SetUpConsole()
        {
            Console.Title = GameTitle;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(128, 72);
            int windowWidth = Console.LargestWindowWidth > Console.BufferWidth ? Console.BufferWidth : Console.LargestWindowWidth - 1;
            int windowHeight = Console.LargestWindowHeight > Console.BufferHeight ? Console.BufferHeight : Console.LargestWindowHeight - 1;
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Close()
        {
            isRunning = false;
        }

        private void Update()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (gameStates.Any())
                {
                    if (gameStates.Peek().End)
                    {
                        gameStates.Pop();
                    }
                    else
                    {
                        gameStates.Peek().Update(ui, input);
                    }
                }
                else
                {
                    Close();
                }
            }
        }

        private void Draw()
        {
            if (gameStates.Any())
            {
                if (!gameStates.Peek().End)
                {
                    gameStates.Peek().Draw(render, ui);
                }
            }
        }

        private void Run()
        {
            Draw();
            while (isRunning)
            {
                Update();
                Draw();
            }
        }
    }
}
