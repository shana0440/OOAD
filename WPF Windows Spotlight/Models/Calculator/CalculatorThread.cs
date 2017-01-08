using System;
using System.Collections.Generic;
using System.ComponentModel;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.Calculator
{
    public class CalculatorThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var expression = (string)e.Argument;
            Calculator calculator = new Calculator();
            try
            {
                var answer = calculator.Execute(expression);
                ItemData item = new ItemData() { Title = answer, Content = expression };
                IResultItemsFactory factory = new CalculatorResultItemsFactory();
                List<IResultItem> list = factory.CreateResultItems(item);
                e.Result = list;
            }
            catch (Exception)
            {
                e.Cancel = true;
            }

        }

    }
}
