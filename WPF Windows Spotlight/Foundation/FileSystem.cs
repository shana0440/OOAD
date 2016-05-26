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
    public class FileSystem : IFoundation
    {
        private string _keyword;
        private readonly Bitmap _dirIcon;

        public FileSystem(string keyword = "")
        {
            _keyword = keyword;
            _dirIcon = (Bitmap)WPF_Windows_Spotlight.Properties.Resources.folder_icon;
            _dirIcon.MakeTransparent();
        }

        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public List<FolderOrFile> Search(string keyword = "")
        {
            if (keyword == "") keyword = _keyword;
            if (keyword == "" || keyword.Length < 3) return new List<FolderOrFile>();
            Everything.Everything_SetSearchW(keyword);
            Everything.Everything_SetMax(150);
            Everything.Everything_QueryW(true);
            Everything.Everything_SortResultsByPath();
            var list = GetResult();
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
                    if (Everything.Everything_IsFolderResult(i))
                    {
                        var folderOrFile = new FolderOrFile(new DirectoryInfo(buf.ToString()));
                        if (folderOrFile.Exists) list.Add(folderOrFile);
                    }
                    else if (Everything.Everything_IsFileResult(i))
                    {
                        var folderOrFile = new FolderOrFile(new FileInfo(buf.ToString()));
                        if (folderOrFile.Exists) list.Add(folderOrFile);
                    } 
                }
                catch (ArgumentException e)
                {
                    //Console.WriteLine(e.Message);
                }
            }
            return list;
        }

        public void DoWork(object sender, DoWorkEventArgs e)
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
                var item = new FileItem(result);
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
