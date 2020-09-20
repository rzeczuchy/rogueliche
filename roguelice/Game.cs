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
        private Dungeon dungeon;
        private bool isRunning;
        private Player player;

        public Game()
        {
            SetUpConsole();
            render = new Graphics();
            ui = new UI();
            isRunning = true;
            Initialize();
            Run();
        }

        private static void SetUpConsole()
        {
            Console.Title = GameTitle;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(160, 90);
            int windowWidth = Console.LargestWindowWidth > Console.BufferWidth ? Console.BufferWidth : Console.LargestWindowWidth - 1;
            int windowHeight = Console.LargestWindowHeight > Console.BufferHeight ? Console.BufferHeight : Console.LargestWindowHeight - 1;
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Close()
        {
            isRunning = false;
        }

        public void DisplayDeathScreen()
        {
            render.DrawStringC("You died.", render.Height / 2);
            render.DrawStringC("You killed " + player.KillCount + " beasts and attained level " + player.Lvl + ".", render.Height / 2 + 1);
            render.DrawStringC("You reached floor " + player.Location.LevelIndex + " of the dungeon.", render.Height / 2 + 2);
            render.DrawStringC("You broke " + player.BrokenWeapons + " weapons.", render.Height / 2 + 3);
            render.DrawStringC("Press a key to restart.", render.Height / 2 + 4);

            render.Draw();
            Console.ReadKey(true);
            Initialize();
        }

        public void DisplayStartScreen()
        {
            render.DrawStringC("Welcome to " + GameTitle + "! Press a key to start new game.", render.Height / 2);
            render.Draw();
            Console.ReadKey(true);
            Draw();
        }

        private void Initialize()
        {
            dungeon = new Dungeon();
            DungeonLevel startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);

            DisplayStartScreen();
        }

        private void Update()
        {
            player.Update(this, ui, dungeon);
        }

        private void Draw()
        {
            player.Location.Tilemap.Draw(render, player);
            ui.Draw(render, player);
            render.Draw();
        }

        private void Run()
        {
            while (isRunning)
            {
                Update();
                Draw();
            }
        }
    }
}
