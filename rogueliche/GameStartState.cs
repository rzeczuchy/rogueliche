using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    class GameStartState : GameState
    {
        private readonly Game game;

        public GameStartState(Game game)
        {
            this.game = game ?? throw new ArgumentNullException();
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.Enter:
                    StartNewGame();
                    break;
            }
        }

        public override void Draw(Graphics render, UI ui)
        {
            render.DrawCenteredString("Welcome to " + Game.GameTitle + "!", render.Height / 2 - 2);
            render.DrawCenteredString("Press the ENTER key to start.", render.Height / 2 + 2);
            render.DrawCenteredString(Game.Copyright + " " + Game.Version, render.Height - 2);
            render.Draw();
        }

        private void StartNewGame()
        {
            game.PushGameState(new GameActionState(game));
        }
    }
}