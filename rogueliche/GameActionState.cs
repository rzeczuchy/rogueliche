using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public class GameActionState : GameState
    {
        private readonly Game game;
        private readonly Dungeon dungeon;
        private readonly Player player;
        private readonly List<ISaveable> saveables;
        private readonly GameSaver saver;

        public GameActionState(Game game)
        {
            this.game = game;
            saver = new GameSaver();
            saveables = new List<ISaveable>();
            
            if (saver.CanLoadGame())
            {

            }
            else
            {
                dungeon = new Dungeon();
                ILocation startingLevel = dungeon.NewLevel();
                player = new Player(startingLevel, startingLevel.Entrance);
            }
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            player.Update(input, ui, dungeon);

            if (player.IsDead)
            {
                saver.DeleteSaveFile();
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
