using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
   public  class Parameter_EMA_Scalp:Parameter_General
    {
       public int A_EMA1 { get; set; }
       public int A_EMA2 { get; set; }
       public int A_EMA3 { get; set; }
       public int A_EMA4 { get; set; }
       public int A_EMA5 { get; set; }
       public int A_EMA6 { get; set; }

       public int B_EMA1 { get; set; }
       public int B_EMA2 { get; set; }
       public int B_EMA3 { get; set; }
       public int B_EMA4 { get; set; }
       public int B_EMA5 { get; set; }
       public int B_EMA6 { get; set; }

       public int C_EMA { get; set; }

       public int Period { get; set; }
    }
}
