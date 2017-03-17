using System.ComponentModel;
using System.Diagnostics;

namespace QuickSearch.Models
{
    class MyBackgroundWorker : BackgroundWorker
    {
        public int SerialNumber { get; set; }
        Stopwatch _watch;
        string _owner = "";

        public MyBackgroundWorker(int serialNumber, string owner = "") : base()
        {
            SerialNumber = serialNumber;
            _watch = Stopwatch.StartNew();
            _owner = owner;
        }

        public Stopwatch Watch
        {
            get
            {
                return _watch;
            }
        }

        public string Owner
        {
            get
            {
                return _owner;
            }
        }
    }
}
