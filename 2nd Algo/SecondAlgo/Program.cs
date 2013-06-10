using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            AlgoSecondLayer.Algo.LoadPrice();
            AlgoSecondLayer.Algo.CalcIndicators();
            Console.ReadLine();
        }
    }
}
