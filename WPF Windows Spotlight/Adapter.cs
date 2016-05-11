using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WPF_Windows_Spotlight.Foundation;
using System.Collections.ObjectModel;

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
        public delegate void ModelChangedHandler(Item item);
        public ModelChangedHandler UpdateContentHandler;

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
            _selectedIndex = 0;
            _queryList.Clear();
            CancelBackgroundWorker();
            GetBackgroundWorkers();
            foreach (IFoundation foundation in _foundations)
            {
                foundation.SetKeyword(_keyword);
            }
            foreach (BackgroundWorker worker in _workers)
            {
                worker.RunWorkerAsync();
            }
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
            UpdateContentHandler(_queryList[index]);
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
                List<Item> list = ((List<Item>)e.Result);
                if (list.Count > 0)
                {
                    list.ForEach(_queryList.Add);
                    UpdateContentHandler(_queryList[0]);
                }
            }
        }

    }
}
