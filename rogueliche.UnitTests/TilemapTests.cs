﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class TilemapTests
    {
        [TestMethod]
        public void GetTile_ReturnsTile()
        {
            var tilemap = new Tilemap(null, 12, 34);
            var type = Tile.TileType.floor;
            tilemap.FillWithTile(tilemap.Bounds, type);

            Assert.AreEqual(tilemap.GetTile(tilemap.Bounds.Center).Type, type);
            Assert.AreEqual(tilemap.GetTile(new Point(tilemap.Bounds.Right + 1, tilemap.Bounds.Bottom + 1)), null);
        }

        [TestMethod]
        public void SetTile_SetsTileType()
        {
            var tilemap = new Tilemap(null, 20, 30);
            var defaultType = Tile.TileType.floor;
            tilemap.FillWithTile(tilemap.Bounds, defaultType);
            var pos = tilemap.Bounds.Center;
            var testType = Tile.TileType.wall;
            tilemap.SetTile(pos, testType);

            Assert.AreNotEqual(testType, defaultType);
            Assert.AreEqual(tilemap.GetTile(pos).Type, testType);
            Assert.AreNotEqual(tilemap.GetTile(new Point(pos.X + 1, pos.Y + 1)).Type, testType);
        }

        [TestMethod]
        public void FillWithType_FillsWithType()
        {
            var tilemap = new Tilemap(null, 10, 4);
            var testedType = Tile.TileType.floor;
            tilemap.FillWithTile(tilemap.Bounds, testedType);

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(tilemap.GetTile(tilemap.RandomPosition(tilemap.Bounds)).Type, testedType);
            }
        }

        [TestMethod]
        public void ContainsPosition_ChecksIfPositionContanied()
        {
            var tilemap = new Tilemap(null, 34, 86);

            Assert.IsTrue(tilemap.ContainsPosition(tilemap.Bounds.Center));
            Assert.IsFalse(tilemap.ContainsPosition(new Point(tilemap.Bounds.Left - 1, tilemap.Bounds.Center.Y)));
            Assert.IsFalse(tilemap.ContainsPosition(new Point(tilemap.Bounds.Center.X, tilemap.Bounds.Bottom + 1)));
            Assert.IsFalse(tilemap.ContainsPosition(new Point(tilemap.Bounds.Right + 1, tilemap.Bounds.Top - 1)));
        }

        [TestMethod]
        public void RandomPosition_ReturnsDifferentPositions()
        {
            var tilemap = new Tilemap(null, 50, 34);
            var randomPos = tilemap.RandomPosition();
            int sameResults = 0;
            int attempts = 100;

            while (attempts > 0)
            {
                var pos = tilemap.RandomPosition();

                if (pos == randomPos)
                {
                    sameResults++;
                }

                attempts--;
            }

            Assert.IsTrue(sameResults < 10);
        }
    }
}
