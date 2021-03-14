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
        private readonly SaveHandler saveHandler;
        private Dungeon dungeon;
        private Player player;

        public GameActionState(Game game)
        {
            this.game = game ?? throw new ArgumentNullException();
            saveHandler = new SaveHandler();
            dungeon = new Dungeon();

            if (saveHandler.CanLoadGame())
            {
                LoadGame();
            }
            else
            {
                StartGame();
            }
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            player.Update(input, ui, dungeon);

            if (player.IsDead)
            {
                DeleteSavedGame();
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

        private void StartGame()
        {
            var startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);
        }

        private void SaveGame()
        {
            saveHandler.SaveGame(player);
        }

        private void LoadGame()
        {
            dungeon.LevelIndex = saveHandler.LoadGame().LevelIndex;
            var startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);
            player.Load(saveHandler.LoadGame());
        }

        private void DeleteSavedGame()
        {
            saveHandler.DeleteSave();
        }

        private void EndGame()
        {
            game.CloseState();
            game.PushGameState(new GameOverState(game, player));
        }
    }
}
