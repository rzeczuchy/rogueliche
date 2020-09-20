using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class GameActionState : GameState
    {
        private Dungeon dungeon;
        private Player player;

        public GameActionState()
        {
            dungeon = new Dungeon();
            DungeonLevel startingLevel = dungeon.NewLevel();
            player = new Player(startingLevel, startingLevel.Entrance);
        }

        public override void Close()
        {
            End = true;
        }

        public override void Update(UI ui, ConsoleKey input)
        {
            player.Update(input, ui, dungeon);
        }

        public override void Draw(Graphics render, UI ui)
        {
            player.Location.Tilemap.Draw(render, player);
            ui.Draw(render, player);
            render.Draw();
        }
    }
}
