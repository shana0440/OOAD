using System.Collections.Generic;
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
using System.Threading;
using System.Windows;

namespace QuickSearch.Controller
{
    public class SearchService
    {
        public delegate void SearchOverEventHandler();
        SearchOverEventHandler _searchOverEvent;
        AsyncObservableCollection<IResultItem> _resultList = new AsyncObservableCollection<IResultItem>();
        public int SelectedIndex { get; internal set; }

        Thread _searchThread;
        SearchThread _searchThreadObject;

        public ObservableCollection<IResultItem> ResultList
        {
            get
            {
                return _resultList;
            }
        }

        public SearchService()
        {
            _searchThreadObject = _searchThreadObject ?? new SearchThread();
            _searchThreadObject.EachWorkerSearchOverEvnet += (List<IResultItem> list) =>
            {
                Console.WriteLine("ResultList Count: {0}", _resultList.Count);
                foreach (var item in list)
                {
                    _resultList.Add(item);
                }
            };
            _searchThreadObject.SubscribeSearchOverEvent(() =>
            {
                _searchThread.Join();
                Console.WriteLine("SearchThread Search Over");
                Console.WriteLine("Search Keyword: {0}", _searchThreadObject.Keyword);
                Console.WriteLine("Get Reslut Items amount: {0}", _resultList.Count);
                Console.WriteLine("====================================");
                SortBestResult(_resultList);
                
                _searchOverEvent();
            });
        }

        void SortBestResult(AsyncObservableCollection<IResultItem> results)
        {
            if(results.Count <= 0) return;
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

        public void Search(string keyword)
        {
            _resultList.Clear();
            _searchThread = _searchThread ?? new Thread(_searchThreadObject.Search);
            if (!_searchThread.IsAlive)
            {
                var isThreadAbandoned = _searchThread.ThreadState == ThreadState.Stopped
                    || _searchThread.ThreadState == ThreadState.Aborted
                    || _searchThread.ThreadState == ThreadState.AbortRequested
                    || _searchThread.ThreadState == ThreadState.Unstarted;
                if (isThreadAbandoned)
                {
                    _searchThread = new Thread(_searchThreadObject.Search);
                    Console.WriteLine("Clear ResultList, Now ResultList Count is {0}", _resultList.Count);
                }
                _searchThread.Start();
                SelectedIndex = 0;
            }

            _searchThreadObject.Wait500ms();
            _searchThreadObject.Keyword = keyword;
        }


        public void SubscribeSearchOverEvent(SearchOverEventHandler handler)
        {
            _searchOverEvent = handler;
        }

        internal void OpenSelectedItemResource()
        {
            if (SelectedIndex < _resultList.Count)
            {
                _resultList[SelectedIndex].OpenResource();
            }
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

        public void AbortSearchThread()
        {
            _searchThread?.Abort();
            GC.Collect();
        }
    }
}
