using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Indicators
{
    class RegressionLine:Indicator 
    {
        public int N { get; set; }
        public double Regression { get; set; }
        public double Slope { get; set; }
        public double CustomValue { get; set; }
    }
}
