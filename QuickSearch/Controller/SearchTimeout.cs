using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace QuickSearch.Controller
{
    /**
     * 當keyword一段時間沒有變動後才進行搜尋
     */
    class SearchTimeout
    {
        Stopwatch _watch = new Stopwatch();
        Timer _timer = new Timer(250);
        int _delay;
        public delegate void SearchEventHandler();
        public SearchEventHandler SearchEvent;

        public SearchTimeout(int delay)
        {
            _delay = delay;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (_watch.ElapsedMilliseconds > _delay)
            {
                _watch.Stop();
                _watch.Reset();
                SearchEvent();
            }
        }

        public void Restart()
        {
            _watch.Restart();
            if (!_timer.Enabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            _watch.Stop();
            _timer.Stop();
        }
    }
}
