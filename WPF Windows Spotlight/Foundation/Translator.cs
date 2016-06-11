using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using HtmlAgilityPack;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Translator : BaseFoundation
    {
        private string _word;
        private readonly string _url;
        private readonly string _xpath;
        private readonly Bitmap _icon;

        public Translator(string name = "") : base(name)
        {
            _url = "https://tw.dictionary.search.yahoo.com/search?p=";
            _xpath = "//div[contains(@class, 'dd algo explain mt-20 lst DictionaryResults')]";
            _icon = (Bitmap)WPF_Windows_Spotlight.Properties.Resources.dictionary;
        }

        public string Word
        {
            set { _word = value; }
        }

        public override void SetKeyword(string keyword)
        {
            _word = keyword;
        }

        public string Translate()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + _word);
            request.Accept = "text/html";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string html = stream.ReadToEnd();
                HtmlDocument dom = new HtmlDocument();
                dom.LoadHtml(html);
                string content;
                if ((content = GetContent(dom)) == "Not Found")
                    return content;
                
                return GenerateHtml(content, GetLinks(dom));
            }
        }

        public static string GenerateHtml(string body, string head)
        {
            string result = "<html>"
                + "<head>"
                + "<meta http-equiv='content-type' content='text/html; charset=UTF-8'>"
                + head
                + "<style>html, body { background: #F0F0F0}</style>"
                + "</head><body>"
                + body
                + "</body></html>";
            return result;
        }

        private string GetLinks(HtmlDocument dom)
        {
            var xpath = "//head/link[@rel='stylesheet']";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            return node.OuterHtml;
        }

        private string GetContent(HtmlDocument dom)
        {
            var xpath = "//div[contains(@class, 'dd algo explain mt-20 lst DictionaryResults')]";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            if (node == null)
                return "Not Found";
            return node.InnerHtml;
        }

        public override void DoWork(object sender, DoWorkEventArgs e)
        {
            var list = new List<Item>();
            var result = Translate();
            if (result != "Not Found")
            {
                var weight = 40 - (_word.Split(' ').Length * 2);
                var item = new TranslateItem(_word, _url + _word, result, Name, weight);
                item.SetIcon(_icon);
                list.Add(item);
            }
            var bg = sender as BackgroundWorker;
            if (bg.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, list);
        }
    }
}
