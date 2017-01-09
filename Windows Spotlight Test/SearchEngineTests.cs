using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.SearchEngine;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using System.Net.NetworkInformation;
using System.Net;
using Telerik.JustMock;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class SearchEngineTests
    {

        [TestMethod]
        public void TestSearch()
        {
            SearchEngine engine = new SearchEngine();
            List<IResultItem> results = engine.Search("google");

            Assert.AreEqual(5, results.Count);
            Assert.AreEqual("Google", results[0].Title);
            Assert.AreEqual("登入- Google 帳戶 - Google Accounts", results[1].Title);
            Assert.AreEqual("Google", results[2].Title);
            Assert.AreEqual("Google - YouTube", results[3].Title);
            Assert.AreEqual("Google | Facebook", results[4].Title);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "找不到符合搜尋字詞的網頁")]
        public void TestSearchFail()
        {
            SearchEngine engine = new SearchEngine();
            List<IResultItem> results = engine.Search("googleasdasdqweqQWEOikjasdlk; jasdl; kjqwn'asdasdasd");
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(WebException), "沒有連接至網際網路")]
        public void TestNetworkAvailable()
        {
            Mock.Arrange(() => NetworkInterface.GetIsNetworkAvailable()).Returns(false);
            SearchEngine engine = new SearchEngine();
            List<IResultItem> results = engine.Search("google");
            Assert.Fail();
        }

    }
}
