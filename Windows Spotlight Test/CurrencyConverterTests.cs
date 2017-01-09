using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.CurrencyConverter;
using Telerik.JustMock;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class CurrencyConverterTests
    {
        [TestMethod]
        public void TestConvert()
        {
            CurrencyConverter converter = new CurrencyConverter();
            string result = converter.Convert("1000", "JPY");
            Assert.AreEqual("276.2000 TWD", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "沒有找到對應的貨幣")]
        public void TestConvertFail()
        {
            CurrencyConverter converter = new CurrencyConverter();
            string result = converter.Convert("1000", "NotThisCurrency");
            Assert.Fail();
        }

    }
}
