using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.FileSystem
{
    public class FileSystemThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            FileSystem fileSystem = new FileSystem();
            try
            {
                List<string> list = fileSystem.Search(keyword);
                FileSystemResultItemFactory factory = new FileSystemResultItemFactory();
                List<IResultItem> results = factory.CreateResultItems(list);
                var worker = (BackgroundWorker)sender;
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Result = results;
                }
            }
            catch (Exception)
            {
                e.Result = null;
            }
        }
    }
}
