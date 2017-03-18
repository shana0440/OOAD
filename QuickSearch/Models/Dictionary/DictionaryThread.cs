using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using QuickSearch.Models.ResultItemsFactory;

namespace QuickSearch.Models.Dictionary
{
    public class DictionaryThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            try
            {
                Dictionary directory = new Dictionary();
                Definition section = directory.Search(keyword);
                DictionaryResultItemFactory factory = new DictionaryResultItemFactory();
                var DTO = new KeyValuePair<string, Definition>(keyword, section);
                List<IResultItem> results = factory.CreateResultItems(DTO);
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
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine(exception.Message);
                e.Result = null;
            }
        }
    }
}
