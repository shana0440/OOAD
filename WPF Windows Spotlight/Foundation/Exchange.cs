using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Exchange : BaseFoundation
    {
        private string _currency;
        private string _url = "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm";
        private readonly Bitmap _icon;
        private HtmlDocument _dom;

        public Exchange(string name = "") : base(name)
        {
            _icon = (Bitmap) WPF_Windows_Spotlight.Properties.Resources.exchange;
        }

        public override void SetKeyword(string keyword)
        {
            _currency = keyword;
            _dom = GetExchangeDocument();
        }

        private HtmlDocument GetExchangeDocument()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Accept = "text/html";
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string html = stream.ReadToEnd();
                HtmlDocument dom = new HtmlDocument();
                dom.LoadHtml(html);
                return dom;
            }
        }

        
        public string ExchangeCurrency(string currency)
        {
            if (currency.Length < 4) return "";
            _dom = _dom ?? GetExchangeDocument();
            var rows = GetExchangeRows(_dom);
            var convertCurrency = currency.ToUpper().Substring(currency.Length - 3, 3);
            var value = currency.Substring(0, currency.Length - 3);
            foreach (var row in rows)
            {
                var currencyName = GetCurrencyName(row);
                var sell = GetSell(row);
                var buy = GetBuy(row);
                if (convertCurrency == currencyName)
                {
                    var result = Double.Parse(value) * Double.Parse(sell);
                    return result.ToString();
                }
            }
            return "";
        }

        private string GetBuy(HtmlNode row)
        {
            var buy = row.ChildNodes[1].InnerText;
            return buy;
        }

        private string GetSell(HtmlNode row)
        {
            var sell = row.ChildNodes[2].InnerText;
            return sell;
        }

        private string GetCurrencyName(HtmlNode row)
        {
            var currency = row.FirstChild.InnerText;
            string pattern = @"\((\w+)\)";
            Match m = Regex.Match(currency, pattern);
            return m.Value.Substring(1, m.Value.Length - 2);
        }

        private HtmlNodeCollection GetExchangeRows(HtmlDocument dom)
        {
            var xpath = "//tr[contains(@class, 'color')]";
            var nodes = dom.DocumentNode.SelectNodes(xpath);
            return nodes;
        }

        public override void DoWork(object sender, DoWorkEventArgs e)
        {
            var list = new List<Item>();
            var result = ExchangeCurrency(_currency);
            var bg = sender as BackgroundWorker;
            if (bg.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            if (result != "")
            {
                var item = new ExchangeItem(result, _currency, Name, 100);
                item.SetIcon(_icon);
                list.Add(item);
            }
            e.Result = new KeyValuePair<string, List<Item>>((string)e.Argument, list);
        }
    }
}
