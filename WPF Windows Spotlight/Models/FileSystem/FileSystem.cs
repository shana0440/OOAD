using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.FileSystem
{
    public class FileSystem
    {
        const int _searchMaxCount = 30;

        public List<string> Search(string keyword)
        {
            List<string> results = Everything.Search(keyword, _searchMaxCount);
            return results;
        }
    }
}
