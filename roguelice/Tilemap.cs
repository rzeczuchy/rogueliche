using System;
using System.Collections.Generic;
using System.Linq;

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
            Tiles = new Tile[Width, Height];
            FogOfWar = new bool[Width, Height];
            FieldOfVisibility = new bool[Width, Height];
            Creatures = new IMappable[Width, Height];
            Items = new IMappable[Width, Height];
        }

        public ILocation Location { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public bool[,] FogOfWar { get; private set; }
        public bool[,] FieldOfVisibility { get; private set; }
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

        public void RemoveItem(IMappable o)
        {
            SetItem(null, new Point(o.Position.X, o.Position.Y));
        }

        public void RemoveCreature(IMappable o)
        {
            SetCreature(null, new Point(o.Position.X, o.Position.Y));
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

        public void FillWithTile(int left, int top, int right, int bottom, Tile.TileType type)
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

        public void FillWithTile(Rectangle rectangle, Tile.TileType type)
        {
            FillWithTile(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, type);
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

        public void Update(Player player)
        {
            UpdateObjects(player);
            UpdateFogOfWar(player);
            UpdateFieldOfVisibility(player);
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
            return IsPositionWithinTilemap(pos) && FogOfWar[pos.X, pos.Y] == true;
        }

        public bool IsVisible(Point pos)
        {
            return IsPositionWithinTilemap(pos) && FieldOfVisibility[pos.X, pos.Y] == true;
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
            if (GetCreature(pos) is IMappable creature)
            {
                return creature;
            }
            else if (GetItem(pos) is IMappable item)
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
                    if (IsPositionWithinTilemap(neighbour))
                        neighbours.Add(Tiles[w, h]);
                }

            return neighbours;
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
    }
}
