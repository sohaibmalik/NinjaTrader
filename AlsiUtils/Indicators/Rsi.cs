using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    class Rsi:Indicator
    {
        private int _N;

        public int N
        {
            get { return _N; }
            set { _N = value; }
        }
				private double _RSI;

				public double RSI
        {
            get { return _RSI; }
            set { _RSI = value; }
        }

        
    }
}
