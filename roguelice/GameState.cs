using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    abstract class GameState
    {
        public bool End { get; protected set; }

        public abstract void Close();

        public abstract void Update(UI ui, ConsoleKey input);

        public abstract void Draw(Graphics render, UI ui);

    }
}
