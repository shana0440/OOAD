using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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

        public List<Item> Search(string keyword = "")
        {
            if (keyword == "" && _keyword != null) keyword = _keyword;
            if (keyword == "" || keyword.Length < 3) return new List<Item>();
            Everything.Everything_SetSearchW(keyword);
            Everything.Everything_SetMax(SearchMaxCount);
            Everything.Everything_QueryW(true);
            Everything.Everything_SortResultsByPath();
            var history = SearchOnHistory(keyword);
            var list = GetResult();
            foreach (var item in history)
            {
                var result = list.Find(target => target.Title == item.Title);
                list.Remove(result);
            }
            list = Sort(list);
            history.AddRange(list);
            Everything.Everything_Reset();
            return history;
        }

        private List<Item> GetResult()
        {
            var list = new List<Item>();
            const int bufsize = 260;
            var buf = new StringBuilder(260);

            for (var i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, buf, bufsize);
                try
                {
                    var folderOrFile = new FolderOrFile(buf.ToString());
                    if (folderOrFile.IsAvailable)
                    {
                        var weight = 50 - (folderOrFile.Name.Length - _keyword.Length);
                        var item = new FileItem(folderOrFile, Name, weight);
                        item.SetIcon(folderOrFile.GetIcon());
                        list.Add(item);
                    }
                }
                catch (ArgumentException e)
                {
                    //Console.WriteLine(e.Message);
                }
            }
            return list;
        }

        private List<Item> SearchOnHistory(string keyword)
        {
            var filePriority = new FilePriority();
            var files = filePriority.InPriorityFile(keyword);
            var result = new List<Item>();
            foreach (var file in files)
            {
                var weight = 50 - (file.Name.Length - _keyword.Length) + file.Count;
                var item = new FileItem(file, Name, weight);
                item.SetIcon(file.GetIcon());
                result.Add(item);
            }
            return result;
        }

        private List<Item> Sort(IEnumerable<Item> originList)
        {
            var sortedLists = new List<List<Item>>();
            // 定義n個要搜尋的條件，第n+1個放不存在於條件內的
            for (var i = 0; i < _sortOrder.Length + 1; i++)
            {
                sortedLists.Add(new List<Item>());
            }
            foreach (var item in originList)
            {
                var file = (FileItem) item;
                var added = false;
                for (var j = 0; j < _sortOrder.Length; j++)
                {
                    if (file.Name.EndsWith(_sortOrder[j]))
                    {
                        sortedLists[j].Add(file);
                        added = true;
                    }
                }
                if (!added)
                    sortedLists[_sortOrder.Length].Add(file);
            }
            var result = new List<Item>();
            foreach (var sortedList in sortedLists)
            {
                result.AddRange(sortedList.OrderBy(item => ((FileItem)item).Name));
            }
            return result;
        }

        public override void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            var bg = sender as BackgroundWorker;
            if (bg.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, results);
        }
        
    }
}
