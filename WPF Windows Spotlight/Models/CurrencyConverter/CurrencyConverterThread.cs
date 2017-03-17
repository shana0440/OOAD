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
                if (originCurrency.Length >= 4)
                {
                    CurrencyConverter converter = new CurrencyConverter();
                    string currency = originCurrency.Substring(originCurrency.Length - 3, 3);
                    string amount = originCurrency.Substring(0, originCurrency.Length - 3);
                    string convertedCurrency = converter.Convert(amount, currency);
                    CurrencyConverterResultItemFactory factory = new CurrencyConverterResultItemFactory();
                    ItemData item = new ItemData() { Title = originCurrency, Content = convertedCurrency };
                    e.Result = factory.CreateResultItems(item);
                }
                else
                {
                    e.Result = null;
                }
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
