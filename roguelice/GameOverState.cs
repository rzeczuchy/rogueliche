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
            render.DrawStringC("You died.", render.Height / 2);
            render.DrawStringC("You killed " + player.KillCount + " beasts and attained level " + player.Lvl + ".", render.Height / 2 + 1);
            render.DrawStringC("You broke " + player.BrokenWeapons + " weapons.", render.Height / 2 + 3);
            render.DrawStringC("Press ENTER to restart.", render.Height / 2 + 4);

            render.Draw();
        }

        private void RestartGame()
        {
            game.CloseState();
        }
    }
}