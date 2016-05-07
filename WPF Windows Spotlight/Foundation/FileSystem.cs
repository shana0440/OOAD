using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Drawing;

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

        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public IEnumerable<FileInfo> Search()
        {
            const int bufsize = 260;
            StringBuilder buf = new StringBuilder(bufsize);
            Everything.Everything_SetSearchW(_keyword);
            Everything.Everything_QueryW(true);
            for (int i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, buf, bufsize);
                FileInfo file;
                try
                {
                    file = new FileInfo(buf.ToString());
                    // 在return type是IEnumerable的時候, yield return之後還會繼續執行
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                yield return file;
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            List<Item> list = new List<Item>();
            foreach (FileInfo result in results)
            {
                if (result.Exists)
                {
                    Icon ico = Icon.ExtractAssociatedIcon(result.FullName);
                    Item item = new Item(result.Name);
                    item.SetIcon(ico.ToBitmap());
                    list.Add(item);
                }
            }
            e.Result = list;
        }

        
    }
}
