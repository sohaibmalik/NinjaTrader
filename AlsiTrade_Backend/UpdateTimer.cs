using System;
using System.ComponentModel;
using System.Timers;
using AlsiUtils.Data_Objects;
namespace AlsiTrade_Backend
{


    public class UpdateTimer
    {
        private BackgroundWorker _Bw;
        private Timer _timer = new Timer();
        private GlobalObjects.TimeInterval _interval;
        private DateTime _Now;        
        private bool _UpdatePending;


        public UpdateTimer(GlobalObjects.TimeInterval interval)
        {
            _interval = interval;
            _timer.Interval = 500;
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);           
            _timer.Start();
        }



        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _Now = DateTime.UtcNow.AddHours(2);

            if (_Now.Second >= 0 && _Now.Second <= 40 && _UpdatePending)
            {
                _UpdatePending = false;

                int tenmin = (_Now.Minute / 10) * 10;
                int onemin = _Now.Minute - tenmin;

                CheckForUpdate(tenmin, onemin);
            }

            if (!_UpdatePending && _Now.Second > 50) _UpdatePending = true;
        }

        private void CheckForUpdate(int ten, int one)
        {

            switch (_interval)
            {
                case GlobalObjects.TimeInterval.Minute_1:
                    break;
                case GlobalObjects.TimeInterval.Minute_2:
                    if (one % 2 == 0)
                    {
                        StartUpDateEvent e = new StartUpDateEvent();
                        e.Time = _Now;
                        e.Message = "TWO Minute Update";
                        e.Interval = _interval;
                        onStartUpdate(this, e);

                                            }
                    break;
                case GlobalObjects.TimeInterval.Minute_5:
                    if (one % 5 == 0)
                    {
                        StartUpDateEvent e = new StartUpDateEvent();
                        e.Time = _Now;
                        e.Interval = _interval;
                        e.Message = "FIVE Minute Update";
                        onStartUpdate(this, e);
                    }
                    break;
            }
        }




        public delegate void StartUpdate(object sender, StartUpDateEvent e);
        public class StartUpDateEvent : EventArgs
        {
            public DateTime Time;
            public GlobalObjects.TimeInterval Interval;
            public string Message;

        }
        public event StartUpdate onStartUpdate;

    }
}
