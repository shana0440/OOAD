using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FileSystem : IFoundation
    {
        private string _keyword;

        public FileSystem(string keyword = "")
        {
            _keyword = keyword;
        }

        public string Keyword {
            set { _keyword = value; }
        }

        public IEnumerable<string> Search()
        {
            const int bufsize = 260;
            StringBuilder buf = new StringBuilder(bufsize);
            Everything.Everything_SetSearchW(_keyword);
            Everything.Everything_QueryW(true);
            for (int i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, buf, bufsize);

                // 在return type是IEnumerable的時候, yield return之後還會繼續執行
                yield return buf.ToString();
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            List<Item> list = new List<Item>();
            foreach (var result in results)
            {
                Item item = new Item(result);
                list.Add(item);
            }
            e.Result = list;
        }

        
    }
}
