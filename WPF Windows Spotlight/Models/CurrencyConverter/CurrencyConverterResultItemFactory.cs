using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.CurrencyConverter
{
    class CurrencyConverterResultItemFactory : IResultItemsFactory
    {
        public List<IResultItem> CreateResultItems(object result)
        {
            ItemData item = (ItemData)result;
            List<IResultItem> resultItems = new List<IResultItem>();
            ResultItem resultItem = new CurrencyConverterResultItem(item.Title, item.Content);
            resultItems.Add(resultItem);
            return resultItems;
        }
    }
}
