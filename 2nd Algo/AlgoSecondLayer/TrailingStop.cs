using AlsiUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class TrailingStop
    {
        private List<Trade> Trades = new List<Trade>();

        public TrailingStop()
        {
            Algo.LoadPrice();
            Trades=Algo.Trades;
        }

        public void PrintTrades()
        {
            for (int x = 1000; x < 2000; x++)
            {
                Utils.PrintAllProperties(Trades[x]);
            }
        }

    }
}
