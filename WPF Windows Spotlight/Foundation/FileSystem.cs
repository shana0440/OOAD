using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FileSystem : BaseFoundation
    {
        private string _keyword;
        private readonly Bitmap _dirIcon;
        private const int SearchMaxCount = 10;
        private readonly string[] _sortOrder = {".exe", ".lnk"};

        public FileSystem(string name = "") : base(name)
        {
            _dirIcon = (Bitmap)WPF_Windows_Spotlight.Properties.Resources.folder_icon;
            _dirIcon.MakeTransparent();
        }

        public override void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public List<FolderOrFile> Search(string keyword = "")
        {
            if (keyword == "" && _keyword != null) keyword = _keyword;
            if (keyword == "" || keyword.Length < 3) return new List<FolderOrFile>();
            Everything.Everything_SetSearchW(keyword);
            Everything.Everything_SetMax(SearchMaxCount);
            Everything.Everything_QueryW(true);
            Everything.Everything_SortResultsByPath();
            var list = GetResult();
            list = Sort(list);
            Everything.Everything_Reset();
            return list;
        }

        private List<FolderOrFile> GetResult()
        {
            var list = new List<FolderOrFile>();
            const int bufsize = 260;
            var buf = new StringBuilder(260);

            for (var i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, buf, bufsize);
                try
                {
                    var folderOrFile = new FolderOrFile(buf.ToString());
                    if (folderOrFile.Exists != null) list.Add(folderOrFile);
                }
                catch (ArgumentException e)
                {
                    //Console.WriteLine(e.Message);
                }
            }
            return list;
        }

        private List<FolderOrFile> Sort(IEnumerable<FolderOrFile> originList)
        {
            var sortedLists = new List<List<FolderOrFile>>();
            // 定義n個要搜尋的條件，第n+1個放不存在於條件內的
            for (var i = 0; i < _sortOrder.Length + 1; i++)
            {
                sortedLists.Add(new List<FolderOrFile>());
            }
            foreach (var item in originList)
            {
                var added = false;
                for (var j = 0; j < _sortOrder.Length; j++)
                {
                    if (item.Name.EndsWith(_sortOrder[j]))
                    {
                        sortedLists[j].Add(item);
                        added = true;
                    }
                }
                if (!added)
                    sortedLists[_sortOrder.Length].Add(item);
            }
            var result = new List<FolderOrFile>();
            foreach (var sortedList in sortedLists)
            {
                result.AddRange(sortedList.OrderBy(item => item.Name));
            }
            return result;
        }

        public override void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            var list = new List<Item>();
            foreach (var result in results)
            {
                var bg = sender as BackgroundWorker;
                if (bg.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                var item = new FileItem(result, Name);
                if (result.IsFile)
                {
                    var ico = Icon.ExtractAssociatedIcon(result.FullName);
                    var bmp = ico.ToBitmap();
                    bmp.MakeTransparent();
                    item.SetIcon(bmp);
                }
                else if (result.IsFolder)
                {
                    item.SetIcon(_dirIcon);
                }
                list.Add(item);
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, list);
        }
        
    }
}
