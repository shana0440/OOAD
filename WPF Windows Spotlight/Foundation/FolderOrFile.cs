﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FolderOrFile
    {
        private FileInfo _file;
        private DirectoryInfo _folder;

        public FolderOrFile(FileInfo file)
        {
            _file = file;
        }

        public FolderOrFile(DirectoryInfo dir)
        {
            _folder = dir;
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
    }
}
