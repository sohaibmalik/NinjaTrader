using System;
using AlsiUtils.Data_Objects;
using System.Linq;
using AlsiUtils;
using System.Diagnostics;

namespace AlsiTrade_Backend
{
  

    public class PrepareForTrade
    {
        public Trade _LastTrade;
        private GlobalObjects.TimeInterval _Interval;
        private AlsiUtils.AlsiDBDataContext dc;

        public PrepareForTrade(GlobalObjects.TimeInterval Interval)
        {
            _Interval = Interval;
        }

        public void GetPricesFromWeb(string ContractName)
        {

            GlobalObjects.Points = HiSat.HistData.GetHistoricalMINUTE_FromWEB(_LastTrade.TimeStamp.AddHours(-1), DateTime.Now, (int)_Interval, ContractName);
            VerifyPrices();

        }

        private void VerifyPrices()
        {
            var pd = GlobalObjects.Points.Last().TimeStamp;
            var t = ConvertTime(_Interval);
            if (pd.Day == t.Day && pd.Hour == t.Hour && pd.Minute == t.Minute)
            {
                AlsiTrade_Backend.UpdateDB.UpdatePricesToTempTable();
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

        private void SkipVerifyPrices()
        {
          
                AlsiTrade_Backend.UpdateDB.UpdatePricesToImportMinuteForTradeUpdate();               
                PricesSyncedEvent p = new PricesSyncedEvent();
                p.ReadyForTradeCalcs = true;
                onPriceSync(this, p);
         
        }


        public void GetPricesFromTick()
        {
            dc = new AlsiUtils.AlsiDBDataContext();
            GlobalObjects.Points = DoStuff.convertTickToMinute(dc.RawTicks.ToList());
            var pd = GlobalObjects.Points.Last();
           // foreach (var pp in GlobalObjects.Prices)          
            //Debug.WriteLine(pd.TimeStamp + "  " + pd.Open + "  " + pd.High + "  " + pd.Low + "  " + pd.Close);
            TickDataToXMinData();
            SkipVerifyPrices();
        }

        private void TickDataToXMinData()
        {
            switch(_Interval )
            {
                case GlobalObjects.TimeInterval.Minute_2:
                    break;

                case GlobalObjects.TimeInterval.Minute_5:
                    dc.OHLC_5_Temp();
                    break;

                case GlobalObjects.TimeInterval.Minute_10:
                    break;
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
                    t = _Now.AddMinutes(-2);
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
