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
            algo.PrintTrades();

            Console.ReadLine();
       
        }
    }


}
