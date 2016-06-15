using System.Collections.Generic;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security;
using HtmlAgilityPack;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Translator : BaseFoundation
    {
        private string _word;
        private readonly string _url = "https://tw.dictionary.search.yahoo.com/search?p=";
        private readonly Bitmap _icon;

        public Translator(string name = "") : base(name)
        {
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

        public List<KeyValuePair<string, List<KeyValuePair<string, string>>>> Translate()
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
                var content = GetContent(dom);
                return content;
            }
        }

        private List<KeyValuePair<string, List<KeyValuePair<string, string>>>> GetContent(HtmlDocument dom)
        {
            var result = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>>();
            var xpath = "//div[contains(@class, 'dd algo explain mt-20 lst DictionaryResults')]";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            if (node != null)
            {
                while (node.SelectSingleNode("div[contains(@class, 'compTitle mb-10')]") != null)
                {
                    var part = node.SelectSingleNode("div[contains(@class, 'compTitle mb-10')]");
                    var defineCollection = node.SelectSingleNode("ul[contains(@class, 'compArticleList mb-15 ml-10')]");
                    var defines = defineCollection.SelectNodes("li");
                    var defineList = new List<KeyValuePair<string, string>>();
                    foreach (var define in defines)
                    {
                        var defined = define.SelectSingleNode("h4");
                        var example = define.SelectSingleNode("span");
                        var exampleText = (example == null) ? "" : example.InnerText.Trim();
                        var combination = new KeyValuePair<string, string>(defined.InnerText.Trim(),
                            exampleText);
                        defineList.Add(combination);
                    }
                    var section = new KeyValuePair<string, List<KeyValuePair<string, string>>>(part.InnerText.Trim(),
                        defineList);
                    node.RemoveChild(part);
                    node.RemoveChild(defineCollection);
                    result.Add(section);
                };
            }
            return result;
        }

        public override void DoWork(object sender, DoWorkEventArgs e)
        {
            var list = new List<Item>();
            var result = Translate();
            if (result.Count > 0)
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
