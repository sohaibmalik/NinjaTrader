using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils.Strategies
{
  public class Parameter_SS_RSI:Parameter_SlowStoch 
    {
      public int RSI { get; set; }
      public int RSI_MA { get; set; }
      public int RSI_MidLine { get; set; }
      public int RSI_LowerLine { get; set; }
      public int RSI_UpperLine { get; set; }
     
    }
}
