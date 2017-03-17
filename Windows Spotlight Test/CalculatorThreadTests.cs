using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using QuickSearch.Models.Calculator;
using System.Threading;
using System.Collections.Generic;
using QuickSearch.Models.ResultItemsFactory;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class CalculatorThreadTests
    {
        object _threadResult;

        [TestMethod]
        public void TestDoWorkSuccess()
        {
            CalculatorThread thread = new CalculatorThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("1 + 1");

            Thread.Sleep(1000);
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("2", result[0].Title);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _threadResult = e.Result;
        }
    }
}
