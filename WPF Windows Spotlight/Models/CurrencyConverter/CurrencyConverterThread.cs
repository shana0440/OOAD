using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models.CurrencyConverter
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
            catch (Exception exception)
            {
                e.Result = null;
            }
        }
    }
}
