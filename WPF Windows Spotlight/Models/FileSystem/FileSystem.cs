using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.FileSystem
{
    public class FileSystem
    {
        const int _searchMaxCount = 30;

        public List<string> SearchFileOrFolder(string keyword)
        {
            List<string> results = Everything.Search(keyword, _searchMaxCount);
            return results;
        }
    }
}
