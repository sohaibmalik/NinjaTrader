using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GATest
{
   public class EvolvingTradingSystemTest
    {
       private List<TradePeriod> Periods = new   List<TradePeriod>();
       public static void Main()
       {
           Console.WriteLine("Starting Evolving Aglo");
           GetAllData();
           CreateDateGroups();
           
           Console.ReadLine();
       }

       private static  void GetAllData()
       {

           string localdata = @"Data Source=PITER-PC;Initial Catalog=AlsiTrade;Integrated Security=True";
           string remotedata = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";

           Console.WindowWidth = 100;
           Console.WriteLine("Getting Prices...");
           AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = remotedata;

           AlsiUtils.Data_Objects.GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
           Console.WriteLine("Done.");
       }

       private static void CreateDateGroups()
       {         

         
               var weekly = from q in AlsiUtils.Data_Objects.GlobalObjects.Points
                            group q by new
                            {
                                Y = q.TimeStamp.Year,
                                M = q.TimeStamp.Month,
                                W = Math.Floor((decimal)q.TimeStamp.DayOfYear / 7) + 1,
                                //D=(DateTime)q.TimeStamp
                            }
                                into FGroup
                                orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.W
                                select new
                                {
                                    Year = FGroup.Key.Y,
                                    Month = FGroup.Key.M,
                                    Week = FGroup.Key.W,
                                 
                                    FirstTradeDate = FGroup.First().TimeStamp,
                                    LastTradeDate = FGroup.Last().TimeStamp,
                                    //AvPrice = (double)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                    // .Average(t => t.RunningProfit),
                                    //SumPrice = (int)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                    //.Sum(t => t.RunningProfit),
                                    //marketmovement = (FGroup.Last().CurrentPrice) - (FGroup.First().CurrentPrice),
                                    //Prices = FGroup.Select(z => z.CurrentPrice),

                                };

               foreach (var v in weekly)
               {
                    Console.WriteLine("Year {0} Month {1} Week {2}  Start {3}  End {4}",v.Year,v.Month,v.Week,v.FirstTradeDate,v.LastTradeDate);
                   
               }

           }

        
       
    }

    public class TradePeriod
    {
        public DateTime Start  { get; set; }
        public DateTime End { get; set; }
        public double RunningProfit { get; set; }
        public int TradeCount { get; set; }
        public string Seq { get; set; }

    }
}
