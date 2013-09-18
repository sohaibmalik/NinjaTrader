using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
   public class Parameter_General
    {
       

        public int TakeProfit{get;set;}
        public int StopLoss { get; set;}
        public double TakeProfitFactor { get; set; }
        public double StoplossFactor { get; set; }
        public bool CloseEndofDay { get; set; }
        
    }
}
