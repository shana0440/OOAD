using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Controller;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class SearchServiceTests
    {
        SearchService _service;
        List<IResultItem> _items;

        [TestInitialize]
        public void Initialize()
        {
            _service = new SearchService();
        }

        [TestMethod]
        public void TestSearch()
        {
            bool isSearchOver = false;
            SearchService.SearchOverEventHandler handler = new SearchService.SearchOverEventHandler(SearchOverEvent);
            _service.SubscribeSearchOverEvent(handler);
            _service.Search("1+1");

            Thread.Sleep(1000);
            Assert.AreEqual("計算機", _items[0].GroupName);
            Assert.AreEqual("2", _items[0].Title);
        }

        void SearchOverEvent(List<IResultItem> items)
        {
            _items = items;
        }
    }
}
