using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies.IndicatorExtentions
{
    public class StochAroon : SlowStoch
    {
        private double _TRIX;

        public double TRIX
        {
            get { return _TRIX; }
            set { _TRIX = value; }
        }
        private double _TRMA;

        public double TRMA
        {
            get { return _TRMA; }
            set { _TRMA = value; }
        }

        public int N { get; set; }
        public double EMA {get;set;}
    }
}
