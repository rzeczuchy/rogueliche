using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class TilemapLayerTests
    {
        [TestMethod]
        public void TilemapLayer_Initializes()
        {
            try
            {
                var dungeon = new Dungeon();
                var level = dungeon.NewLevel();
                new TilemapLayer(level.Tilemap);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Constructor_ThrowsExceptionWhenTilemapNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { new TilemapLayer(null); });
        }

        [TestMethod]
        public void GetMappable_ReturnsMappableAtPosition()
        {
            var pos = new Point(1, 1);
            var level = new EmptyLevel("level", pos.X + 10, pos.Y + 10);
            var plant = new HealingPlant(level, pos);

            Assert.AreEqual(level.Tilemap.Creatures.GetMappable(pos), plant);
        }

        [TestMethod]
        public void GetMappable_ReturnsNullWhenNoMappableAtPosition()
        {
            var pos = new Point(1, 1);
            var level = new EmptyLevel("level", pos.X + 10, pos.Y + 10);

            Assert.AreEqual(level.Tilemap.Creatures.GetMappable(pos), null);
        }

        [TestMethod]
        public void SetMappable_SetsMappableAtPosition()
        {
            var pos = new Point(1, 1);
            var level = new EmptyLevel("level", pos.X + 10, pos.Y + 10);
            var item = new Weapon(null, null, new WeaponType("type", 'W', 1, 1, 1), null);

            Assert.AreEqual(level.Tilemap.Items.GetMappable(pos), null);

            level.Tilemap.Items.SetMappable(item, pos);

            Assert.AreEqual(level.Tilemap.Items.GetMappable(pos), item);
        }

        [TestMethod]
        public void SetMappable_ThrowsNoExceptionWhenPositionOutOfBounds()
        {
            var level = new EmptyLevel("level", 10, 10);
            var pos = new Point(level.Bounds.Right + 1, level.Bounds.Bottom + 1);

            try
            {
                level.Tilemap.Creatures.SetMappable(null, pos);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RemoveDeatAtPosition_RemovesOnlyDeadMappables()
        {
            var pos = new Point(1, 1);
            var level = new EmptyLevel("level", pos.X + 10, pos.Y + 10);
            var plant = new HealingPlant(level, pos);

            level.Tilemap.Creatures.FilterDeatAtPosition(pos);
            Assert.AreEqual(level.Tilemap.Creatures.GetMappable(pos), plant);

            plant.IsDead = true;
            level.Tilemap.Creatures.FilterDeatAtPosition(pos);
            Assert.AreEqual(level.Tilemap.Creatures.GetMappable(pos), null);
        }
    }
}
