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
        private string _result;
        private FoundationFactory _factory;
        private ObservableCollection<Item> _queryList;
        public delegate void ViewChangeHandler();
        public ViewChangeHandler UpdateView;

        public Adapter()
        {
            _workers = new List<BackgroundWorker>();
            _factory = new FoundationFactory();
            _queryList = new ObservableCollection<Item>();
        }

        public void Search(string keyword)
        {
            _queryList.Clear();
            CancelBackgroundWorker();
            List<string> foundations = _factory.GetFoundations();
            foreach (string foundation in foundations)
            {
                BackgroundWorker worker = CreateBackgroundWorker(foundation, keyword);
                worker.RunWorkerAsync();
                _workers.Add(worker);
            }
        }

        private BackgroundWorker CreateBackgroundWorker(string foundationName, string keyword)
        {
            BackgroundWorker bgworker = new BackgroundWorker();
            IFoundation foundation = _factory.CreateFoundation(foundationName, keyword);
            bgworker.WorkerSupportsCancellation = true; // 支援取消
            bgworker.RunWorkerCompleted += WorkerCompleted; // 結束時呼叫
            bgworker.DoWork += foundation.DoWork; // start thread時呼叫
            return bgworker;
        }

        public string GetResult()
        {
            return _result;
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
            Console.WriteLine("Result = " + e.Result);
            if (e.Result != null)
            {
                //_result = e.Result.ToString();
                _queryList.Add((Item)e.Result);
            }
        }
    }
}
