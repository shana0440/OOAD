using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    class CalculatorResultItemsFactory : IResultItemsFactory
    {
        public List<IResultItem> CreateResultItems(ItemData item)
        {
            List<IResultItem> resultItems = new List<IResultItem>();
            ResultItem resultItem = new CalculatorResultItem(item.Title, item.Content);
            resultItems.Add(resultItem);
            return resultItems;
        }
    }
}
