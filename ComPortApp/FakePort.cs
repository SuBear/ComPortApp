using System;
using System.Timers;

namespace ComPortApp
{
    public class FakePort
    {
        public event EventHandler DataReceived;
        private readonly Timer _timer = new Timer();

        public FakePort(int repeatTime)
        {
            _timer.Interval = repeatTime;
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DataReceived(this, EventArgs.Empty);
            _timer.Start();
        }
    }
}
