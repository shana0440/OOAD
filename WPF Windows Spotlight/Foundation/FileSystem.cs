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
        private Bitmap _dirIcon;
        const int _bufsize = 260;
        StringBuilder _buf = new StringBuilder(260);

        public FileSystem(string keyword = "")
        {
            _keyword = keyword;
            _dirIcon = (Bitmap)Image.FromFile("Images/folder_icon.png");
            _dirIcon.MakeTransparent();
        }

        public string Keyword {
            set { _keyword = value; }
        }

        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public List<FolderOrFile> Search(string keyword = "")
        {
            if (keyword == "") keyword = _keyword;
            if (keyword == "") return new List<FolderOrFile>();
            
            Everything.Everything_SetSearchW(keyword);
            Everything.Everything_QueryW(true);
            return GetResult();
        }

        public List<FolderOrFile> GetResult()
        {
            List<FolderOrFile> list = new List<FolderOrFile>();
            for (int i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, _buf, _bufsize);
                FolderOrFile folderOrFile = null;
                try
                {
                    if (Everything.Everything_IsFolderResult(i))
                    {
                        folderOrFile = new FolderOrFile(new DirectoryInfo(_buf.ToString()));
                    }
                    else if (Everything.Everything_IsFileResult(i))
                    {
                        folderOrFile = new FolderOrFile(new FileInfo(_buf.ToString()));
                        // 在return type是IEnumerable的時候, yield return之後還會繼續執行
                    }
                    if (folderOrFile.Exists) list.Add(folderOrFile);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return list;
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            List<FolderOrFile> results = Search();
            List<Item> list = new List<Item>();
            foreach (FolderOrFile result in results)
            {
                Item item = new Item(result.Name);
                if (result.IsFile)
                {
                    Icon ico = Icon.ExtractAssociatedIcon(result.FullName);
                    Bitmap bmp = ico.ToBitmap();
                    bmp.MakeTransparent();
                    item.SetIcon(bmp);
                }
                else if (result.IsFolder)
                {
                    item.SetIcon(_dirIcon);
                }
                item.Content = result.FullName;
                list.Add(item);
            }
            e.Result = list;
        }
        
    }
}
