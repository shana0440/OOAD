using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    interface IResultItemsFactory
    {
        List<IResultItem> CreateResultItems(ItemData item);
    }
}
