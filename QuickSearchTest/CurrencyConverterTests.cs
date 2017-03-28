using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.CurrencyConverter;
using System.Net;
using Telerik.JustMock;
using System.Net.NetworkInformation;
using QuickSearch.Models;

namespace QuickSearchTest
{
    [TestClass]
    public class CurrencyConverterTests
    {
        [TestMethod]
        public void TestConvert()
        {
            var url = "https://www.google.com/finance/converter?a=1000&from=JPY&to=TWD";
            Mock.SetupStatic(typeof(Crawler), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => Crawler.GetResponse(url)).Returns("<span class=bld>277.2000 TWD</span>");
            CurrencyConverter converter = new CurrencyConverter();
            string result = converter.Convert("1000JPY");
            Assert.AreEqual("277.2000 TWD", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "貨幣輸入不合法")]
        public void TestConvertUnLegal()
        {
            CurrencyConverter converter = new CurrencyConverter();
            string result = converter.Convert("1000NotThisCurrency");
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "沒有找到對應的貨幣")]
        public void TestConvertFail()
        {
            CurrencyConverter converter = new CurrencyConverter();
            string result = converter.Convert("1000NT$");
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(WebException), "沒有連接至網際網路")]
        public void TestNetworkAvailable()
        {
            Mock.SetupStatic(typeof(NetworkInterface), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => NetworkInterface.GetIsNetworkAvailable()).Returns(false);
            CurrencyConverter converter = new CurrencyConverter();

            string result = converter.Convert("1000NotThisCurrency");

            Assert.Fail();
        }

    }
}
