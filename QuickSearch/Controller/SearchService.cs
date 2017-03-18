﻿using System.Collections.Generic;
using System.ComponentModel;
using QuickSearch.Models.Calculator;
using QuickSearch.Models;
using QuickSearch.Models.ResultItemsFactory;
using System.Collections.ObjectModel;
using QuickSearch.Models.FileSystem;
using System;
using QuickSearch.Models.Dictionary;
using QuickSearch.Models.CurrencyConverter;
using QuickSearch.Models.SearchEngine;

namespace QuickSearch.Controller
{
    public class SearchService
    {
        int _searchingCount = 0;
        List<BackgroundWorker> _workers = new List<BackgroundWorker>();
        public delegate void SearchOverEventHandler();
        SearchOverEventHandler _searchOverEvent;
        ObservableCollection<IResultItem> _resultList = new ObservableCollection<IResultItem>();
        int _serialNumber = 0;
        public int SelectedIndex { get; internal set; }

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

            IThread calculatorThread = new CalculatorThread();
            MyBackgroundWorker calculatorWorker = new MyBackgroundWorker(_serialNumber, "計算機");
            calculatorWorker.DoWork += new DoWorkEventHandler(calculatorThread.DoWork);
            calculatorWorker.RunWorkerCompleted += SearchOver;
            calculatorWorker.WorkerSupportsCancellation = true; // support cancel
            calculatorWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(calculatorWorker);

            IThread fileSystemThread = new FileSystemThread();
            MyBackgroundWorker fileSystemWorker = new MyBackgroundWorker(_serialNumber, "檔案系統");
            fileSystemWorker.DoWork += new DoWorkEventHandler(fileSystemThread.DoWork);
            fileSystemWorker.RunWorkerCompleted += SearchOver;
            fileSystemWorker.WorkerSupportsCancellation = true;
            fileSystemWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(fileSystemWorker);

            IThread directoryThread = new DictionaryThread();
            MyBackgroundWorker directoryWorker = new MyBackgroundWorker(_serialNumber, "字典");
            directoryWorker.DoWork += new DoWorkEventHandler(directoryThread.DoWork);
            directoryWorker.RunWorkerCompleted += SearchOver;
            directoryWorker.WorkerSupportsCancellation = true;
            directoryWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(directoryWorker);

            IThread currencyThread = new CurrencyConverterThread();
            MyBackgroundWorker currencyWorker = new MyBackgroundWorker(_serialNumber, "匯率轉換");
            currencyWorker.DoWork += new DoWorkEventHandler(currencyThread.DoWork);
            currencyWorker.RunWorkerCompleted += SearchOver;
            currencyWorker.WorkerSupportsCancellation = true;
            currencyWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(currencyWorker);

            IThread searchEngineThread = new SearchEngineThread();
            MyBackgroundWorker searchEngineWorker = new MyBackgroundWorker(_serialNumber, "網頁搜尋");
            searchEngineWorker.DoWork += new DoWorkEventHandler(searchEngineThread.DoWork);
            searchEngineWorker.RunWorkerCompleted += SearchOver;
            searchEngineWorker.WorkerSupportsCancellation = true;
            searchEngineWorker.RunWorkerAsync(keyword);
            _searchingCount++;
            _workers.Add(searchEngineWorker);
        }

        public void CancelCurrentSearching()
        {
            foreach (var worker in _workers)
            {
                worker.CancelAsync();
            }
            SelectedIndex = 0;
            _searchingCount = 0;
            _resultList.Clear();
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
                    MergeListToObservableCollection(_resultList, result);
                }

                var spendTime = worker.Watch.ElapsedMilliseconds;
                Console.WriteLine(String.Format("{0} 運作了 {1} 毫秒", worker.Owner, spendTime));
                if (_searchingCount == 0)
                {
                    _workers.Clear();
                    if (_resultList.Count > 0)
                    {
                        SortBestResult(_resultList);
                    }
                    SelectItem(0);
                    _searchOverEvent?.Invoke();
                    GC.Collect();
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

        internal void OpenSelectedItemResource()
        {
            _resultList[SelectedIndex].OpenResource();
        }

        public void OpenItemResource(int selectedIndex)
        {
            _resultList[selectedIndex].OpenResource();
        }

        public void SelectItem(int selectedIndex)
        {
            if (selectedIndex >= 0 && selectedIndex < _resultList.Count)
            {
                foreach (var item in _resultList)
                {
                    item.IsSelected = false;
                }
                SelectedIndex = selectedIndex;
                _resultList[SelectedIndex].IsSelected = true;
            }
        }

    }
}