using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using QuickSearch.Models.FileSystem;
using System.Threading;
using System.Collections.Generic;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearchTest
{
    [TestClass]
    public class FileSystemThreadTests
    {
        object _threadResult;

        [TestMethod]
        public void TestDoWorkSuccess()
        {
            FileSystemThread thread = new FileSystemThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("TestFileSystemFiles");
            
            while(worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.AreEqual(7, result.Count);
            foreach (var item in result)
            {
                StringAssert.Contains(item.Title, "TestFileSystemFiles");
            }
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _threadResult = e.Result;
        }
    }
}
