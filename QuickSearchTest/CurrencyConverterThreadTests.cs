using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickSearch.Models.CurrencyConverter;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using QuickSearch.Models.ResultItemsFactory;
using Telerik.JustMock;
using System.Net.NetworkInformation;
using System.Net;
using QuickSearch.Models;

namespace QuickSearchTest
{
    [TestClass]
    public class CurrencyConverterThreadTests
    {
        object _threadResult;

        [TestMethod]
        public void TestDoWorkSuccess()
        {
            var url = "https://www.google.com/finance/converter?a=1000&from=JPY&to=TWD";
            Mock.SetupStatic(typeof(Crawler), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => Crawler.GetResponse(url)).Returns("<span class=bld>277.2000 TWD</span>");
            CurrencyConverterThread thread = new CurrencyConverterThread();
            DoWorkEventArgs e = new DoWorkEventArgs("1000JPY");
            thread.DoWork("0.0", e);
            
            Thread.Sleep(1000);
            List<IResultItem> result = (List<IResultItem>)e.Result;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("277.2000 TWD", result[0].Title);
        }

        [TestMethod]
        public void TestDoWorkWhenKeywordLengthLess4()
        {
            CurrencyConverterThread thread = new CurrencyConverterThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("JPY");

            Thread.Sleep(1000);
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestNetworkAvailable()
        {
            Mock.Arrange(() => NetworkInterface.GetIsNetworkAvailable()).Returns(false);

            CurrencyConverterThread thread = new CurrencyConverterThread();
            DoWorkEventArgs e = new DoWorkEventArgs("1000JPY");
            thread.DoWork("0.0", e);

            Thread.Sleep(1000);
            Assert.IsNull(e.Result);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _threadResult = e.Result;
        }
    }
}
