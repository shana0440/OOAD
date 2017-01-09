using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models;

namespace WPF_Windows_Spotlight.Models.CurrencyConverter
{
    public class CurrencyConverter
    {
        string _convertUrl = "https://www.google.com/finance/converter?a={0}&from={1}&to=TWD";

        public string Convert(string amount, string curreny)
        {
            string url = String.Format(_convertUrl, amount, curreny.ToLower());
            try
            {
                string html = Crawler.GetResponse(url);
                return ParseHTML(html);
            }
            catch(WebException exception)
            {
                throw exception;
            }
            catch (Exception)
            {
                throw new ArgumentException("沒有找到對應的貨幣");
            }
        }

        string ParseHTML(string html)
        {
            HtmlDocument dom = new HtmlDocument();
            dom.LoadHtml(html);
            string xpath = "//span[contains(@class, 'bld')]";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            var result = node.InnerText;
            return result;
        }
    }
}
