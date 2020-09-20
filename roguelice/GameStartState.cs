using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class GameStartState : GameState
    {
        private readonly Game game;

        public GameStartState(Game game)
        {
            this.game = game;
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
            render.DrawStringC("Welcome to " + Game.GameTitle + "! Press ENTER to start.", render.Height / 2);
            render.Draw();
        }

        private void StartNewGame()
        {
            game.PushGameState(new GameActionState(game));
        }
    }
}