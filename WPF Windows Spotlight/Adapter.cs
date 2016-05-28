using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WPF_Windows_Spotlight.Foundation;
using System.Collections.ObjectModel;
using WPF_Windows_Spotlight.Foundation.ItemType;

namespace WPF_Windows_Spotlight
{
    public class Adapter
    {
        private string _keyword;
        private List<BackgroundWorker> _workers;
        private FoundationFactory _factory;
        private ObservableCollection<Item> _queryList;
        private List<IFoundation> _foundations;
        private int _selectedIndex;
        public delegate void ModelChangedHandler();
        public ModelChangedHandler UpdateContentHandler;
        private string _identify;

        public Adapter()
        {
            _workers = new List<BackgroundWorker>();
            _factory = new FoundationFactory();
            _queryList = new ObservableCollection<Item>();
            _foundations = _factory.GetFoundations();
            _selectedIndex = 0;
        }

        public void Search(string keyword)
        {
            _keyword = keyword;
            _selectedIndex = -1;
            _queryList.Clear();
            CancelBackgroundWorker();
            GetBackgroundWorkers();
            _identify = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            foreach (IFoundation foundation in _foundations)
            {
                foundation.SetKeyword(_keyword);
            }
            foreach (BackgroundWorker worker in _workers)
            {
                worker.RunWorkerAsync(_identify);
            }
        }

        public int GetWrokerCount()
        {
            return _workers.Count;
        }

        private BackgroundWorker CreateBackgroundWorker(IFoundation foundation)
        {
            BackgroundWorker bgworker = new BackgroundWorker();
            bgworker.WorkerSupportsCancellation = true; // 支援取消
            bgworker.RunWorkerCompleted += WorkerCompleted; // 結束時呼叫
            bgworker.DoWork += foundation.DoWork; // start thread時呼叫
            return bgworker;
        }

        private void GetBackgroundWorkers()
        {
            foreach (IFoundation foundation in _foundations)
            {
                BackgroundWorker worker = CreateBackgroundWorker(foundation);
                _workers.Add(worker);
            }
        }

        public ObservableCollection<Item> QueryList
        {
            get { return _queryList; }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }

        public void SelectItem(int index)
        {
            if (_queryList.Count == 0) return;
            foreach (Item item in _queryList)
            {
                item.IsSelected = false;
            }
            if (index < 0) index = 0;
            if (index > _queryList.Count - 1) index = _queryList.Count - 1;
            _queryList[index].IsSelected = true;
            _selectedIndex = index;
            if (UpdateContentHandler != null)
            {
                UpdateContentHandler();
            }
        }

        private void CancelBackgroundWorker()
        {
            foreach (BackgroundWorker worker in _workers)
            {
                if (worker.IsBusy)
                {
                    worker.CancelAsync();
                }
            }
            _workers.Clear();
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Result != null)
            {
                KeyValuePair<string, List<Item>> result = ((KeyValuePair<string, List<Item>>)e.Result);
                if (result.Key != _identify) 
                    return;

                if (result.Value.Count > 0)
                {
                    _selectedIndex = 0;
                    result.Value.ForEach(_queryList.Add);
                    
                    _queryList[0].IsSelected = true;
                }
                if (UpdateContentHandler != null)
                {
                    UpdateContentHandler();
                }
            }
        }

    }
}
