using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
namespace AlsiUtils.Data_Objects
{
    public static class GlobalObjects
    {
        public static List<Price> Points = new List<Price>();
        public static string CustomConnectionString;
        public static Trade LastTrade; //Last Trade form long list Carry over to temp list when trading
        public static bool LastTradeOfTheDay;

        public enum TimeInterval
        {
            Minute_1 = 1,
            Minute_2 = 2,
            Minute_5 = 5,
            Minute_10 = 10,

        }

        private static List<Price> _EndOfDayPrice = new List<Price>();
        public static List<Price> EndOfDayPrice
        {
            get
            {
                if (Points.Count == 0) throw new Exception("GlobalObjects.Points is empty");
                _EndOfDayPrice.Clear();
                var daily = from q in Points
                            group q by new
                            {
                                Y = q.TimeStamp.Year,
                                M = q.TimeStamp.Month,
                                D = q.TimeStamp.Day,

                            }
                                into FGroup
                                orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.D
                                select new
                                {
                                    Year = FGroup.Key.Y,
                                    Month = FGroup.Key.M,
                                    Day = FGroup.Key.D,
                                    Open = FGroup.First().Open,
                                    High = FGroup.Select(z => z.High).Max(),
                                    Low = FGroup.Select(z => z.Low).Min(),
                                    Close = FGroup.Last().Close,
                                    Inst = FGroup.First().InstrumentName,
                                  
                                };

                foreach (var v in daily)
                {
                    var p = new Price
                    {
                        TimeStamp = new DateTime(v.Year, v.Month, v.Day, 17, 30, 0),
                        InstrumentName = v.Inst,
                        Open = v.Open,
                        High = v.High,
                        Low = v.Low,
                        Close = v.Close,
                      
                    };

                    _EndOfDayPrice.Add(p);
                }
                return _EndOfDayPrice;
            }

        }

    }
}
