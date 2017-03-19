using System.Collections.Generic;

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
