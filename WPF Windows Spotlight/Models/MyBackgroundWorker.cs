using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Windows_Spotlight.Models
{
    class MyBackgroundWorker : BackgroundWorker
    {
        public int SerialNumber { get; set; }
        public MyBackgroundWorker(int serialNumber) : base()
        {
            SerialNumber = serialNumber;
        }
    }
}
