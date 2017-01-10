using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using WPF_Windows_Spotlight.Models.Dictionary;
using System.Threading;
using System.Collections.Generic;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using Telerik.JustMock;
using System.Net.NetworkInformation;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class DictionaryThreadTests
    {
        object _threadResult;

        [TestMethod]
        public void TestDoWorkSuccess()
        {
            DictionaryThread thread = new DictionaryThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("apple");

            while (worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestDoWorkFail()
        {
            DictionaryThread thread = new DictionaryThread();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += thread.DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync("googleqweoirjwflszjksdhf;laewoijfdz;sldkfj");

            while (worker.IsBusy)
            {
                Thread.Sleep(1000);
            }
            List<IResultItem> result = (List<IResultItem>)_threadResult;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestNetworkAvailable()
        {
            Mock.SetupStatic(typeof(NetworkInterface), Behavior.Strict, StaticConstructor.Mocked);
            Mock.Arrange(() => NetworkInterface.GetIsNetworkAvailable()).Returns(false);

            DictionaryThread thread = new DictionaryThread();
            DoWorkEventArgs e = new DoWorkEventArgs("keyword");
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
