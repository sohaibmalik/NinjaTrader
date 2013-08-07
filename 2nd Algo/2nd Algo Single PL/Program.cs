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
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            var algo = new TrailingStop();


            //algo.SetTriggers(int.Parse(args[0].Split(',')[0]), int.Parse(args[0].Split(',')[1]), int.Parse(args[0].Split(',')[2]), int.Parse(args[0].Split(',')[3]));
            algo.SetTriggers();        
           

            Console.ReadLine();
          
       
        }
    }


}
