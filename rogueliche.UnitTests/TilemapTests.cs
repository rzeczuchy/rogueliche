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
            var tilemap = TestMap(12, 23);

            Assert.AreEqual(tilemap.GetTile(tilemap.Bounds.Center).Type, Tile.TileType.floor);
            Assert.AreEqual(tilemap.GetTile(new Point(tilemap.Bounds.Right + 1, tilemap.Bounds.Bottom + 1)), null);
        }

        [TestMethod]
        public void SetTile_SetsTileType()
        {
            var tilemap = TestMap(20, 30);
            var pos = tilemap.Bounds.Center;
            var defaultType = Tile.TileType.floor;
            var testType = Tile.TileType.wall;
            tilemap.SetTile(pos, testType);

            Assert.AreNotEqual(testType, defaultType);
            Assert.AreEqual(tilemap.GetTile(pos).Type, testType);
            Assert.AreNotEqual(tilemap.GetTile(new Point(pos.X + 1, pos.Y + 1)).Type, testType);
        }

        private Tilemap TestMap(int width, int height)
        {
            var mapSize = new Point(width, height);
            var tilemap = new Tilemap(null, mapSize.X, mapSize.Y);
            tilemap.FillWithTile(tilemap.Bounds, Tile.TileType.floor);
            return tilemap; 
        }
    }
}
