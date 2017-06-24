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

namespace QuickSearch.Controller
{
    public class SearchThread
    {
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        int _serialNumber = 0;
        int _searchingCount = 0;
        private string _keyword;
        public volatile ManualResetEvent _pauseEvent = new ManualResetEvent(true); // 暫停執行續用的
        public delegate void SearchOverEventHandler();
        public SearchOverEventHandler SearchOverEvent;
        public delegate void EachWorkerSearchOverEventHandler(List<IResultItem> list);
        public EachWorkerSearchOverEventHandler EachWorkerSearchOverEvnet;

        public void Search()
        {
            CancelCurrentSearchingWorker();
            
            _workers.Add(CreateWoker("計算機"));
            _workers.Add(CreateWoker("檔案系統"));
            _workers.Add(CreateWoker("字典"));
            _workers.Add(CreateWoker("匯率轉換"));
            _workers.Add(CreateWoker("網頁搜尋"));
        }

        public void CancelCurrentSearchingWorker()
        {
            foreach (var worker in _workers)
            {
                worker.CancelAsync();
            }
            _searchingCount = 0;
            _workers.Clear();
        }

        MyBackgroundWorker CreateWoker(string threadName)
        {
            IThread thread;
            switch (threadName)
            {
                case "計算機":
                    thread = new CalculatorThread();
                    break;
                case "檔案系統":
                    thread = new FileSystemThread();
                    break;
                case "字典":
                    thread = new DictionaryThread();
                    break;
                case "匯率轉換":
                    thread = new CurrencyConverterThread();
                    break;
                case "網頁搜尋":
                    thread = new SearchEngineThread();
                    break;
                default:
                    throw new ArgumentException("不存在該功能");
            }

            var worker = new MyBackgroundWorker(_serialNumber, threadName);
            worker.DoWork += new DoWorkEventHandler(thread.DoWork);
            worker.RunWorkerCompleted += BackgroundWorkerSearchOver;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(_keyword);
            _searchingCount++;
            return worker;
        }

        internal void SetKeyword(string keyword)
        {
            this._keyword = keyword;
            _serialNumber++;
            _serialNumber = _serialNumber + 1 % 1000;
        }

        void BackgroundWorkerSearchOver(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (MyBackgroundWorker)sender;

            var isCurrentSearch = worker.SerialNumber == _serialNumber;
            Console.WriteLine("Is Current Search Worker: {0}", isCurrentSearch);
            Console.WriteLine("worker.SerialNumber: {0}, _serialNumber: {1}", worker.SerialNumber, _serialNumber);
            if (isCurrentSearch)
            {
                _searchingCount--;
                if (!e.Cancelled && e.Result != null)
                {
                    List<IResultItem> result = (List<IResultItem>)e.Result;
                    EachWorkerSearchOverEvnet(result);
                }

                var spendTime = worker.Watch.ElapsedMilliseconds;
                Console.WriteLine("{0} 運作了 {1} 毫秒", worker.Owner, spendTime);
                var isAllBackgroundWokersSearchOver = _searchingCount == 0;
                if (isAllBackgroundWokersSearchOver)
                {
                    _workers.Clear();
                    SearchOverEvent?.Invoke();
                    GC.Collect();
                }
            }
        }
        
    }

}