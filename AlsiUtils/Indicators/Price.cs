using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    public class Price : Point
    {
        private string _InstrumentCode;

        public string InstrumentName
        {
            get { return _InstrumentCode; }
            set { _InstrumentCode = value; }
        }


			public Price(DateTime datetime, double open, double high, double low, double close, int interval, string Instrument)
        {
            this.TimeStamp = datetime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Interval = interval;
            this.InstrumentName = Instrument;        
        }

        public Price()
        {

        }



    }
}
