using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.Calculator
{
    class CalculatorResultItemsFactory : IResultItemsFactory
    {
        public List<IResultItem> CreateResultItems(object result)
        {
            var item = (ItemData)result;
            List<IResultItem> resultItems = new List<IResultItem>();
            ResultItem resultItem = new CalculatorResultItem(item.Title, item.Content);
            resultItems.Add(resultItem);
            return resultItems;
        }
    }
}
