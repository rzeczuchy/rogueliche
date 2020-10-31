﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    public class Tilemap
    {
        private readonly List<IMappable> toUpdate;

        public Tilemap(ILocation location, int width, int height)
        {
            Location = location;

            toUpdate = new List<IMappable>();

            SetLevelSize(width, height);
        }

        private void SetLevelSize(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[Width + 1, Height + 1];
            FogOfWar = new bool[Width + 1, Height + 1];
            Creatures = new IMappable[Width, Height];
            Items = new IMappable[Width, Height];
        }

        public ILocation Location { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public bool[,] FogOfWar { get; private set; }
        public IMappable[,] Creatures { get; private set; }
        public IMappable[,] Items { get; private set; }

        public IMappable GetCreature(Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                return Creatures[position.X, position.Y];
            }
            else
            {
                return null;
            }
        }

        public void SetCreature(IMappable entity, Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                Creatures[position.X, position.Y] = entity;
            }
        }

        public void ChangeObjectPosition(IMappable entity, Point targetPosition)
        {
            if (entity.Position != null)
            {
                entity.Location.Tilemap.SetCreature(null, entity.Position);
            }
            if (targetPosition != null)
            {
                entity.Location.Tilemap.SetCreature(entity, targetPosition);
            }
            entity.Position = targetPosition;
        }

        public void ChangeObjectLocation(IMappable mappableObject, ILocation targetLocation, Point targetPosition)
        {
            if (mappableObject.Location != null && mappableObject.Position != null)
            {
                mappableObject.Location.Tilemap.SetCreature(null, mappableObject.Position);
            }
            if (targetLocation != null && targetPosition != null)
            {
                targetLocation.Tilemap.SetCreature(mappableObject, targetPosition);
            }
            mappableObject.Location = targetLocation;
            mappableObject.Position = targetPosition;
        }

        public IMappable GetItem(Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                return Items[position.X, position.Y];
            }
            else
            {
                return null;
            }
        }

        public void SetItem(IMappable entity, Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                Items[position.X, position.Y] = entity;
            }
        }

        public void ChangeItemPosition(IMappable entity, Point targetPosition)
        {
            if (entity.Position != null)
            {
                entity.Location.Tilemap.SetItem(null, entity.Position);
            }
            if (targetPosition != null)
            {
                Location.Tilemap.SetItem(entity, targetPosition);
            }
            entity.Position = targetPosition;
        }

        public void ChangeItemLocation(IMappable entity, ILocation targetLocation, Point targetPosition)
        {
            if (entity.Location != null && entity.Position != null)
            {
                entity.Location.Tilemap.SetItem(null, entity.Position);
            }
            if (targetLocation != null && targetPosition != null)
            {
                targetLocation.Tilemap.SetItem(entity, targetPosition);
            }
            entity.Location = targetLocation;
            entity.Position = targetPosition;
        }

        public Tile GetTile(Point position)
        {
            if (IsPositionWithinTilemap(position))
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
            if (IsPositionWithinTilemap(position))
            {
                Tiles[position.X, position.Y] = new Tile(type);
            }
        }

        public void FillWithType(int left, int top, int right, int bottom, Tile.TileType type)
        {
            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                {
                    if (IsPositionWithinTilemap(new Point(x, y)))
                    {
                        Tiles[x, y] = new Tile(type);
                    }
                }
        }

        public void FillWithType(Rectangle rectangle, Tile.TileType type)
        {
            FillWithType(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, type);
        }

        public bool IsPositionWithinTilemap(Point position)
        {
            return position.X >= 0 && position.X < Width && position.Y >= 0 && position.Y < Height;
        }

        public Point RandomPosition(Rectangle rect)
        {
            return new Point(Numbers.RandomNumber(rect.Left, rect.Right), Numbers.RandomNumber(rect.Top, rect.Bottom));
        }

        public bool IsWalkable(Point position)
        {
            return GetTile(position) != null && (GetTile(position).Type == Tile.TileType.floor || GetTile(position).Type == Tile.TileType.exit);
        }

        public void UpdateObjects(Player player)
        {
            toUpdate.Clear();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    FilterDeadCreatures(y, x);
                    FilterDeadItems(y, x);
                }
            }

            for (int u = 0; u < toUpdate.Count(); u++)
            {
                if (!toUpdate[u].IsDead)
                {
                    toUpdate[u].Update(player);
                }
            }
        }

        private void FilterDeadItems(int y, int x)
        {
            IMappable o = GetItem(new Point(x, y));
            if (o != null)
            {
                if (o.IsDead)
                {
                    RemoveItem(o);
                }
                else
                {
                    toUpdate.Add(o);
                }
            }
        }

        public void RemoveItem(IMappable o)
        {
            SetItem(null, new Point(o.Position.X, o.Position.Y));
        }

        private void FilterDeadCreatures(int y, int x)
        {
            IMappable o = GetCreature(new Point(x, y));
            if (o != null)
            {
                if (o.IsDead)
                {
                    RemoveCreature(o);
                }
                else
                {
                    toUpdate.Add(o);
                }
            }
        }

        public void RemoveCreature(IMappable o)
        {
            SetCreature(null, new Point(o.Position.X, o.Position.Y));
        }

        public void PerformOnVisibleTiles(Action<Point> action, Graphics render, Player player)
        {
            Point cameraPosition = new Point(player.Position.X - render.Width / 2, player.Position.Y - render.Height / 2);

            for (int y = cameraPosition.Y; y < cameraPosition.Y + render.Height; y++)
                for (int x = cameraPosition.X; x < cameraPosition.X + render.Width; x++)
                {
                    var pos = new Point(x, y);
                    if (player.CanSee(pos))
                    {
                        action(pos);
                    }
                }
        }

        public void Draw(Graphics render, Player player)
        {
            PerformOnVisibleTiles((point) => DrawPosition(point, render, player), render, player);
        }

        private void DrawPosition(Point pos, Graphics render, Player player)
        {
            int xCameraTransform = player.Position.X - render.Width / 2;
            int yCameraTransform = player.Position.Y - render.Height / 2;

            if (GetTile(pos) != null)
            {
                if (render.IsWithinBuffer(pos.X - xCameraTransform, pos.Y - yCameraTransform))
                {
                    DrawTile(render, player, xCameraTransform, yCameraTransform, pos);
                }
            }
            if (GetItem(pos) != null && render.IsWithinBuffer(pos.X - xCameraTransform, pos.Y - yCameraTransform) && player.CanSee(pos))
                render.DrawChar(GetItem(pos).Symbol, pos.X - xCameraTransform, pos.Y - yCameraTransform);

            if (GetCreature(pos) != null && render.IsWithinBuffer(pos.X - xCameraTransform, pos.Y - yCameraTransform) && player.CanSee(pos))
                render.DrawChar(GetCreature(pos).Symbol, pos.X - xCameraTransform, pos.Y - yCameraTransform);
        }

        private void DrawTile(Graphics render, Player player, int xCameraTransform, int yCameraTransform, Point pos)
        {
            char symbol = GetTile(pos).Symbol;

            if (IsTileVisible(pos, player))
            {
                render.DrawChar(symbol, pos.X - xCameraTransform, pos.Y - yCameraTransform);
            }
        }

        private bool IsTileVisible(Point pos, Player player)
        {
            List<Tile> neighbours = new List<Tile>();
            for (int w = pos.X - 1; w <= pos.X + 1; w++)
                for (int h = pos.Y - 1; h <= pos.Y + 1; h++)
                {
                    var neighbour = new Point(w, h);
                    if (IsPositionWithinTilemap(neighbour))
                        neighbours.Add(Tiles[w, h]);
                }

            int floor = 0;
            foreach (Tile t in neighbours)
            {
                if (t.Type == Tile.TileType.floor || t.Type == Tile.TileType.exit)
                    floor++;
            }

            return floor >= 1 && player.CanSee(pos);
        }
    }
}
