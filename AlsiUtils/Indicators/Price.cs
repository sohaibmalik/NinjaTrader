using System;

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


        public Price(DateTime datetime, double open, double high, double low, double close, string Instrument)
        {
            this.TimeStamp = datetime;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.InstrumentName = Instrument;
        }

        public Price()
        {

        }



    }
}
