using AlgoSecondLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _2nd_Algo_Single_PL
{
  public  class Program
    {
        static void Main(string[] args)
        {
            var algo = new TrailingStop();


         //   algo.SetTriggers(int.Parse(args[0].Split(',')[0]), int.Parse(args[0].Split(',')[1]), int.Parse(args[0].Split(',')[2]), int.Parse(args[0].Split(',')[3]));
          //  algo.SetTriggers();
          //  algo.PrintTrades();
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            List<string> lines = new List<string>();
            foreach (var t in Utils.GetTrades())
                lines.Add(t.TimeStamp + "," + t.Reason + "," + t.TradeVolume + "," + t.RunningProfit);
          
          
            var sw = new StreamWriter(@"D:\plRev.csv");
            foreach (var s in lines)
                sw.WriteLine(s);
            sw.Close();

           // Console.ReadLine();
       
        }
    }


}
