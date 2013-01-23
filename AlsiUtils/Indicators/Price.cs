using System;

namespace AlsiUtils
{
    public class Price:ICloneable
    {

        public DateTime TimeStamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public string InstrumentName { get; set; }
       


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






        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
