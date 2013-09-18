using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    public abstract class Indicator
    {


        private DateTime _TimeStamp;

        public DateTime TimeStamp
        {
            get { return _TimeStamp; }
            set { _TimeStamp = value; }
        }
        private double _Price_Close;

        public double Price_Close
        {
            get { return _Price_Close; }
            set { _Price_Close = value; }
        }
        private double _Price_Open;

        public double Price_Open
        {
            get { return _Price_Open; }
            set { _Price_Open = value; }
        }
        private double _Price_High;

        public double Price_High
        {
            get { return _Price_High; }
            set { _Price_High = value; }
        }
        private double _Price_Low;

        public double Price_Low
        {
            get { return _Price_Low; }
            set { _Price_Low = value; }
        }



        private string _InstrumentName;

        public string InstrumentName
        {
            get { return _InstrumentName; }
            set { _InstrumentName = value; }
        }
    }

    public class VariableIndicator : Indicator
    {
        public double Value { get; set; }
    }

    public class Aroon : Indicator
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
                double aroonup = 100 * (((double)N - lastcount + 1) / (double)N);

                CheckListUP.Clear();
                // Debug.WriteLine(prices[x].TimeStamp + "  " + prices[x].High + "  " + "up " + up + "   Per " + lastcount);
                //Debug.WriteLine(prices[x].TimeStamp + " Aroon Up " + aroonup);
                #endregion
                if (x > 0)
                {
                    a.N = N;
                    a.TimeStamp = prices[x - 1].TimeStamp;
                    a.Aroon_Down = aroonDown;
                    a.Aroon_Up = aroonup;
                    a.Price_Close = prices[x - 1].Close;
                    a.Price_High = prices[x - 1].High;
                    a.Price_Low = prices[x - 1].Low;
                    a.Price_Open = prices[x - 1].Open;
                    aroon.Add(a);
                }

            }


            prices.Reverse();
            return aroon;
        }
    }

    public class BollingerBand : Indicator
    {
        private double _P;

        public double P
        {
            get { return _P; }
            set { _P = value; }
        }
        private int _N;

        public int N
        {
            get { return _N; }
            set { _N = value; }
        }
        private double _Upper;

        public double Upper
        {
            get { return _Upper; }
            set { _Upper = value; }
        }
        private double _Lower;

        public double Lower
        {
            get { return _Lower; }
            set { _Lower = value; }
        }
        private double _Mid;

        public double Mid
        {
            get { return _Mid; }
            set { _Mid = value; }
        }
    }

    public class EMA : Indicator
    {
        public double Ema { get; set; }
        public double CustomValue { get; set; }
    }
  
    public class Macd : Indicator
    {
        private int _L;

        public int L
        {
            get { return _L; }
            set { _L = value; }
        }
        private int _M;

        public int M
        {
            get { return _M; }
            set { _M = value; }
        }
        private int _S;

        public int S
        {
            get { return _S; }
            set { _S = value; }
        }
        private double _Line;

        public double Line
        {
            get { return _Line; }
            set { _Line = value; }
        }
        private double _Trigger;

        public double Trigger
        {
            get { return _Trigger; }
            set { _Trigger = value; }
        }
        private double _Histogram;

        public double Histogram
        {
            get { return _Histogram; }
            set { _Histogram = value; }
        }


    }

    public class RegressionLine : Indicator
    {
        public int N { get; set; }
        public double Regression { get; set; }
        public double Slope { get; set; }
        public double CustomValue { get; set; }
    }

    public class Rsi : Indicator
    {
        private int _N;

        public int N
        {
            get { return _N; }
            set { _N = value; }
        }
        private double _RSI;

        public double RSI
        {
            get { return _RSI; }
            set { _RSI = value; }
        }


    }

    public class ADX:Indicator
    {
        public int N { get; set; }
        public double AvDirIndex { get; set; }
        public double DI_Up { get; set; }
        public double DI_Down { get; set; }
        public double Test  { get; set; }
    }

    public class SAR : Indicator
    {
        public double StopAndReverse { get; set; }
    }

    public class SlowStoch : Indicator
    {
        public int Slow_D { get; set; }

        private int _Slow_K;

        public int Slow_K
        {
            get { return _Slow_K; }
            set { _Slow_K = value; }
        }
        private int _Fast_K;

        public int Fast_K
        {
            get { return _Fast_K; }
            set { _Fast_K = value; }
        }

        private double _K;

        public double K
        {
            get { return _K; }
            set { _K = value; }
        }
        private double _D;

        public double D
        {
            get { return _D; }
            set { _D = value; }
        }
    }

    public class SMA : Indicator
    {
        public double Sma { get; set; }
        public double CustomValue { get; set; }
    }

 /// <summary>
 /// MAMA defualt = 0.05
 /// FAMA(Following Adaptive Moving Average) defualt=0.5
 /// </summary>
    public class MAMA:Indicator
    {
        public double Mama { get; set; }
        public double Fama { get; set; }
    }

    public class StandardDev : Indicator
    {
        public int N { get; set; }
        public double StdDev { get; set; }
        public double CustomValue { get; set; }
        public double SingleStdev { get; set; }
    }

    public class Trix : Indicator
    {

        private double _TRIX;

        public double TRIX
        {
            get { return _TRIX; }
            set { _TRIX = value; }
        }
        private double _TRMA;

        public double TRMA
        {
            get { return _TRMA; }
            set { _TRMA = value; }
        }
    }

    public class ATR : Indicator
    {
       public double AvgTrueRange { get; set; }
       public  int N { get; set; }
    }
}
