using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
    public class Parameter_MAMA : Parameter_EMA_Scalp
    {     
        public double Fast { get; set; }//defualt = 0.5;
        public double Slow { get; set; }//defualt=0.05;
    }
}
