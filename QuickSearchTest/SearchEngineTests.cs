using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.SearchEngine;
using QuickSearch.Models.ResultItemsFactory;
using System.Net.NetworkInformation;
using System.Net;
using Telerik.JustMock;

namespace QuickSearchTest
{
    [TestClass]
    public class SearchEngineTests
    {

        [TestMethod]
        public void TestSearch()
        {
            SearchEngine engine = new SearchEngine();
            List<IResultItem> results = engine.Search("facebook");

            Assert.AreEqual(5, results.Count);
            Assert.AreEqual("登入Facebook | Facebook", results[0].Title);
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
