using System.Collections.Generic;
using System.ComponentModel;
using QuickSearch.Models;
using System;
using System.Threading;
using QuickSearch.Models.SearchEngine;
using QuickSearch.Models.ResultItemsFactory;
using QuickSearch.Models.Calculator;
using QuickSearch.Models.FileSystem;
using QuickSearch.Models.Dictionary;
using QuickSearch.Models.CurrencyConverter;
using System.Collections.ObjectModel;

namespace QuickSearch.Controller
{
    public class SearchThread
    {
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        int _serialNumber = 0;
        int _searchingCount = 0;
        volatile bool _isPause = false;
        public List<IResultItem> ResultList = new List<IResultItem>();
        public volatile string Keyword;
        public volatile ManualResetEvent _pauseEvent = new ManualResetEvent(true); // 暫停執行續用的
        public delegate void SearchOverEventHandler();
        SearchOverEventHandler _searchOverEvent;

        public void Search()
        {
            do
            {
                _pauseEvent.WaitOne(500);
                _isPause = false;
                Thread.Sleep(500);
            } while (_isPause);

            CancelCurrentSearching();

            _serialNumber++;
            _serialNumber = _serialNumber + 1 % 1000;

            IThread calculatorThread = new CalculatorThread();
            MyBackgroundWorker calculatorWorker = new MyBackgroundWorker(_serialNumber, "計算機");
            calculatorWorker.DoWork += new DoWorkEventHandler(calculatorThread.DoWork);
            calculatorWorker.RunWorkerCompleted += SearchOver;
            calculatorWorker.WorkerSupportsCancellation = true; // support cancel
            calculatorWorker.RunWorkerAsync(Keyword);
            _searchingCount++;
            _workers.Add(calculatorWorker);

            IThread fileSystemThread = new FileSystemThread();
            MyBackgroundWorker fileSystemWorker = new MyBackgroundWorker(_serialNumber, "檔案系統");
            fileSystemWorker.DoWork += new DoWorkEventHandler(fileSystemThread.DoWork);
            fileSystemWorker.RunWorkerCompleted += SearchOver;
            fileSystemWorker.WorkerSupportsCancellation = true;
            fileSystemWorker.RunWorkerAsync(Keyword);
            _searchingCount++;
            _workers.Add(fileSystemWorker);

            IThread directoryThread = new DictionaryThread();
            MyBackgroundWorker directoryWorker = new MyBackgroundWorker(_serialNumber, "字典");
            directoryWorker.DoWork += new DoWorkEventHandler(directoryThread.DoWork);
            directoryWorker.RunWorkerCompleted += SearchOver;
            directoryWorker.WorkerSupportsCancellation = true;
            directoryWorker.RunWorkerAsync(Keyword);
            _searchingCount++;
            _workers.Add(directoryWorker);

            IThread currencyThread = new CurrencyConverterThread();
            MyBackgroundWorker currencyWorker = new MyBackgroundWorker(_serialNumber, "匯率轉換");
            currencyWorker.DoWork += new DoWorkEventHandler(currencyThread.DoWork);
            currencyWorker.RunWorkerCompleted += SearchOver;
            currencyWorker.WorkerSupportsCancellation = true;
            currencyWorker.RunWorkerAsync(Keyword);
            _searchingCount++;
            _workers.Add(currencyWorker);

            IThread searchEngineThread = new SearchEngineThread();
            MyBackgroundWorker searchEngineWorker = new MyBackgroundWorker(_serialNumber, "網頁搜尋");
            searchEngineWorker.DoWork += new DoWorkEventHandler(searchEngineThread.DoWork);
            searchEngineWorker.RunWorkerCompleted += SearchOver;
            searchEngineWorker.WorkerSupportsCancellation = true;
            searchEngineWorker.RunWorkerAsync(Keyword);
            _searchingCount++;
            _workers.Add(searchEngineWorker);
        }

        public void CancelCurrentSearching()
        {
            foreach (var worker in _workers)
            {
                worker.CancelAsync();
            }
            _searchingCount = 0;
            ResultList.Clear();
            _workers.Clear();
            _serialNumber++;
        }

        void SearchOver(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (MyBackgroundWorker)sender;

            var isCurrentSearch = worker.SerialNumber == _serialNumber;
            if (isCurrentSearch)
            {
                _searchingCount--;
                if (!e.Cancelled && e.Result != null)
                {
                    List<IResultItem> result = (List<IResultItem>)e.Result;
                    MergeListToObservableCollection(ResultList, result);
                }

                var spendTime = worker.Watch.ElapsedMilliseconds;
                Console.WriteLine(String.Format("{0} 運作了 {1} 毫秒", worker.Owner, spendTime));
                if (_searchingCount == 0)
                {
                    _workers.Clear();
                    if (ResultList.Count > 0)
                    {
                        SortBestResult(ResultList);
                    }
                    _searchOverEvent?.Invoke();
                    GC.Collect();
                }
            }
        }

        List<IResultItem> MergeListToObservableCollection(List<IResultItem> target, List<IResultItem> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
            return target;
        }

        void SortBestResult(List<IResultItem> results)
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

        public void SubscribeSearchOverEvent(SearchOverEventHandler handler)
        {
            _searchOverEvent = handler;
        }

        public void Wait500ms()
        {
            _pauseEvent.Reset();
            _isPause = true;
            Console.WriteLine("SearchThread is paused");
        }

        public void Resume()
        {
            _pauseEvent.Set();
            _isPause = false;
            Console.WriteLine("SearchThread is resuming ");
        }
    }

}