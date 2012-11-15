using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
   public class Parameter_General
    {
        private int _TakeProfit;

        public int TakeProfit
        {
            get { return _TakeProfit; }
            set { _TakeProfit = value; }
        }
        private int _StopLoss;

        public int StopLoss
        {
            get { return _StopLoss; }
            set { _StopLoss = value; }
        }
    }
}
