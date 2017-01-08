using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_Windows_Spotlight.Models.Calculator;
using WPF_Windows_Spotlight.Foundation.ItemType;
using WPF_Windows_Spotlight.Models;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;

namespace WPF_Windows_Spotlight.Controller
{
    public class SearchService
    {
        int _searchCount = 0;
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        public delegate void SearchOverEventHandler(List<IResultItem> results);
        SearchOverEventHandler _searchOverEvent;
        List<IResultItem> _results = new List<IResultItem>();

        public void Search(string keyword)
        {
            _searchCount = 0;
            IThread thread = new CalculatorThread();
            BackgroundWorker calculatorWorker = new BackgroundWorker();
            calculatorWorker.DoWork += new DoWorkEventHandler(thread.DoWork);
            calculatorWorker.RunWorkerCompleted += SearchOver;
            calculatorWorker.WorkerSupportsCancellation = true; // support cancel
            calculatorWorker.RunWorkerAsync(keyword);

            _searchCount++;
            _workers.Add(calculatorWorker);
        }

        void SearchOver(object sender, RunWorkerCompletedEventArgs e)
        {
            // notify view, if search is over;
            List<IResultItem> result = (List<IResultItem>)e.Result;
            _results.AddRange(result);
            _searchCount--;
            if (_searchCount == 0)
            {
                _workers.Clear();
                if (_searchOverEvent != null)
                {
                    _searchOverEvent(_results);
                }
            }
        }

        public void SubscribeSearchOverEvent(SearchOverEventHandler handler)
        {
            _searchOverEvent = handler;
        }

        public void CancelSearch()
        {
            foreach (var worker in _workers)
            {
                worker.CancelAsync();
            }
            _searchCount = 0;
            _results.Clear();
            _workers.Clear();
        }
    }
}
