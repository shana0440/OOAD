using System.Collections.Generic;
using System.ComponentModel;
using WPF_Windows_Spotlight.Models.Calculator;
using WPF_Windows_Spotlight.Models;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using System.Collections.ObjectModel;
using WPF_Windows_Spotlight.Models.FileSystem;

namespace WPF_Windows_Spotlight.Controller
{
    public class SearchService
    {
        int _searchingCount = 0;
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        public delegate void SearchOverEventHandler(ObservableCollection<IResultItem> results);
        SearchOverEventHandler _searchOverEvent;
        ObservableCollection<IResultItem> _resultList = new ObservableCollection<IResultItem>();

        public ObservableCollection<IResultItem> ResultList
        {
            get
            {
                return _resultList;
            }
        }

        public void Search(string keyword)
        {
            _resultList.Clear();
            _searchingCount = 0;

            //IThread calculatorThread = new CalculatorThread();
            //BackgroundWorker calculatorWorker = new BackgroundWorker();
            //calculatorWorker.DoWork += new DoWorkEventHandler(calculatorThread.DoWork);
            //calculatorWorker.RunWorkerCompleted += SearchOver;
            //calculatorWorker.WorkerSupportsCancellation = true; // support cancel
            //calculatorWorker.RunWorkerAsync(keyword);
            //_searchingCount++;
            //_workers.Add(calculatorWorker);

            IThread fileSystemThread = new FileSystemThread();
            BackgroundWorker fileSystemWorker = new BackgroundWorker();
            fileSystemWorker.DoWork += new DoWorkEventHandler(fileSystemThread.DoWork);
            fileSystemWorker.RunWorkerCompleted += SearchOver;
            fileSystemWorker.WorkerSupportsCancellation = true;
            fileSystemWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(fileSystemWorker);
        }

        void SearchOver(object sender, RunWorkerCompletedEventArgs e)
        {
            // notify view, if search is over;
            _searchingCount--;
            if (!e.Cancelled && e.Result != null)
            {
                List<IResultItem> result = (List<IResultItem>)e.Result;
                AddRange(_resultList, result);
                if (_searchingCount == 0)
                {
                    _workers.Clear();
                    if (_searchOverEvent != null)
                    {
                        _searchOverEvent(_resultList);
                    }
                }
            }
        }

        ObservableCollection<IResultItem> AddRange(ObservableCollection<IResultItem> target, List<IResultItem> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
            return target;
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
            _searchingCount = 0;
            _resultList.Clear();
            _workers.Clear();
        }
    }
}
