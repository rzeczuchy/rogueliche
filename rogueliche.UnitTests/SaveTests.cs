using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class SaveTests
    {
        [TestMethod]
        public void Save_Initializes()
        {
            try
            {
                var player = new Player(new EmptyLevel("name", 10, 10), new Point(1,1));
                var emptySave = new Save();
                var save = new Save(player);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Save_ThrowsExceptionIfPlayerNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Save(null));
        }
    }
}
