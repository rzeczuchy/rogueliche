using System;
using System.Collections.Generic;
using System.Linq;

namespace roguelice
{
    public class Tilemap
    {
        public Tilemap(ILocation location, int width, int height)
        {
            Location = location;
            ToUpdate = new List<IMappable>();
            Width = width;
            Height = height;
            Tiles = new Tile[Width, Height];
            FogOfWar = new bool[Width, Height];
            FieldOfVisibility = new bool[Width, Height];
            Creatures = new TilemapLayer(this);
            Items = new TilemapLayer(this);
        }

        public ILocation Location { get; private set; }
        public List<IMappable> ToUpdate { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public bool[,] FogOfWar { get; private set; }
        public bool[,] FieldOfVisibility { get; private set; }
        public TilemapLayer Creatures { get; private set; }
        public TilemapLayer Items { get; private set; }

        public Tile GetTile(Point position)
        {
            if (ContainsPosition(position))
            {
                return Tiles[position.X, position.Y];
            }
            else
            {
                return null;
            }
        }

        public void SetTile(Point position, Tile.TileType type)
        {
            if (ContainsPosition(position))
            {
                Tiles[position.X, position.Y] = new Tile(type);
            }
        }

        public void FillWithTile(int left, int top, int right, int bottom, Tile.TileType type)
        {
            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    if (ContainsPosition(new Point(x, y)))
                    {
                        Tiles[x, y] = new Tile(type);
                    }
                }
        }

        public void FillWithTile(Rectangle rectangle, Tile.TileType type)
        {
            FillWithTile(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, type);
        }

        public bool ContainsPosition(Point pos)
        {
            return pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
        }

        public Point RandomPosition(Rectangle rect)
        {
            return new Point(Numbers.RandomNumber(rect.Left, rect.Right), Numbers.RandomNumber(rect.Top, rect.Bottom));
        }

        public bool IsWalkable(Point position)
        {
            return GetTile(position) != null && (GetTile(position).Type == Tile.TileType.floor || GetTile(position).Type == Tile.TileType.exit);
        }

        public void Update(Player player)
        {
            UpdateObjects(player);
            UpdateFogOfWar(player);
            UpdateFieldOfVisibility(player);
        }

        public void UpdateObjects(Player player)
        {
            ToUpdate.Clear();

            Creatures.FilterDead();
            Items.FilterDead();

            ToUpdate.Where(o => !o.IsDead).ToList().ForEach(o => o.Update(player));
        }

        public void UpdateFieldOfVisibility(Player player)
        {
            PerformOnAllTiles((pos) => SetVisibilityForPosition(player, pos));
        }

        public void UpdateFogOfWar(Player player)
        {
            PerformOnAllTiles((pos) => SetFogForPosition(player, pos));
        }

        public bool IsUnfogged(Point pos)
        {
            return ContainsPosition(pos) && FogOfWar[pos.X, pos.Y] == true;
        }

        public bool IsVisible(Point pos)
        {
            return ContainsPosition(pos) && FieldOfVisibility[pos.X, pos.Y] == true;
        }

        public void PerformOnAllTiles(Action<Point> action)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    var pos = new Point(x, y);
                    action(pos);
                }
        }

        public void PerformOnVisibleTiles(Action<Point> action, Graphics render, Player player)
        {
            Point cameraPosition = new Point(player.Position.X - render.Width / 2, player.Position.Y - render.Height / 2);

            for (int y = cameraPosition.Y; y < cameraPosition.Y + render.Height; y++)
                for (int x = cameraPosition.X; x < cameraPosition.X + render.Width; x++)
                {
                    var pos = new Point(x, y);
                    if (IsUnfogged(pos))
                    {
                        action(pos);
                    }
                }
        }

        public IMappable TopMappable(Point pos)
        {
            if (Creatures.Get(pos) is IMappable creature)
            {
                return creature;
            }
            else if (Items.Get(pos) is IMappable item)
            {
                return item;
            }
            else return null;
        }

        public void Draw(Graphics render, Player player)
        {
            PerformOnVisibleTiles((point) => DrawPosition(point, render, player), render, player);
        }

        private void DrawPosition(Point pos, Graphics render, Player player)
        {
            var transformedPos = TransformedPosition(pos, render, player);

            if (TopMappable(pos) is IMappable mappable && IsVisible(pos))
            {
                render.DrawChar(mappable.Symbol, transformedPos);
            }
            else if (GetTile(pos) is Tile tile)
            {
                DrawTile(render, player, pos);
            }
        }

        private void DrawTile(Graphics render, Player player, Point pos)
        {
            if (GetTile(pos) is Tile tile && IsAccessible(pos, player))
            {
                char symbol = IsVisible(pos) ? tile.Symbol : tile.NotVisibleSymbol;
                var transformedPos = TransformedPosition(pos, render, player);
                render.DrawChar(symbol, transformedPos);
            }
        }

        private Point TransformedPosition(Point pos, Graphics render, Player player)
        {
            return new Point(pos.X - Graphics.CameraTransform(player, render).X,
                pos.Y - Graphics.CameraTransform(player, render).Y);
        }

        private void SetVisibilityForPosition(Player player, Point pos)
        {
            FieldOfVisibility[pos.X, pos.Y] = Point.Distance(pos, player.Position) <= 10;
        }

        private void SetFogForPosition(Player player, Point pos)
        {
            if (Point.Distance(pos, player.Position) <= 10)
            {
                FogOfWar[pos.X, pos.Y] = true;
            }
        }

        private bool IsAccessible(Point pos, Player player)
        {
            foreach (Tile t in NeighbouringTiles(pos))
            {
                if (t.Type == Tile.TileType.floor || t.Type == Tile.TileType.exit)
                    return true;
            }
            return false;
        }

        private List<Tile> NeighbouringTiles(Point pos)
        {
            List<Tile> neighbours = new List<Tile>();
            for (int w = pos.X - 1; w <= pos.X + 1; w++)
                for (int h = pos.Y - 1; h <= pos.Y + 1; h++)
                {
                    var neighbour = new Point(w, h);
                    if (ContainsPosition(neighbour))
                        neighbours.Add(Tiles[w, h]);
                }

            return neighbours;
        }
    }
}
