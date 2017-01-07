using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.ResultItemsFactory
{
    class CalculatorResultItem : ResultItem
    {
        string _expression;

        public CalculatorResultItem(string answer, string expression)
        {
            GroupName = "計算機";
            Weight = 100;
            Title = answer;
            _expression = expression;
        }

        public override void GenerateContent()
        {
            throw new NotImplementedException();
        }

        public override void OpenResource()
        {
            throw new NotImplementedException();
        }
    }
}
