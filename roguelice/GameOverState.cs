using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class GameOverState : GameState
    {
        private readonly Game game;
        private readonly Player player;

        public GameOverState(Game game, Player player)
        {
            this.game = game;
            this.player = player;
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.Enter:
                    RestartGame();
                    break;
            }
        }

        public override void Draw(Graphics render, UI ui)
        {
            render.DrawCenteredString("You died.", render.Height / 2);
            render.DrawCenteredString("You killed " + player.KillCount + " beasts and attained level " + player.Lvl + ".", render.Height / 2 + 1);
            render.DrawCenteredString("You reached " + player.Location.Name + ".", render.Height / 2 + 2);
            render.DrawCenteredString("You broke " + player.BrokenWeapons + " weapons.", render.Height / 2 + 3);
            render.DrawCenteredString("Press ENTER to restart.", render.Height / 2 + 4);

            render.Draw();
        }

        private void RestartGame()
        {
            game.CloseState();
        }
    }
}