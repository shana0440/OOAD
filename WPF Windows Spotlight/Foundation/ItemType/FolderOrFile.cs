using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FolderOrFile
    {
        private readonly FileInfo _file;
        private readonly DirectoryInfo _folder;
        private readonly int _count;

        public FolderOrFile(string path, int count = 0)
        {
            if (Directory.Exists(path))
            {
                _folder = new DirectoryInfo(path);
            }
            else if (File.Exists(path))
            {
                _file = new FileInfo(path);
            }
            _count = count;
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsAvailable
        {
            get { return _file != null || _folder != null; }
        }

        public string FullName
        {
            get
            {
                return (_file != null) ? _file.FullName : _folder.FullName;
            }
        }

        public string Name
        {
            get
            {
                return (_file != null) ? _file.Name : _folder.Name;
            }
        }

        public bool Exists
        {
            get
            {
                return (_file != null) ? _file.Exists : _folder.Exists;
            }
        }

        public bool IsFile
        {
            get { return _file != null; }
        }

        public bool IsFolder
        {
            get { return _folder != null; }
        }

        public string CreationDate
        {
            get { return (_file != null) ? _file.CreationTime.ToString("d") : _folder.CreationTime.ToString("d"); }
        }

        public string LastWriteDate
        {
            get { return (_file != null) ? _file.LastWriteTime.ToString("d") : _folder.LastWriteTime.ToString("d"); }
        }

        public string LastAccessDate
        {
            get { return (_file != null) ? _file.LastAccessTime.ToString("d") : _folder.LastAccessTime.ToString("d"); }
        }

        public System.Drawing.Bitmap GetIcon()
        {
            if (IsFile)
            {
                var ico = Icon.ExtractAssociatedIcon(FullName);
                var bmp = ico.ToBitmap();
                bmp.MakeTransparent();
                return bmp;
            }
            else if (IsFolder)
            {
                return (Bitmap) WPF_Windows_Spotlight.Properties.Resources.folder_icon;
            }
            return null;
        }
    }
}
