using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace roguelice
{
    class Game
    {
        public const string GameTitle = "Roguelice";

        private readonly Graphics render;
        private readonly UI ui;
        private readonly Stopwatch loopTimer;
        private const double fps = 30;
        private const double timeStep = (1 / fps) * 1000;
        private readonly Stack<GameState> gameStates;
        private bool isRunning;

        public Game()
        {
            SetUpConsole();
            render = new Graphics();
            ui = new UI();
            loopTimer = new Stopwatch();
            
            gameStates = new Stack<GameState>();
            PushGameState(new GameStartState(this));

            isRunning = true;
            Run();
        }

        public void PushGameState(GameState state)
        {
            gameStates.Push(state);
        }

        public void CloseState()
        {
            gameStates.Pop();
        }

        private void SetUpConsole()
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

        private void Close()
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
                    gameStates.Peek().Update(ui, input);
                }
                else
                {
                    Close();
                }
            }
            FlushInput();
        }

        private void FlushInput()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        private void Draw()
        {
            if (gameStates.Any())
            {
                gameStates.Peek().Draw(render, ui);
            }
        }

        private void Run()
        {
            loopTimer.Start();
            Draw();
            while (isRunning)
            {
                if (loopTimer.Elapsed.Milliseconds > timeStep)
                {
                    Update();
                    Draw();
                    loopTimer.Restart();
                }
            }
        }
    }
}
