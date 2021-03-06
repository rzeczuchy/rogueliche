﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace rogueliche
{
    public class Tilemap
    {
        public Tilemap(ILocation location)
        {
            if (location != null)
            {
                Location = location;
            }
            else
            {
                throw new ArgumentNullException();
            }

            ToUpdate = new List<IMappable>();
            Width = Location.Bounds.Width;
            Height = Location.Bounds.Height;
            Bounds = new Rectangle(0, 0, Width, Height);
            Tiles = new Tile[Width, Height];
            FogOfWar = new bool[Width, Height];
            FieldOfVisibility = new bool[Width, Height];
            Creatures = new TilemapLayer(this);
            Items = new TilemapLayer(this);
        }

        public ILocation Location { get; private set; }
        public List<IMappable> ToUpdate { get; }
        public int Width { get; }
        public int Height { get; }
        public Rectangle Bounds { get; }
        public Tile[,] Tiles { get; }
        public bool[,] FogOfWar { get; }
        public bool[,] FieldOfVisibility { get; }
        public TilemapLayer Creatures { get; }
        public TilemapLayer Items { get; }

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
            return new Point(Utilities.RandomNumber(rect.Left, rect.Right - 1), Utilities.RandomNumber(rect.Top, rect.Bottom - 1));
        }

        public Point RandomPosition()
        {
            return RandomPosition(Bounds);
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

        public void PerformOnTiles(Action<Point> action, Rectangle rectangle)
        {
            for (int y = rectangle.Top; y < rectangle.Bottom; y++)
                for (int x = rectangle.Left; x <= rectangle.Right; x++)
                {
                    var pos = new Point(x, y);
                    if (ContainsPosition(pos))
                    {
                        action(pos);
                    }
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
            if (Creatures.GetMappable(pos) is IMappable creature)
            {
                return creature;
            }
            else if (Items.GetMappable(pos) is IMappable item)
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
