using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AlsiUtils
{
	public class Calc_Indicator
	{
		#region Averageges
		public static List<double> SimpleMovingAverage(int Interval, int NumberOfPoints, List<Price> Price)
		{
			int first = 1;
			int last = Interval;
			int x, y;
			double sum = 0;
			List<double> SMA = new List<double>();

			for (y = Interval; y <= NumberOfPoints; y++)
			{

				for (x = first; x <= last; x++)
				{
					sum = sum + Price[x - 1].Close;
				}

				SMA.Add((sum / Convert.ToInt16(Interval)));
				first++;
				last++;
				sum = 0;
			}

			//Console.WriteLine("==========SMA " + Interval + " ===========");

			//foreach (double d in SMA)
			//{
			//    Console.WriteLine(d);
			//}


			return SMA;
		}

		public static List<double> SimpleMovingAverage(int Interval, int NumberOfPoints, List<double> Price)
		{
			int first = 1;
			int last = Interval;
			int x, y;
			double sum = 0;
			List<double> SMA = new List<double>();

			for (y = Interval; y <= NumberOfPoints; y++)
			{

				for (x = first; x <= last; x++)
				{
					sum = sum + Price[x - 1];
				}

				SMA.Add((sum / Convert.ToInt16(Interval)));
				first++;
				last++;
				sum = 0;
			}

			return SMA;
		}

		public static List<double> ExponentialMovingAverage(int Interval, int NumberOfPoints, List<Price> Price)
		{
			int first = 1;
			int last = Interval;
			int x, y;
			double sum = 0;
			List<double> SMA = new List<double>();
			List<double> EMA = new List<double>();
			double constant = (double)2 / (Interval + 1);


			for (y = Interval; y <= NumberOfPoints; y++)
			{

				for (x = first; x <= last; x++)
				{
					sum = sum + Price[x - 1].Close;
				}

				SMA.Add((sum / Convert.ToInt16(Interval)));
				first++;
				last++;
				sum = 0;
			}

			EMA.Add(0);

			for (int z = 1; z <= SMA.Count - 1; z++)
			{
				double _ema = constant * (Price[z + (Interval - 1)].Close - EMA[z - 1]) + EMA[z - 1];
				EMA.Add(_ema);


			}


			//Console.WriteLine("==========EMA " + Interval + " ===========");

			//foreach (double d in EMA)
			//{
			//    Console.WriteLine(d);
			//}

			return EMA;
		}

		public static List<double> ExponentialMovingAverage(int Interval, int NumberOfPoints, List<double> Price)
		{
			int first = 1;
			int last = Interval;
			int x, y;
			double sum = 0;
			List<double> SMA = new List<double>();
			List<double> EMA = new List<double>();
			double constant = (double)2 / (Interval + 1);


			for (y = Interval; y <= NumberOfPoints; y++)
			{

				for (x = first; x <= last; x++)
				{
					sum = sum + Price[x - 1];
				}

				SMA.Add((sum / Convert.ToInt16(Interval)));
				first++;
				last++;
				sum = 0;
			}

			EMA.Add(0);

			for (int z = 1; z <= SMA.Count - 1; z++)
			{
				double _ema = constant * (Price[z + (Interval - 1)] - EMA[z - 1]) + EMA[z - 1];
				EMA.Add(_ema);


			}


			//Console.WriteLine("==========EMA " + Interval + " ===========");

			//foreach (double d in EMA)
			//{
			//    Console.WriteLine(d);
			//}

			return EMA;
		}

		#endregion
        
		#region RSI
		public static List<double> RSI(int N, int NumberOfPoints, List<Price> Price)
		{
			List<double> rsi = new List<double>();


			List<double> nett_gain = new List<double>();
			List<double> nett_loss = new List<double>();
			List<double> avg_gain = new List<double>();
			List<double> avg_loss = new List<double>();



			nett_gain.Add(0);
			nett_loss.Add(0);

			for (int x = 1; x <= Price.Count - 1; x++)
			{
				double chg = Price[x].Close - Price[x - 1].Close;

				if (chg > 0)
				{
					nett_gain.Add(chg);
					nett_loss.Add(0);
				}
				else
					if (chg < 0)
					{
						nett_gain.Add(0);
						nett_loss.Add(chg * -1);
					}
					else
						if (chg == 0)
						{
							nett_gain.Add(0);
							nett_loss.Add(0);
						}
			}


			avg_gain = ExponentialMovingAverage(N, NumberOfPoints, nett_gain);
			avg_loss = ExponentialMovingAverage(N, NumberOfPoints, nett_loss);



			for (int x = N; x <= avg_loss.Count - 1; x++)
			{
				double rs = avg_gain[x] / avg_loss[x];

				if (rs > 0)
				{
					rsi.Add((double)100 - (100 / (1 + rs)));

				}
				else
					if (rs == 0)
					{
						rsi.Add(0);

					}
			}


			return rsi;
		}
		#endregion
        
	}
}
