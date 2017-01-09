using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Models.CurrencyConverter;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

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

            while (worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("275.1000 TWD", result[0].Title);
        }

        [TestMethod]
        public void TestDoWorkWhenKeywordLengthLess4()
        {
            CurrencyConverterThread thread = new CurrencyConverterThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("JPY");

            while (worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.IsNull(result);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _threadResult = e.Result;
        }
    }
}
