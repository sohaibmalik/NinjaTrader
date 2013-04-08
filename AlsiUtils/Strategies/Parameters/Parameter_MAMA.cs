using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
    public class Parameter_EMA_SAR : Parameter_EMA_Scalp
    {      
        public double SAR_STEP { get; set; }
        public double SAR_MAXP { get; set; }
    }
}
