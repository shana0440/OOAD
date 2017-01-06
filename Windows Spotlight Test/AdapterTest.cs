using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight;
using System.Threading;
using System.Collections.ObjectModel;
using WPF_Windows_Spotlight.Foundation;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class AdapterTest
    {
        private Adapter _adapter;

        [TestInitialize]
        public void TestInit()
        {
            _adapter = new Adapter();
        }

        [TestMethod]
        public void TestAdapterSearch()
        {
            _adapter.Search("AnswerItem.cs");

            Thread.Sleep(1000);

            ObservableCollection<Item> list = _adapter.QueryList;
            Assert.AreEqual(6, list.Count); // 五個網頁 + 一個檔案
        }

        [TestMethod]
        public void TestSelectItem()
        {
            _adapter.Search("notepad");

            Thread.Sleep(1000);

            Assert.AreEqual(0, _adapter.SelectedIndex);
            _adapter.SelectItem(1);
            Assert.AreEqual(1, _adapter.SelectedIndex);

            _adapter.SelectItem(-1);
            Assert.AreEqual(0, _adapter.SelectedIndex);

            _adapter.SelectItem(_adapter.QueryList.Count);
            Assert.AreEqual(_adapter.QueryList.Count - 1, _adapter.SelectedIndex);
        }
    }
}
