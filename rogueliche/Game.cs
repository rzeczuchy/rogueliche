using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace rogueliche
{
    public class Game
    {
        public const string GameTitle = "Rogueliche";
        public const string Version = "1.1.3";
        public const string Copyright = "coded by rzeczuchy 2019-2021 ver";
        private const double fps = 30;
        private readonly double timeStep;
        private readonly Graphics render;
        private readonly UI ui;
        private readonly Stopwatch loopTimer;
        private readonly Stack<GameState> gameStates;
        private bool isRunning;

        public Game()
        {
            SetWindowTitle(GameTitle);

            timeStep = (1 / fps) * 1000;
            render = new Graphics();
            ui = new UI(render);
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

        private void SetWindowTitle(string title)
        {
            Console.Title = title;
        }

        private void Close()
        {
            isRunning = false;
        }

        private void Update()
        {
            ui.Update();

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
