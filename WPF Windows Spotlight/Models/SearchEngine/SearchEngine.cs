using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.SearchEngine
{
    public class SearchEngine
    {
        string _url = "https://www.google.com.tw/search?q={0}";
        int _searchResultCount = 5;
        
        public List<IResultItem> Search(string keyword)
        {
            string url = String.Format(_url, keyword);
            string html = Crawler.GetResponse(url);
            return ParseHTML(html);
        }

        public List<IResultItem> ParseHTML(string html)
        {
            List<IResultItem> results = new List<IResultItem>();
            HtmlDocument dom = new HtmlDocument();
            dom.LoadHtml(html);

            var titles = GetTitles(dom);
            if (titles == null)
            {
                throw new ArgumentException("找不到符合搜尋字詞的網頁");
            }
            var intros = GetIntros(dom);
            var urls = GetUrls(dom);

            for (int i = 0; i < _searchResultCount; i++)
            {
                if (i >= titles.Count) break;
                var title = WebUtility.HtmlDecode(titles[i].InnerText);
                var intro = WebUtility.HtmlDecode(intros[i].InnerText);
                var url = WebUtility.HtmlDecode(urls[i].InnerText);
                SearchEngineResultItem item = new SearchEngineResultItem(title, url, intro);
                results.Add(item);
            }

            return results;
        }

        HtmlNodeCollection GetTitles(HtmlDocument dom)
        {
            var xpath = "//div[@class='s']/preceding-sibling::h3[@class='r']";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }

        HtmlNodeCollection GetIntros(HtmlDocument dom)
        {
            var xpath = "//span[@class='st']";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }

        HtmlNodeCollection GetUrls(HtmlDocument dom)
        {
            var xpath = "//div[@class='s']/div/div[contains(@class, 'kv')]/cite";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }
    }
}
