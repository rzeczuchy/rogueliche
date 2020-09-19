using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Rectangle
    {
        public Rectangle(Point position, Point size)
        {
            Position = position;
            Size = size;
        }

        public Rectangle(int x, int y, int width, int height)
        {
            Position = new Point(x, y);
            Size = new Point(width, height);
        }

        public Point Position { get; private set; }
        public Point Size { get; private set; }
        public int Width { get => Size.X; }
        public int Height { get => Size.Y; }
        public int Left { get => Position.X; }
        public int Right { get => Position.X + Width; }
        public int Top { get => Position.Y; }
        public int Bottom { get => Position.Y + Height; }
        public Point Center { get => new Point(Position.X + Width / 2, Position.Y + Height / 2); }

        public bool Intersects(Rectangle other)
        {
            if (Position.X > other.Position.X + other.Size.X ||
              other.Position.X > Position.X + Size.X)
            {
                return false;
            }
            if (Position.Y > other.Position.Y + other.Size.Y ||
              other.Position.Y > Position.Y + Size.Y)
            {
                return false;
            }
            return true;
        }
    }
}
