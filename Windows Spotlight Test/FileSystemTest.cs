using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FileSystemTest
    {
        FileSystem _fileSystem;
        KeyValuePair<string, List<Item>> _list;
        bool _running;
        private bool _canceled;

        [TestInitialize]
        public void TestInit()
        {
            _fileSystem = new FileSystem();
        }

        [TestMethod]
        public void TestSearchFile()
        {
            Assert.AreEqual(1, _fileSystem.Search("AnswerItem.cs").Count);
            Assert.AreEqual(0, _fileSystem.Search("").Count);
        }

        [TestMethod]
        public void TestDoWork()
        {
            BackgroundWorker woker = new BackgroundWorker();
            _fileSystem.SetKeyword("AnswerItem.cs");
            woker.DoWork += _fileSystem.DoWork;
            woker.RunWorkerCompleted += TestBackgroundWokerSearchFileComplete;
            woker.RunWorkerAsync();
            _running = true;
            while(_running)
            {
                Thread.Sleep(100);
            }
            Assert.AreEqual(1, _list.Value.Count);
        }

        public void TestBackgroundWokerSearchFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                _list = (KeyValuePair<string, List<Item>>)e.Result;
                _running = false;
            }
            else
            {
                _running = false;
                _canceled = e.Cancelled;
            }
            _running = false;
        }

        [TestMethod]
        public void TestCancelDoWork()
        {
            BackgroundWorker woker = new BackgroundWorker();
            _fileSystem.SetKeyword("AnswerItem.cs");
            woker.DoWork += _fileSystem.DoWork;
            woker.RunWorkerCompleted += TestBackgroundWokerSearchFileComplete;
            woker.WorkerSupportsCancellation = true;
            woker.RunWorkerAsync();
            woker.CancelAsync();
            _running = true;
            while (_running)
            {
                Thread.Sleep(100);
            }
            Assert.AreEqual(true, _canceled);
        }

    }
}
