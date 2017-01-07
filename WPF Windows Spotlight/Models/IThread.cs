using System.ComponentModel;

namespace WPF_Windows_Spotlight.Models
{
    public interface IThread
    {
        void DoWork(object sender, DoWorkEventArgs e);
    }
}