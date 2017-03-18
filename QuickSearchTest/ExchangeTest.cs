using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class ExchangeTest
    {
        private Exchange _exchange;

        [TestInitialize]
        public void TestInit()
        {
            _exchange = new Exchange();
        }

        [TestMethod]
        public void TestCurrecnyExchange()
        {
            _exchange.Currency = "1000JPY";
            string result = _exchange.GetResult();
            Assert.AreEqual("289.79NT", result);
        }

    }
}
