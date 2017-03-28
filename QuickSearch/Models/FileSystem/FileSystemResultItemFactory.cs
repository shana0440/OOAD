using System.Collections.Generic;
using QuickSearch.Models.ResultItemsFactory;
using System.Text.RegularExpressions;
using System;

namespace QuickSearch.Models.FileSystem
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
                if (System.IO.Directory.Exists(path))
                {
                     item = new FileSystemResultFolderItem(path);
                }
                else if (System.IO.File.Exists(path))
                {
                    item = new FileSystemResultFileItem(path);
                }
                
                if (item != null)
                {
                    if (Regex.IsMatch(item.Title, @"(.(exe)|(lnk)$)"))
                    {
                        item.Priority += 50;
                        Console.WriteLine("{0} Priority is: {1}", item.Title, item.Priority);
                    }
                    results.Add(item);
                }
            }
         return results;
        }
    }
}
