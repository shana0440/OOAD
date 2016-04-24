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

        public Adapter()
        {

        }

        public void Search(string keyword)
        {
            
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Result = " + e.Result);
        }
    }
}
