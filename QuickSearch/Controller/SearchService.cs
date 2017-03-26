using System.Collections.Generic;
using QuickSearch.Models.ResultItemsFactory;
using System.Collections.ObjectModel;
using System;
using System.Threading;

namespace QuickSearch.Controller
{
    public class SearchService
    {
        public delegate void SearchOverEventHandler();
        SearchOverEventHandler _searchOverEvent;
        AsyncObservableCollection<IResultItem> _resultList = new AsyncObservableCollection<IResultItem>();
        public int SelectedIndex { get; internal set; }
        
        SearchThread _searchThreadObject = new SearchThread();
        SearchTimeout _searchTimeout = new SearchTimeout(300);

        public ObservableCollection<IResultItem> ResultList
        {
            get { return _resultList; }
        }

        public SearchService()
        {
            _searchThreadObject.EachWorkerSearchOverEvnet += (List<IResultItem> list) =>
            {
                foreach (var item in list)
                {
                    _resultList.Add(item);
                }
            };

            _searchThreadObject.SearchOverEvent += () =>
            {
                SortBestResult(_resultList);
                _searchOverEvent();
            };

            _searchTimeout.SearchEvent = () =>
            {
                _resultList.Clear();
                _searchThreadObject.Search();
                SelectedIndex = 0;
            };
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
            _searchTimeout.Restart();
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

        public void StopSearching()
        {
            _searchTimeout.Stop();
            GC.Collect();
        }
    }
}
