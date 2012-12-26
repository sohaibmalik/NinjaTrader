using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    public abstract class Indicator
    {
      
   
        private DateTime _TimeStamp;

        public DateTime TimeStamp
        {
            get { return _TimeStamp; }
            set { _TimeStamp = value; }
        }
        private double _Price_Close;

        public double Price_Close
        {
            get { return _Price_Close; }
            set { _Price_Close = value; }
        }
        private double _Price_Open;

        public double Price_Open
        {
            get { return _Price_Open; }
            set { _Price_Open = value; }
        }
        private double _Price_High;

        public double Price_High
        {
            get { return _Price_High; }
            set { _Price_High = value; }
        }
        private double _Price_Low;

        public double Price_Low
        {
            get { return _Price_Low; }
            set { _Price_Low = value; }
        }      
       
      

        private string _InstrumentName;

        public string InstrumentName
        {
            get { return _InstrumentName; }
            set { _InstrumentName = value; }
        }
    }
}
