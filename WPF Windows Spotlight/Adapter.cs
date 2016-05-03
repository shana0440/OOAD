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

        public Adapter()
        {
            _workers = new List<BackgroundWorker>();
        }

        public void Search(string keyword)
        {
            CancelBackgroundWorker();

            BackgroundWorker bgworker = new BackgroundWorker();
            IFoundation foundation = new Calculator(keyword);
            bgworker.RunWorkerCompleted += WorkerCompleted;
            bgworker.DoWork += foundation.DoWork;
            bgworker.RunWorkerAsync();
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
