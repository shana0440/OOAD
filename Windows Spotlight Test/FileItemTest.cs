using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF_Windows_Spotlight.Foundation.ItemType;
using WPF_Windows_Spotlight.Foundation;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Windows_Spotlight_Test
{
    [TestClass]
    public class FileItemTest
    {

        [TestMethod]
        public void TestFileGetProperty()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string[] filepath = Directory.GetFiles(currentDir, "WPF Windows Spotlight.exe");
            FolderOrFile file = new FolderOrFile(filepath[0]);
            FileItem item = new FileItem(file);
            List<KeyValuePair<string, string>> propertys = item.GetProperty();
            Assert.AreEqual("Created", propertys[0].Key);
            Assert.AreEqual("Modified", propertys[1].Key);
            Assert.AreEqual("Accessed", propertys[2].Key);
        }

        [TestMethod]
        public void TestOpen()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string[] filepath = Directory.GetFiles(currentDir, "filePriority.xml");
            FolderOrFile file = new FolderOrFile(filepath[0]);
            FileItem item = new FileItem(file);
            try
            {
                item.Open();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Can't open this file or folder", e.Message);
            }
            
        }
    }
}
