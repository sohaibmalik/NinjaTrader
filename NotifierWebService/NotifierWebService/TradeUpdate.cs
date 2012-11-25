using System;
using System.Collections.Generic;
using ExcelLink;

namespace NotifierWebService
{  
    
    public class TradeUpdate
    {
        public static List<xlTradeOrder> Orders = new List<xlTradeOrder>();
        public static List<Boodskap> Messages = new List<Boodskap>();
               
    }

    public class Boodskap
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
    }
}