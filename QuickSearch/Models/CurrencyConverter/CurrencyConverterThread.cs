using System;
using System.ComponentModel;
using System.Net;

namespace QuickSearch.Models.CurrencyConverter
{
    public class CurrencyConverterThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            var originCurrency = (string)e.Argument;
            try
            {
                CurrencyConverter converter = new CurrencyConverter();
                string convertedCurrency = converter.Convert(originCurrency);
                CurrencyConverterResultItemFactory factory = new CurrencyConverterResultItemFactory();
                ItemData item = new ItemData() { Title = originCurrency, Content = convertedCurrency };
                e.Result = factory.CreateResultItems(item);
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
        }
    }
}
