using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.Dictionary
{
    public class Dictionary
    {
        const string _directoryUrl = "https://tw.dictionary.search.yahoo.com/search?p=";

        public List<ExplanationSection> Search(string keyword)
        {
            string url = String.Format("{0}{1}", _directoryUrl, keyword);
            string html = Crawler.GetResponse(url);
            return ParseHTML(html);
        }

        List<ExplanationSection> ParseHTML(string html)
        {
            HtmlDocument dom = new HtmlDocument();
            dom.LoadHtml(html);
            List<ExplanationSection> result = new List<ExplanationSection>();
            string xpath = "//div[contains(@class, 'dd algo explain mt-20 lst DictionaryResults')]";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            if (node == null)
            {
                throw new ArgumentException("沒有找到符合的結果");
            }
            while (node.SelectSingleNode("div[contains(@class, 'compTitle mb-10')]") != null)
            {
                var part = node.SelectSingleNode("div[contains(@class, 'compTitle mb-10')]");
                var defineCollection = node.SelectSingleNode("ul[contains(@class, 'compArticleList mb-15 ml-10')]");
                var defines = defineCollection.SelectNodes("li");
                var interpretations = new List<Explanation>();
                foreach (var define in defines)
                {
                    var defined = define.SelectSingleNode("h4");
                    var example = define.SelectSingleNode("span");
                    var exampleText = (example == null) ? "" : example.InnerText.Trim();
                    var combination = new Explanation() { Interpretation = defined.InnerText.Trim(), Example = exampleText };
                    interpretations.Add(combination);
                }
                var section = new ExplanationSection() { PartOfSpeech = part.InnerText.Trim(), Interpretations = interpretations };
                node.RemoveChild(part);
                node.RemoveChild(defineCollection);
                result.Add(section);
            };
            return result;
        }

    }
}
