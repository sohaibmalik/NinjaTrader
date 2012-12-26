using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Data_Objects
{
    public class RegressionExt
    {
        public double Regression { get; set; }
        public double Slope { get; set; }
        public bool IsSlopeBiggerZero { get; set; }
        public bool IsSlopeHigherThanPrevSlope { get; set; }
        public int OrderVol { get; set; }
        public double newTotalProfit { get; set; }
        public int newRunningProf { get; set; }
        public double Difference { get; set; }


    }
}
