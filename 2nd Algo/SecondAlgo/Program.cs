using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoSecondLayer;
namespace SecondAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            Algo.LoadPrice();
            Algo.CalculatePL(Algo.CalcIndicators());           

        }
    }
}
