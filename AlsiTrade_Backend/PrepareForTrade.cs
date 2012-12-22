using System;

using System.Linq;
using AlsiUtils.Data_Objects;

namespace AlsiTrade_Backend
{
    public class PrepareForTrade
    {
        public void GetPricesFromWeb(GlobalObjects.TimeInterval Interval)
        {
            var PD = HiSat.HistData.GetHistoricalMINUTE_FromWEB(DateTime.Now.AddDays(-5), DateTime.Now, (int)Interval, "MAR13ALSI");
            var pd = PD.Last().TimeStamp;
            var t = ConvertTime(GlobalObjects.TimeInterval.Minute_5);
            if (pd.Day == t.Day && pd.Hour == t.Hour && pd.Minute == t.Minute)
            {
                PricesSyncedEvent p = new PricesSyncedEvent();
                p.ReadyForTradeCalcs = true;
                onPriceSync(this, p);
            }
            else
            {
                PricesSyncedEvent p = new PricesSyncedEvent();
                p.ReadyForTradeCalcs = false;
                onPriceSync(this, p);
            }

        }




        private DateTime ConvertTime(GlobalObjects.TimeInterval Interval)
        {
            DateTime _Now = DateTime.UtcNow.AddHours(2);
            DateTime t = new DateTime();
            // int tenmin = (_Now.Minute / 10) * 10;
            //int onemin = _Now.Minute - tenmin;

            switch (Interval)
            {
                case GlobalObjects.TimeInterval.Minute_1:
                    break;

                case GlobalObjects.TimeInterval.Minute_2:
                    break;

                case GlobalObjects.TimeInterval.Minute_5:
                    t = _Now.AddMinutes(-5);
                    break;

                case GlobalObjects.TimeInterval.Minute_10:
                    break;
            }

            return t;
        }

        public event PricesSynced onPriceSync;
        public delegate void PricesSynced(object sender, PricesSyncedEvent e);
        public class PricesSyncedEvent : EventArgs
        {
            public bool ReadyForTradeCalcs;

        }

    }
}
