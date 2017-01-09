using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.CurrencyConverter;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using Telerik.JustMock;
using System.Net.NetworkInformation;
using System.Net;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class CurrencyConverterThreadTests
    {
        object _threadResult;

        [TestMethod]
        public void TestDoWorkSuccess()
        {
            CurrencyConverterThread thread = new CurrencyConverterThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("1000JPY");

            Thread.Sleep(1000);
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("276.2000 TWD", result[0].Title);
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
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.IsNull(result);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _threadResult = e.Result;
        }
    }
}
