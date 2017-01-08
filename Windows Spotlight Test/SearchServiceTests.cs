using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Controller;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using System.Collections.ObjectModel;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class SearchServiceTests
    {
        SearchService _service;
        ObservableCollection<IResultItem> _items;

        [TestInitialize]
        public void Initialize()
        {
            _service = new SearchService();
            _items = new ObservableCollection<IResultItem>();
        }

        [TestMethod]
        public void TestSearch()
        {
            SearchService.SearchOverEventHandler handler = new SearchService.SearchOverEventHandler(SearchOverEvent);
            _service.SubscribeSearchOverEvent(handler);
            _service.Search("1+1");

            Thread.Sleep(1000);
            Assert.AreEqual("計算機", _items[0].GroupName);
            Assert.AreEqual("2", _items[0].Title);
        }

        void SearchOverEvent(ObservableCollection<IResultItem> items)
        {
            _items = items;
        }

        [TestMethod]
        public void TestCancelSearch()
        {
            SearchService.SearchOverEventHandler handler = new SearchService.SearchOverEventHandler(SearchOverEvent);
            _service.SubscribeSearchOverEvent(handler);
            _service.Search("1+1");
            _service.CancelSearch();

            Thread.Sleep(1000);
            Assert.AreEqual(0, _items.Count);
        }
    }
}
