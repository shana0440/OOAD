using System.ComponentModel;

namespace QuickSearch.Models
{
    public interface IThread
    {
        void DoWork(object sender, DoWorkEventArgs e);
    }
}