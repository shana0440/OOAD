using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight;
using System.Threading;
using System.Collections.ObjectModel;
using WPF_Windows_Spotlight.Foundation;

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
            _adapter.Search("putty.exe");

            Thread.Sleep(1000);

            ObservableCollection<Item> list = _adapter.QueryList;
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void TestSelectItem()
        {
            _adapter.Search("putty");

            Thread.Sleep(1000);

            Assert.AreEqual(0, _adapter.SelectedIndex);
            _adapter.SelectItem(1);
            Assert.AreEqual(1, _adapter.SelectedIndex);
            _adapter.SelectItem(2);
            Assert.AreEqual(2, _adapter.SelectedIndex);

            _adapter.SelectItem(-1);
            Assert.AreEqual(0, _adapter.SelectedIndex);

            _adapter.SelectItem(_adapter.QueryList.Count);
            Assert.AreEqual(_adapter.QueryList.Count - 1, _adapter.SelectedIndex);
        }
    }
}
