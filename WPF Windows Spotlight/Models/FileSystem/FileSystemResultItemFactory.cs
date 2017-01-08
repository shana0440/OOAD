using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.FileSystem
{
    class FileSystemResultItemFactory : IResultItemsFactory
    {
        public List<IResultItem> CreateResultItems(object result)
        {
            List<IResultItem> results = new List<IResultItem>();
            var list = (List<string>)result;
            foreach (var path in list)
            {
                IResultItem item = null;
                if (Directory.Exists(path))
                {
                     item = new FileSystemResultFolderItem(path);
                }
                else if (File.Exists(path))
                {
                    item = new FileSystemResultFileItem(path);
                }
                
                if (item != null)
                {
                    results.Add(item);
                }
            }
            return results;
        }
    }
}
