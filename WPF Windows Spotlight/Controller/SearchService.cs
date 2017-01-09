using System.Collections.Generic;
using System.ComponentModel;
using WPF_Windows_Spotlight.Models.Calculator;
using WPF_Windows_Spotlight.Models;
using WPF_Windows_Spotlight.Models.ResultItemsFactory;
using System.Collections.ObjectModel;
using WPF_Windows_Spotlight.Models.FileSystem;
using System;
using WPF_Windows_Spotlight.Models.Dictionary;

namespace WPF_Windows_Spotlight.Controller
{
    public class SearchService
    {
        int _searchingCount = 0;
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        public delegate void SearchOverEventHandler(ObservableCollection<IResultItem> results);
        SearchOverEventHandler _searchOverEvent;
        ObservableCollection<IResultItem> _resultList = new ObservableCollection<IResultItem>();
        int _serialNumber = 0;

        public ObservableCollection<IResultItem> ResultList
        {
            get
            {
                return _resultList;
            }
        }

        public void Search(string keyword)
        {
            CancelCurrentSearching();
            _serialNumber = _serialNumber + 1 % 1000;

            //IThread calculatorThread = new CalculatorThread();
            //MyBackgroundWorker calculatorWorker = new MyBackgroundWorker(_serialNumber);
            //calculatorWorker.DoWork += new DoWorkEventHandler(calculatorThread.DoWork);
            //calculatorWorker.RunWorkerCompleted += SearchOver;
            //calculatorWorker.WorkerSupportsCancellation = true; // support cancel
            //calculatorWorker.RunWorkerAsync(keyword);
            //_searchingCount++;
            //_workers.Add(calculatorWorker);

            //IThread fileSystemThread = new FileSystemThread();
            //MyBackgroundWorker fileSystemWorker = new MyBackgroundWorker(_serialNumber);
            //fileSystemWorker.DoWork += new DoWorkEventHandler(fileSystemThread.DoWork);
            //fileSystemWorker.RunWorkerCompleted += SearchOver;
            //fileSystemWorker.WorkerSupportsCancellation = true;
            //fileSystemWorker.RunWorkerAsync(keyword);
            //_searchingCount++;
            //_workers.Add(fileSystemWorker);

            IThread directoryThread = new DictionaryThread();
            MyBackgroundWorker directoryWorker = new MyBackgroundWorker(_serialNumber);
            directoryWorker.DoWork += new DoWorkEventHandler(directoryThread.DoWork);
            directoryWorker.RunWorkerCompleted += SearchOver;
            directoryWorker.WorkerSupportsCancellation = true;
            directoryWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(directoryWorker);

            Console.WriteLine("目前有 {0} 個 worker 在運作中", _searchingCount);
        }

        public void CancelCurrentSearching()
        {
            foreach (var worker in _workers)
            {
                worker.CancelAsync();
            }
            _searchingCount = 0;
            _resultList.Clear();
            _workers.Clear();
        }

        void SearchOver(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (MyBackgroundWorker)sender;
            var isCurrentSearch = worker.SerialNumber == _serialNumber;
            if (isCurrentSearch)
            {
                _searchingCount--;
                Console.WriteLine("目前有 {0} 個 worker 在運作中", _searchingCount);
                if (!e.Cancelled && e.Result != null)
                {
                    List<IResultItem> result = (List<IResultItem>)e.Result;
                    MergeListToObservableCollection(_resultList, result);
                }

                if (_searchingCount == 0)
                {
                    Console.WriteLine("總共搜尋到 {0} 筆資料", _resultList.Count);
                    _workers.Clear();
                    if (_resultList.Count > 0)
                    {
                        SortBestResult(_resultList);
                    }
                    _searchOverEvent?.Invoke(_resultList);
                }
            }
        }

        ObservableCollection<IResultItem> MergeListToObservableCollection(ObservableCollection<IResultItem> target, List<IResultItem> source)
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

        void SortBestResult(ObservableCollection<IResultItem> results)
        {
            IResultItem bestSolution = null;
            foreach (var item in results)
            {
                if (bestSolution == null || bestSolution.Priority < item.Priority)
                {
                    bestSolution = item;
                }
            }
            bestSolution.GroupName = "最佳搜尋結果";
            results.Remove(bestSolution);
            results.Insert(0, bestSolution);
        }

    }
}
