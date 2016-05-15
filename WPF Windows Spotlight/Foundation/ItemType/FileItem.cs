using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight.Foundation.ItemType
{
    public class FileItem : Item
    {
        private FolderOrFile _folderOrFile;

        public FileItem(FolderOrFile folderOrFile)
            : base(folderOrFile.Name)
        {
            _folderOrFile = folderOrFile;
        }

        public string FullName
        {
            get { return _folderOrFile.FullName; }
        }

        public List<KeyValuePair<string, string>> GetProperty()
        {
            List<KeyValuePair<string, string>> propertys = new List<KeyValuePair<string, string>>();
            propertys.Add(new KeyValuePair<string, string>("Created", _folderOrFile.CreationDate));
            propertys.Add(new KeyValuePair<string, string>("Modified", _folderOrFile.LastWriteDate));
            propertys.Add(new KeyValuePair<string, string>("Accessed", _folderOrFile.LastAccessDate));
            return propertys;
        }

        public override void Open()
        {
            if (_folderOrFile != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(_folderOrFile.FullName);
                }
                catch (Win32Exception e)
                {
                    throw new Exception("Can't open this file or folder");
                }
            }   
        }
    }
}
