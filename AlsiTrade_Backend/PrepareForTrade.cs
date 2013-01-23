using System;
using AlsiUtils.Data_Objects;
using System.Linq;
using AlsiUtils;
using System.Diagnostics;
using AlsiUtils.Strategies;
namespace AlsiTrade_Backend
{


    public class PrepareForTrade
    {
        public Trade _LastTrade;
        private GlobalObjects.TimeInterval _Interval;
        private DateTime _Start;
        string _ContractName;
        private Parameter_EMA_Scalp _Param;
        public PrepareForTrade(GlobalObjects.TimeInterval Interval, string ContractName, Parameter_EMA_Scalp Parameter, DateTime StartPeriod)
        {
            _Interval = Interval;
            _ContractName = ContractName;
            _Start = StartPeriod;
            _Param = Parameter;
        }

        public void GetPricesFromWeb()
        {

            GlobalObjects.Points = HiSat.HistData.GetHistoricalMINUTE_FromWEB(_LastTrade.TimeStamp.AddDays(-10), DateTime.Now, (int)_Interval, _ContractName);
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




        public void GetPricesFromTick()
        {
            AlsiTrade_Backend.UpdateDB.FullHistoricUpdate_MasterMinute(_ContractName);
            PricesSyncedEvent p = new PricesSyncedEvent();
            p.ReadyForTradeCalcs = true;
            onPriceSync(this, p);
        }


        private void TickDataToXMinData()
        {
            AlsiUtils.AlsiDBDataContext dc = new AlsiDBDataContext();
            switch (_Interval)
            {
                case GlobalObjects.TimeInterval.Minute_2:
                    break;

                case GlobalObjects.TimeInterval.Minute_5:
                    dc.OHLC_5_Temp();
                    break;

                case GlobalObjects.TimeInterval.Minute_10:
                    break;
            }

            GlobalObjects.Points.Clear();
            foreach (var p in dc.OHLC_Temps)
            {
                var P = new Price()
                {
                    TimeStamp = p.Stamp,
                    Open = p.O,
                    High = p.H,
                    Low = p.L,
                    Close = p.C,
                    InstrumentName = _ContractName,
                };
                GlobalObjects.Points.Add(P);
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
