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
            var level = new EmptyLevel("level", 25, 25);
            var plant = new HealingPlant(level, pos);
            Assert.AreEqual(level.Tilemap.Creatures.GetMappable(pos), plant);
        }
    }
}
