using AlsiUtils;
using AlsiUtils.Data_Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class Utils
    {
        public static void PrintAllProperties(object obj)
        {
            Console.WriteLine("===========Print new object====================");
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                Console.WriteLine("{0}={1}", name, value);
            }
            Console.WriteLine("===============================================");
        }


        public static List<Trade> GetTrades()
       {
        Statistics _Stats = new Statistics();
         List<Trade> _FullTradeList;
         List<CompletedTrade> NewTrades;
         List<Trade> _TradeOnlyList;

         double TPF = 10;
         double SLF = 20;


         AlsiUtils.Strategies.Parameter_MAMA E = new AlsiUtils.Strategies.Parameter_MAMA()
         {
             

             Fast = 0.5,//0.1
             Slow = 0.05,//0.01

             A_EMA1 = 16,
             A_EMA2 = 17,
             B_EMA1 = 43,
             B_EMA2 = 45,
             C_EMA = 52,
             TakeProfit = 550,
             StopLoss = -250,
             TakeProfitFactor = TPF,
             StoplossFactor = SLF,
             CloseEndofDay = false,
         };
           
            GlobalObjects.TimeInterval t = GlobalObjects.TimeInterval.Minute_5;
            DataBase.dataTable dt = DataBase.dataTable.MasterMinute;
            //_FullTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(GetParametersSAR_EMA(), t, false, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), dt);
            _FullTradeList = AlsiTrade_Backend.RunCalcs.RunMAMAScalp(E, t, false, new DateTime(2012, 01, 01), new DateTime(2014, 04, 15), dt);
            _FullTradeList = _Stats.CalcBasicTradeStats_old(_FullTradeList);
            NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(11, _FullTradeList);
            NewTrades = _Stats.CalcExpandedTradeStats(NewTrades);
            _TradeOnlyList = CompletedTrade.CreateList(NewTrades);


            return _FullTradeList;
        
       }
    }
}
