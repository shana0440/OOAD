using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.Calculator
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
