using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace rogueliche.UnitTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Game_Initializes()
        {
            try
            {
                var game = new Game(true);
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
