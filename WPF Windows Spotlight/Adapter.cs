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

        public Adapter()
        {
            _workers = new List<BackgroundWorker>();
            _factory = new FoundationFactory();
            _queryList = new ObservableCollection<Item>();
            _foundations = _factory.GetFoundations();
        }

        public void Search(string keyword)
        {
            _keyword = keyword;
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
                    list.ForEach(_queryList.Add);
            }
        }
    }
}
