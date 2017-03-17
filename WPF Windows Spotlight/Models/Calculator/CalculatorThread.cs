using System;
using System.Collections.Generic;
using System.ComponentModel;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.Calculator
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
                List<IResultItem> results = factory.CreateResultItems(item);
                var worker = (BackgroundWorker)sender;
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Result = results;
                }
            }
            catch (Exception)
            {
                e.Result = null;
            }
        }
    }
}
