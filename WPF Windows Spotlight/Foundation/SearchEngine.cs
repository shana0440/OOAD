using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class SearchEngine : IFoundation
    {
        private string _keyword;
        private readonly string _url;
        private readonly Bitmap _icon;
        private readonly string _name;

        public SearchEngine(string name = "")
        {
            _name = name;
            _url = "https://www.google.com.tw/search?q=";
            _icon = (Bitmap)WPF_Windows_Spotlight.Properties.Resources.web;
        }

        public string Name
        {
            get { return _name; }
        }

        public List<Item> Search()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + _keyword);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            request.Headers.Set("accept-language", "zh-TW,zh;q=0.8,en-US;q=0.6,en;q=0.4");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string html = stream.ReadToEnd();
                HtmlDocument dom = new HtmlDocument();
                dom.LoadHtml(html);
                return GetWebSite(dom, 5);
            }

            return null;
        }

        private List<Item> GetWebSite(HtmlDocument dom, int count)
        {
            var result = new List<Item>();

            var titles = GetTitles(dom);
            if (titles == null)
                return result;
            var intros = GetIntros(dom);
            var urls = GetUrls(dom);

            for (int i = 0; i < count; i++)
            {
                var title = WebUtility.HtmlDecode(titles[i].InnerText);
                var intro = WebUtility.HtmlDecode(intros[i].InnerText);
                var url = WebUtility.HtmlDecode(urls[i].InnerText);
                var web = new WebSite(title, intro, url, _name);
                web.SetIcon(_icon);
                result.Add(web);
            }
            return result;
        }

        private HtmlNodeCollection GetUrls(HtmlDocument dom)
        {
            var xpath = "//div[contains(@class, 'kv')]/cite";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }

        private HtmlNodeCollection GetIntros(HtmlDocument dom)
        {
            var xpath = "//span[@class='st']";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }

        private HtmlNodeCollection GetTitles(HtmlDocument dom)
        {
            var xpath = "//div[@class='s']/preceding-sibling::h3[@class='r']";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }
        
        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var results = Search();
            var bg = sender as BackgroundWorker;
            if (bg.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, results);
        }

    }
}
