using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Controller;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class SearchServiceTests
    {
        SearchService _service;

        [TestInitialize]
        public void Initialize()
        {
            _service = new SearchService();
        }

        [TestMethod]
        public void TestSearch()
        {
            bool isSearchOver = false;
            SearchService.SearchOverEventHandler handler = new SearchService.SearchOverEventHandler((List<Item> items) => isSearchOver = true);
            _service.SubscribeSearchOverEvent(handler);
            _service.Search("1+1");
            Thread.Sleep(1000);
            Assert.IsTrue(isSearchOver);
        }
    }
}
