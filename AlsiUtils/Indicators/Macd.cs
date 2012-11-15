using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    class Macd:Indicator
    {
        private int _L;

        public int L
        {
            get { return _L; }
            set { _L = value; }
        }
        private int _M;

        public int M
        {
            get { return _M; }
            set { _M = value; }
        }
        private int _S;

        public int S
        {
            get { return _S; }
            set { _S = value; }
        }
				private double _Line;

				public double Line
        {
            get { return _Line; }
            set { _Line = value; }
        }
				private double _Trigger;

				public double Trigger
        {
            get { return _Trigger; }
            set { _Trigger = value; }
        }
				private double _Histogram;

				public double Histogram
        {
            get { return _Histogram; }
            set { _Histogram = value; }
        }
    
        
    }
}
