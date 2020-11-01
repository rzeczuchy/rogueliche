using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public interface IMappable
    {
        ILocation Location { get; set; }
        TilemapLayer Layer { get; set; }
        Point Position { get; set; }
        bool IsDead { get; }
        string Name { get; }
        char Symbol { get; }
        string Overhead { get; }

        void ChangeLocation(ILocation targetLocation, Point targetPosition);
        void ChangePosition(Point targetPosition);
        void Update(Player player);
    }

}
