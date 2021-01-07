using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    class GameActionState : GameState
    {
        private readonly Game game;
        private Dungeon dungeon;
        private Player player;

        public GameActionState(Game game)
        {
            this.game = game;
            dungeon = new Dungeon();
            ILocation startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            player.Update(input, ui, dungeon);

            if (player.IsDead)
            {
                EndGame();
            }
        }

        public override void Draw(Graphics render, UI ui)
        {
            player.Location.Tilemap.Draw(render, player);
            ui.Draw(render, player);
            render.Draw();
        }

        private void EndGame()
        {
            game.CloseState();
            game.PushGameState(new GameOverState(game, player));
        }
    }
}
