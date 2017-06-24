using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models;
using System.Text.RegularExpressions;

namespace QuickSearch.Models.CurrencyConverter
{
    public class CurrencyConverter
    {

        public string Convert(string originCurrency)
        {
            if (!Regex.IsMatch(originCurrency, @"^\d+[A-Za-z]{3}$")) throw new ArgumentException("貨幣輸入不合法");
            string currency = originCurrency.Substring(originCurrency.Length - 3, 3);
            string amount = originCurrency.Substring(0, originCurrency.Length - 3);
            string url = String.Format(Config.CurrencyConvertUrl, amount, currency.ToUpper());
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
