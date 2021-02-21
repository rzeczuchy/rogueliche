using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rogueliche
{
    static class AI
    {
        public static double DistanceBetween(IMappable moveable, IMappable target)
        {
            return (moveable.Position != null && target.Position != null) ?
                Point.Distance(moveable.Position, target.Position) : 0;
        }

        public static bool HasLowHealth(IFightable fighter)
        {
            return fighter.Health < fighter.MaxHealth / 4;
        }

        public static bool AreNeighboring(IMappable moveable, IMappable target)
        {
            return DistanceBetween(moveable, target) < 2;
        }

        public static void MoveAtRandom(IMoveable moveable)
        {
            if (moveable.Position != null)
            {
                Point pos = moveable.Position;
                moveable.Move(new Point(Utilities.RandomNumber(pos.X - 1, pos.X + 1), Utilities.RandomNumber(pos.Y - 1, pos.Y + 1)));
            }
        }

        public static void MoveTowards(IMoveable moveable, Point position)
        {
            if (moveable.Position != null && OpenNeighbors(moveable).Any())
            {
                double minCost = OpenNeighbors(moveable).Min(p => MovementCost(moveable.Position, p,
                    position));
                moveable.Move(OpenNeighbors(moveable).FirstOrDefault(p => MovementCost(moveable.Position,
                    p, position) == minCost));
            }
        }

        public static void MoveTowards(IMoveable moveable, IMappable mapObject)
        {
            MoveTowards(moveable, mapObject.Position);
        }

        private static double GetMinCost(IMoveable fighter, IMappable mapObject)
        {
            return OpenNeighbors(fighter).Min(p => MovementCost(fighter.Position, p,
                                mapObject.Position));
        }

        public static void MoveAwayFrom(IMoveable moveable, IMappable mapObject)
        {
            if (moveable.Position != null && mapObject.Position != null && OpenNeighbors(moveable).Any())
            {
                moveable.Move(OpenNeighbors(moveable).FirstOrDefault(p => MovementCost(moveable.Position,
                    p, mapObject.Position) == GetMaxCost(moveable, mapObject)));
            }
        }

        private static double GetMaxCost(IMoveable moveable, IMappable mapObject)
        {
            return OpenNeighbors(moveable).Max(p => MovementCost(moveable.Position, p,
                                mapObject.Position));
        }

        public static List<Point> OpenNeighbors(IMoveable moveable)
        {
            return moveable.Position != null ? CountOpenNeighbors(moveable) : new List<Point>();
        }

        private static List<Point> CountOpenNeighbors(IMoveable moveable)
        {
            var resulting = new List<Point>();
            Point position = moveable.Position;
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    Point pos = new Point(position.X + x, position.Y + y);
                    if (pos != position && IsPositionOpen(moveable, position, pos))
                    {
                        resulting.Add(pos);
                    }
                }
            return resulting;
        }

        private static bool IsPositionOpen(IMoveable moveable, Point position, Point pos)
        {
            return moveable.Location.Tilemap.ContainsPosition(pos) &&
                moveable.Location.Tilemap.IsWalkable(pos) &&
                moveable.Location.Tilemap.Creatures.GetMappable(pos) == null;
        }

        public static double MovementCost(Point start, Point tested, Point target)
        {
            double g = Point.Distance(start, tested);
            double h = Point.Distance(target, tested);
            double f = g + h;
            return f;
        }

        public static bool CanMove(IMoveable moveable)
        {
            return OpenNeighbors(moveable).Any();
        }
    }
}
