using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight;
using System.Threading;

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
        public void TestCalculaterBackgroundWorker()
        {
            //_adapter.Search("1+1");

            //Thread.Sleep(1000);

            //string result = _adapter.GetResult();
            //Assert.AreEqual(Item, result);
        }
    }
}
