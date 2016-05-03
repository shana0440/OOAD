using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WPF_Windows_Spotlight.Foundation;

namespace WPF_Windows_Spotlight
{
    public class Adapter
    {
        private string _keyword;
        private List<BackgroundWorker> _workers;
        private string _result;
        private FoundationFactory _factory;

        public Adapter()
        {
            _workers = new List<BackgroundWorker>();
            _factory = new FoundationFactory();
        }

        public void Search(string keyword)
        {
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
            bgworker.RunWorkerCompleted += WorkerCompleted; // 結束時呼叫
            bgworker.DoWork += foundation.DoWork; // start thread時呼叫
            return bgworker;
        }

        public string GetResult()
        {
            return _result;
        }

        private void CancelBackgroundWorker()
        {
            foreach (BackgroundWorker worker in _workers)
            {
                worker.CancelAsync();
            }
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Result = " + e.Result);
            _result = e.Result.ToString();
        }
    }
}
