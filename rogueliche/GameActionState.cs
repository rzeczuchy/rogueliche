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
        private Dungeon dungeon;
        private Player player;
        private SaveFileHandler saveHandler;

        public GameActionState(Game game)
        {
            this.game = game;
            saveHandler = new SaveFileHandler();
            dungeon = new Dungeon();

            if (saveHandler.CanLoadGame())
            {
                LoadGame();
            }
            else
            {
                StartNewGame();
            }
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            player.Update(input, ui, dungeon);

            if (player.IsDead)
            {
                DeleteSave();
                EndGame();
            }
            else
            {
                SaveGame();
            }
        }

        public override void Draw(Graphics render, UI ui)
        {
            player.Location.Tilemap.Draw(render, player);
            ui.Draw(render, player);
            render.Draw();
        }

        private void StartNewGame()
        {
            var startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);
        }

        private void SaveGame()
        {
        }

        private void LoadGame()
        {
        }

        private void DeleteSave()
        {
            saveHandler.DeleteSaveFile();
        }

        private void EndGame()
        {
            game.CloseState();
            game.PushGameState(new GameOverState(game, player));
        }
    }
}
