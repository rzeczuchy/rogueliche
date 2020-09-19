using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roguelice
{
    class Tilemap
    {
        public Tilemap(DungeonLevel location, int width, int height)
        {
            Location = location;

            SetLevelSize(width, height);
        }

        private void SetLevelSize(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[Width + 1, Height + 1];
            FogOfWar = new bool[Width + 1, Height + 1];
            Creatures = new IMapObject[Width, Height];
            Items = new IMapObject[Width, Height];
        }
        
        public DungeonLevel Location { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public bool[,] FogOfWar { get; private set; }
        public IMapObject[,] Creatures { get; private set; }
        public IMapObject[,] Items { get; private set; }

        public IMapObject GetCreature(Point position)
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

        public void SetCreature(IMapObject entity, Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                Creatures[position.X, position.Y] = entity;
            }
        }

        public void ChangeObjectPosition(IMapObject entity, Point targetPosition)
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

        public void ChangeObjectLocation(IMapObject entity, DungeonLevel targetLocation, Point targetPosition)
        {
            if (entity.Location != null && entity.Position != null)
            {
                entity.Location.Tilemap.SetCreature(null, entity.Position);
            }
            if (targetLocation != null && targetPosition != null)
            {
                targetLocation.Tilemap.SetCreature(entity, targetPosition);
            }
            entity.Location = targetLocation;
            entity.Position = targetPosition;
        }

        public IMapObject GetItem(Point position)
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

        public void SetItem(IMapObject entity, Point position)
        {
            if (IsPositionWithinTilemap(position))
            {
                Items[position.X, position.Y] = entity;
            }
        }

        public void ChangeItemPosition(IMapObject entity, Point targetPosition)
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

        public void ChangeItemLocation(IMapObject entity, DungeonLevel targetLocation, Point targetPosition)
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

        public bool IsWalkable(Point position)
        {
            return GetTile(position) != null && (GetTile(position).Type == Tile.TileType.floor || GetTile(position).Type == Tile.TileType.exit);
        }

        public void Draw(Graphics render, Player player)
        {
            int xCameraTransform = player.Position.X - render.Width / 2;
            int yCameraTransform = player.Position.Y - render.Height / 2;

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    var pos = new Point(x, y);

                    if (Tiles[x, y] != null)
                    {
                        if (render.IsWithinBuffer(x - xCameraTransform, y - yCameraTransform))
                        {
                            DrawTile(render, player, xCameraTransform, yCameraTransform, y, x);
                        }
                    }
                    if (Items[x, y] != null && render.IsWithinBuffer(x - xCameraTransform, y - yCameraTransform) && player.CanSee(x, y))
                        render.DrawChar(GetItem(pos).Symbol, x - xCameraTransform, y - yCameraTransform);

                    if (Creatures[x, y] != null && render.IsWithinBuffer(x - xCameraTransform, y - yCameraTransform) && player.CanSee(x, y))
                        render.DrawChar(GetCreature(pos).Symbol, x - xCameraTransform, y - yCameraTransform);
                }
        }

        private void DrawTile(Graphics render, Player player, int xCameraTransform, int yCameraTransform, int y, int x)
        {
            char symbol;
            switch (Tiles[x, y].Type)
            {
                case Tile.TileType.floor:
                    symbol = ' ';
                    break;
                case Tile.TileType.wall:
                    symbol = '\u2591';
                    break;
                case Tile.TileType.exit:
                    symbol = 'E';
                    break;
                default:
                    symbol = '?';
                    break;
            }

            List<Tile> neighbours = new List<Tile>();
            for (int w = x - 1; w <= x + 1; w++)
                for (int h = y - 1; h <= y + 1; h++)
                {
                    var pos = new Point(w, h);
                    if (IsPositionWithinTilemap(pos))
                        neighbours.Add(Tiles[w, h]);
                }

            int floor = 0;
            foreach (Tile t in neighbours)
            {
                if (t.Type == Tile.TileType.floor || t.Type == Tile.TileType.exit)
                    floor++;
            }

            if (floor >= 1 && player.CanSee(x, y))
            {
                render.DrawChar(symbol, x - xCameraTransform, y - yCameraTransform);
            }
        }

    }


}
