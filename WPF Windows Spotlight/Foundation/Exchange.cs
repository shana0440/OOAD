using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Web;

namespace WPF_Windows_Spotlight.Foundation
{
    public class Exchange : IFoundation
    {
        private string _currency;
        private string _url = "http://rate.bot.com.tw/Pages/Static/UIP003.zh-TW.htm";
        private string _xpath = "//table[@title=\"牌告匯率\"]";

        public Exchange(string currency = "")
        {
            _currency = currency;
        }

        public string Currency
        {
            set { _currency = value; }
        }

        private HtmlNode GetTable()
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
                HtmlNode node = dom.DocumentNode.SelectSingleNode(_xpath);
                return node;
            }
        }

        /**
         * DOTO 用正規表達式將currency分成數字以及幣別兩部分
         * 透過GetTable取得臺灣銀行公告的幣別匯率
         * 遊歷所有幣別直到找到使用者輸入的幣別
         * 將匯率換算成台幣
         */
        private string ExchangeCurrency(string currency)
        {
            HtmlNode table = GetTable();
            for (int i = 5; i < table.ChildNodes.Count; i+=2)
            {
                HtmlNode row = table.ChildNodes[i];
            }
            return "";
        }

        public string GetResult()
        {
            return ExchangeCurrency(_currency);
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
