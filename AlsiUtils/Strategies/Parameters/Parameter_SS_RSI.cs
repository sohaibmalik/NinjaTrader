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
      public int RSI_MA2 { get; set; }
      public int RSI_MidLine_Long { get; set; }
      public int RSI_MidLine_Short { get; set; }
      public int RSI_CloseLong { get; set; }               
      public int RSI_CloseShort { get; set; }
     
    }
}
