using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AlsiUtils
{
    class Aroon : Indicator
    {
        public double Aroon_Up { get; set; }

        public double Aroon_Down { get; set; }

        public int N { get; set; }

        /// <summary>
        /// Calculates Aroon
        /// </summary>
        /// <param name="N">Period</param>
        /// <param name="prices">Prices must NOT be reversed</param>
        /// <returns>List of Aroon</returns>

        public static List<Aroon> getManualAroon(int N, List<Price> prices)
        {
            List<Aroon> aroon = new List<AlsiUtils.Aroon>();
            prices.Reverse();
           

            double lastcount = 0;
            List<double> CheckListDown = new List<double>();
            List<double> CheckListUP = new List<double>();
            for (int x = 0; x < prices.Count - N; x++)
            {
               
                Aroon a = new Aroon();

                #region Aroon Down

                double low = prices[x].Low;
                for (int y = x; y < x + N; y++)
                {
                    CheckListDown.Add(prices[y].Low);
                    if (prices[y].Low <= low) low = prices[y].Low;

                }               

                lastcount = 0;
                foreach (double d in CheckListDown)
                {
                    lastcount++;
                    if (d == low) break;
                }
                double aroonDown = 100 * (((double)N - lastcount) / (double)N);

                CheckListDown.Clear();
               // Debug.WriteLine(prices[x].TimeStamp + "  " + prices[x].Low + "  " + "low " + low + "   Per " + lastcount);
                //Debug.WriteLine(prices[x].TimeStamp + " Aroon Down " + aroonDown);

                #endregion

                #region Aroon Up

                double up = prices[x].High;
                for (int y = x; y < x + N; y++)
                {
                    CheckListUP.Add(prices[y].High);
                    if (prices[y].High >= up) up = prices[y].High;

                }

                lastcount = 0;
                foreach (double d in CheckListUP)
                {
                    lastcount++;
                    if (d == up) break;
                }
                double aroonup = 100 * (((double)N - lastcount+1) / (double)N);

                CheckListUP.Clear();
               // Debug.WriteLine(prices[x].TimeStamp + "  " + prices[x].High + "  " + "up " + up + "   Per " + lastcount);
                //Debug.WriteLine(prices[x].TimeStamp + " Aroon Up " + aroonup);
                #endregion
                if (x > 0)
                { 
                a.N = N;
                a.Timestamp = prices[x-1].TimeStamp;
                a.Aroon_Down = aroonDown;
                a.Aroon_Up = aroonup;
                a.Price_Close = prices[x-1].Close;
                a.Price_High = prices[x-1].High;
                a.Price_Low = prices[x-1].Low;
                a.Price_Open = prices[x-1].Open;
                aroon.Add(a);
                }

            }
           

            prices.Reverse();
            return aroon;
        }
    }
}
