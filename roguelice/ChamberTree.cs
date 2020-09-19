using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class ChamberTree
    {
        public ChamberTree(Point size, int minChambers, int maxChambers, int minChamberWidth, int maxChamberWidth, int minChamberHeight, int maxChamberHeight,
            bool forceRegularChambers)
        {
            Chambers = new List<Rectangle>();
            Passages = new List<Point>();

            Size = size;
            MinChambers = minChambers;
            MaxChambers = maxChambers;
            MinChamberWidth = minChamberWidth;
            MaxChamberWidth = maxChamberWidth;
            MinChamberHeight = minChamberHeight;
            MaxChamberHeight = maxChamberHeight;
            
            CreateChamberTree(NewStartingChamber());
        }

        public List<Rectangle> Chambers { get; }
        public List<Point> Passages { get; }
        public Rectangle StartingChamber { get; private set; }
        public Point Size { get; }
        public int MinChambers { get; }
        public int MaxChambers { get; }
        public int MinChamberWidth { get; }
        public int MaxChamberWidth { get; }
        public int MinChamberHeight { get; }
        public int MaxChamberHeight { get; }
        public bool ForceRegularChambers { get; }

        public void FillChambersWithTile(Tile.TileType tileType, Tilemap tilemap)
        {
            foreach (Rectangle chamber in Chambers)
            {
                tilemap.FillWithType(chamber, tileType);
            }
        }

        public void FillPassagesWithTile(Tile.TileType tileType, Tilemap tilemap)
        {
            foreach (Point passage in Passages)
            {
                tilemap.SetTile(passage, tileType);
            }
        }

        public Rectangle GetRandomChamber()
        {
            return Chambers[Numbers.RandomNumber(1, Chambers.Count - 1)];
        }

        private void CreateChamberTree(Rectangle startingChamber)
        {
            int chamberNumber = Numbers.RandomNumber(MinChambers, MaxChambers);
            Chambers.Add(startingChamber);

            int attempts = 0;
            while (Chambers.Count < chamberNumber && attempts < 100)
            {
                int n = 0;
                while (n < Chambers.Count)
                {
                    AddRandomNeighborChamber(Chambers[n]);
                    n++;
                }
                attempts++;
            }
        }

        private void AddRandomNeighborChamber(Rectangle chamber)
        {
            Point passage = new Point(0, 0);
            int nborWidth = Numbers.RandomNumber(MinChamberWidth, MaxChamberWidth);
            int nborHeight = Numbers.RandomNumber(MinChamberHeight, MaxChamberHeight);
            Point nborPos = new Point(0, 0);
            
            switch (Numbers.RandomNumber(1, 4))
            {
                case 1:
                    passage = PassageNorth(chamber);
                    nborPos = ChamberNorth(passage, nborWidth, nborHeight);
                    break;
                case 2:
                    passage = PassageSouth(chamber);
                    nborPos = ChamberSouth(passage, nborWidth);
                    break;
                case 3:
                    passage = PassageWest(chamber);
                    nborPos = ChamberWest(passage, nborWidth, nborHeight);
                    break;
                case 4:
                    passage = PassageEast(chamber);
                    nborPos = ChamberEast(passage, nborHeight);
                    break;
                default:
                    break;
            }

            Rectangle neighbor = new Rectangle(nborPos, new Point(nborWidth, nborHeight));

            if (CanFitChamber(GetChamberWithWalls(neighbor)) &&
                (ForceRegularChambers && !ChamberIntersects(GetChamberWithWalls(neighbor)) || !ForceRegularChambers && !ChamberIntersects(neighbor)))
            {
                Chambers.Add(neighbor);
                Passages.Add(passage);
            }
        }

        private bool CanFitChamber(Rectangle rectangle)
        {
            return rectangle.Left >= 0 && rectangle.Top >= 0 && rectangle.Right < Size.X && rectangle.Bottom < Size.Y;
        }

        private static Rectangle GetChamberWithWalls(Rectangle chamber)
        {
            return new Rectangle(chamber.Left - 1, chamber.Top - 1, chamber.Width + 1, chamber.Height + 1);
        }

        private static Point ChamberEast(Point passage, int nborHeight)
        {
            return new Point(passage.X + 1, Numbers.RandomNumber(passage.Y - nborHeight + 1, passage.Y));
        }

        private static Point PassageEast(Rectangle room)
        {
            return new Point(room.Right, Numbers.RandomNumber(room.Top, room.Bottom - 1));
        }

        private static Point ChamberWest(Point passage, int nborWidth, int nborHeight)
        {
            return new Point(passage.X - nborWidth, Numbers.RandomNumber(passage.Y - nborHeight + 1, passage.Y));
        }

        private static Point PassageWest(Rectangle room)
        {
            return new Point(room.Left - 1, Numbers.RandomNumber(room.Top, room.Bottom - 1));
        }

        private static Point ChamberSouth(Point passage, int nborWidth)
        {
            return new Point(Numbers.RandomNumber(passage.X - nborWidth + 1, passage.X), passage.Y + 1);
        }

        private static Point PassageSouth(Rectangle room)
        {
            return new Point(Numbers.RandomNumber(room.Left, room.Right - 1), room.Bottom);
        }

        private static Point ChamberNorth(Point passage, int nborWidth, int nborHeight)
        {
            return new Point(Numbers.RandomNumber(passage.X - nborWidth + 1, passage.X), passage.Y - nborHeight);
        }

        private static Point PassageNorth(Rectangle room)
        {
            return new Point(Numbers.RandomNumber(room.Left, room.Right - 1), room.Top - 1);
        }

        private Rectangle NewStartingChamber()
        {
            int width = Numbers.RandomNumber(MinChamberWidth, MaxChamberWidth);
            int height = Numbers.RandomNumber(MinChamberHeight, MaxChamberHeight);
            StartingChamber = new Rectangle(Size.X / 2 - height / 2, Size.Y / 2 - width / 2, width, height);
            return StartingChamber;
        }

        private bool ChamberIntersects(Rectangle chamber)
        {
            return Chambers.Where(i => GetChamberWithWalls(i).Intersects(chamber)).Any();
        }

    }
}
