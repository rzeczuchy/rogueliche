using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public static double Distance(Point p1, Point p2)
        {
            double xDist = Math.Pow(p1.X - p2.X, 2);
            double yDist = Math.Pow(p1.Y - p2.Y, 2);
            double distance = Math.Sqrt(xDist + yDist);
            return distance;
        }
    }
}
