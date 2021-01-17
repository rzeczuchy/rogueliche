using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class TilemapTests
    {
        [TestMethod]
        public void GetTile_ReturnsTile()
        {
            var mapSize = new Point(12, 23);
            var tilemap = new Tilemap(null, mapSize.X, mapSize.Y);
            tilemap.FillWithTile(tilemap.Bounds, Tile.TileType.floor);

            Assert.AreEqual(tilemap.GetTile(new Point(mapSize.X - 1, mapSize.Y - 1)).Type, Tile.TileType.floor);
            Assert.AreEqual(tilemap.GetTile(new Point(mapSize.X + 1, mapSize.Y + 1)), null);
        }
    }
}
