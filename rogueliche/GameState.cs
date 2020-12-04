using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    abstract class GameState
    {
        public abstract void Update(UI ui, ConsoleKey input);

        public abstract void Draw(Graphics render, UI ui);
    }
}
