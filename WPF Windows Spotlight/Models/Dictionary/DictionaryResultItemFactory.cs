using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.Dictionary
{
    class DictionaryResultItemFactory : IResultItemsFactory
    {
        public List<IResultItem> CreateResultItems(object result)
        {
            var sections = (KeyValuePair<string, List<ExplanationSection>>)result;
            List<IResultItem> results = new List<IResultItem>();
            DictionaryResultItem resultItem = new DictionaryResultItem(sections.Key, sections.Value);
            results.Add(resultItem);
            return results;
        }
    }
}
