using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.FileSystem
{
    public class FileSystemThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            FileSystem fileSystem = new FileSystem();
            try
            {
                List<string> list = fileSystem.SearchFileOrFolder(keyword);
                FileSystemResultItemFactory factory = new FileSystemResultItemFactory();
                List<IResultItem> results = factory.CreateResultItems(list);
                e.Result = results;
            }
            catch (Exception)
            {
                e.Cancel = true;
            }

        }
    }
}
