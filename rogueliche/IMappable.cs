using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    public interface IMappable
    {
        ILocation Location { get; set; }
        Point Position { get; set; }
        bool IsDead { get; }
        string Name { get; }
        char Symbol { get; }
        string Overhead { get; }

        void Place(ILocation targetLocation, Point targetPosition);
        void Update(Player player);
    }

}
