using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.SearchEngine
{
    public class SearchEngineThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            SearchEngine engine = new SearchEngine();
            try
            {
                List<IResultItem> results = engine.Search(keyword);
                e.Result = results;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
        }
    }
}
