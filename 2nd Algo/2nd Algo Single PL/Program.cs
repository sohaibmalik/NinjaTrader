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
           var a= new AlgoSecondLayer.SinglePl();

           a.CheckTradeAfterPL(500,1);

           Console.ReadLine();
        }
    }


}
