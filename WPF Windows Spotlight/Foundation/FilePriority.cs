using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xml;
using System.Xml.Resolvers;

namespace WPF_Windows_Spotlight.Foundation
{
    public class FilePriority
    {
        private const string PriorityFile = "filePriority.xml";

        public FilePriority()
        {
            
        }

        public int PriorityUp(string fileName)
        {
            fileName = fileName.ToLower();
            var xml = new XmlDocument();
            var openCount = 1;
            if (File.Exists(PriorityFile))
            {
                xml.Load(PriorityFile);
                var node = xml.SelectSingleNode("History/File[@Name='"+ fileName +"']");
                if (node != null)
                {
                    var count = Int32.Parse(node.Attributes["Count"].InnerText) + 1;
                    node.Attributes["Count"].InnerText = count.ToString();
                    openCount = count;
                }
                else
                {
                    var history = xml.SelectSingleNode("History");
                    history.AppendChild(CreateFileXml(xml, fileName));
                    xml.AppendChild(history);
                }
            }
            else
            {
                var history = xml.CreateElement("History");
                history.AppendChild(CreateFileXml(xml, fileName));
                xml.AppendChild(history);
            }
            xml.Save(PriorityFile);
            return openCount;
        }

        private XmlNode CreateFileXml(XmlDocument xml, string fileName)
        {
            var ele = xml.CreateElement("File");
            ele.SetAttribute("Name", fileName);
            ele.SetAttribute("Count", "1");
            return ele;
        }
    }

    public class FileData
    {
        public string FullName { get; set; }
        public int Count { get; set; }
    }
}
