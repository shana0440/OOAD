using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using HtmlAgilityPack;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Translator : IFoundation
    {
        private string _word;
        private string _url;
        private string _xpath;

        public Translator(string word = "")
        {
            _word = word;
            _url = "https://tw.dictionary.search.yahoo.com/search?p=";
            _xpath = "//div[contains(@class, 'dd algo explain mt-20 lst DictionaryResults')]";
        }

        public string Word
        {
            set { _word = value; }
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
                HtmlNode node = dom.DocumentNode.SelectSingleNode(_xpath);
                string result = node.InnerHtml;
                return result;
            }
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Translate();
        }
    }
}
