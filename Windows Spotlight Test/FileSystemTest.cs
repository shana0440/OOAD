using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FileSystemTest
    {
        FileSystem _fileSystem;
        List<Item> _list;
        bool _running;

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
            Assert.AreEqual(1, _list.Count);
        }

        public void TestBackgroundWokerSearchFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            _list = ((List<Item>)e.Result);
            _running = false;
        }
    }
}
