using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class ExchangeTester
    {
        private Exchange _system;

        [TestInitialize]
        public void TestInit()
        {
            _system = new Exchange();
        }

        [TestMethod]
        public void TestExchange()
        {
            Assert.AreEqual("3271.2", _system.ExchangeCurrency("100USD"));
            Assert.AreEqual("29.89", _system.ExchangeCurrency("100JPY"));
            Assert.AreEqual("501.6", _system.ExchangeCurrency("100CNY"));
        }
    }
}
