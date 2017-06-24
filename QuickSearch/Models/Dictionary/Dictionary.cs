using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuickSearch.Models.Dictionary
{
    public class Dictionary
    {

        public Definition Search(string keyword)
        {
            if (!Regex.IsMatch(keyword, @"^[A-Za-z]*$")) throw new ArgumentException("輸入除了英文以外的資料");
            string url = String.Format(Config.DirectoryUrl, Uri.EscapeDataString(keyword));
            string html = Crawler.GetResponse(url);
            return ParseHTML(html);
        }

        Definition ParseHTML(string html)
        {
            HtmlDocument dom = new HtmlDocument();
            dom.LoadHtml(html);
            string xpath = "//div[@id='definition']";
            HtmlNode node = dom.DocumentNode.SelectSingleNode(xpath);
            if (node == null)
            {
                throw new ArgumentException("沒有找到符合的結果");
            }

            Definition definition = new Definition();
            var content = node.InnerHtml.Replace("\n", "").Trim();
            content = Regex.Replace(content, @"</?a.*?>", "");
            const string explanationsPattern = @"(</h3>(?<explanations>.*?)<h3>)";
            Match explanationsMatch = Regex.Match(content, explanationsPattern, RegexOptions.IgnoreCase);
            var explanations = Regex.Split(explanationsMatch.Groups["explanations"].Value, @"<br><br>");

            const string examplePattern = @"(</h3>(?<examples>.*?)$)";
            Match exampleMatch = Regex.Match(content, examplePattern, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            var examples = Regex.Split(exampleMatch.Groups["examples"].Value, @"<br><br>");

            foreach (var explanation in explanations)
            {
                if (explanation == "") break;
                definition.explanations.Add(explanation);
            }

            foreach (var example in examples)
            {
                var sentences = Regex.Split(example, "<br>");
                if (sentences.Length != 2) break;
                definition.examples.Add(new Example { origin = sentences[0], translated = sentences[1] });
            }

            return definition;
        }

    }
}
