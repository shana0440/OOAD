using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Models.Dictionary
{
    public class DictionaryThread : IThread
    {
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            string keyword = (string)e.Argument;
            try
            {
                Dictionary directory = new Dictionary();
                List<ExplanationSection> sections = directory.Search(keyword);
                DictionaryResultItemFactory factory = new DictionaryResultItemFactory();
                var DTO = new KeyValuePair<string, List<ExplanationSection>>(keyword, sections);
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
        }
    }
}
