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

        public IEnumerable<FolderOrFile> Search()
        {
            const int bufsize = 260;
            StringBuilder buf = new StringBuilder(bufsize);
            Everything.Everything_SetSearchW(_keyword);
            Everything.Everything_QueryW(true);
            for (int i = 0; i < Everything.Everything_GetNumResults(); i++)
            {
                // get the result's full path and file name.
                Everything.Everything_GetResultFullPathNameW(i, buf, bufsize);
                FolderOrFile folderOrFile = null;
                if (Everything.Everything_IsFolderResult(i))
                {
                    folderOrFile = new FolderOrFile(new DirectoryInfo(buf.ToString()));
                }
                else if (Everything.Everything_IsFileResult(i))
                {
                    folderOrFile = new FolderOrFile(new FileInfo(buf.ToString()));
                    // 在return type是IEnumerable的時候, yield return之後還會繼續執行
                }
                yield return folderOrFile;
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            List<Item> list = new List<Item>();
            foreach (FolderOrFile result in results)
            {
                if (result != null && result.Exists)
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
                    list.Add(item);
                }
            }
            e.Result = list;
        }

        
    }
}
