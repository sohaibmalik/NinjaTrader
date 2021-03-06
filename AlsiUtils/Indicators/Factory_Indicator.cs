﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AlsiUtils;
using TicTacTec.TA.Library;

namespace AlsiUtils
{
	public class Factory_Indicator
	{

        public static List<ADX> createADX(int N, List<Price> Price)
        {
            List<ADX> ADX = new List<ADX>();


            double[] _close = new double[Price.Count];
            double[] _high = new double[Price.Count];
            double[] _low = new double[Price.Count];
            double[] _open = new double[Price.Count];

            double[] _minusDi = new double[Price.Count];
            double[] _plusDi = new double[Price.Count];

            for (int x = 0; x < Price.Count; x++)
            {
                _close[x] = Price[x].Close;
                _high[x] = Price[x].High;
                _low[x] = Price[x].Low;
                _open[x] = Price[x].Open;

            }

            int a, b;
                                  
           
           // Core.Dx(0, Price.Count - 1, _high, _low, _close, N, out a, out b, _adx);
            Core.MinusDI(0, Price.Count - 1, _high, _low, _close, N, out a, out b, _minusDi);
            Core.PlusDI(0, Price.Count - 1, _high, _low, _close, N, out a, out b, _plusDi );


            for (int x = 0; x < _plusDi.Count() - a; x++)
            {
                //Debug.WriteLine("UPPER " + _upperBand[x]);
                //Debug.WriteLine("MID " + _midBand[x]);
                //Debug.WriteLine("LOWER " + _lowerBand[x]);
                //Debug.WriteLine(Price[x + a].TimeStamp + "   " + Price[x + a].Close);
                //Debug.WriteLine("========" + x + "===========");

                ADX adx = new ADX();
                adx.AvDirIndex = 0;
                adx.DI_Down = _minusDi[x];
                adx.DI_Up = _plusDi[x];
                adx.N = N;               
                adx.TimeStamp = Price[x + a].TimeStamp;
                adx.Price_Close = Price[x + a].Close;
                adx.Price_High = Price[x + a].High;
                adx.Price_Low = Price[x + a].Low;
                adx.Price_Open = Price[x + a].Open;
                ADX.Add(adx);
            }

            return ADX;
           
        }

		public static List<Macd> createMACD(int M, int L, int S, List<Price> Price)
		{
			List<Macd> MACD = new List<Macd>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_price[x] = Price[x].Close;


			}

			double[] macd = new double[Price.Count];
			double[] trigger = new double[Price.Count];
			double[] hist = new double[Price.Count];


			int a, b;


			Core.Macd(0, Price.Count - 1, _price, M, L, S, out a, out b, macd, trigger, hist);




			int zeroCount = 0;
			for (int x = 0; x < Price.Count; x++)
			{
				if (macd[x] == 0)
				{
					zeroCount++;
				}

			}




			Debug.WriteLine("ZeroCount " + zeroCount);
			for (int x = 0; x < Price.Count - zeroCount; x++)
			{
				//Debug.WriteLine("Macd " + macd[x]);
				//Debug.WriteLine("Trigger " + trigger[x]);
				//Debug.WriteLine("Histo " + hist[x] * 2);
				//Debug.WriteLine(Price[x+zeroCount].TimeStamp + "   " + Price[x+zeroCount].Close);

				//Debug.WriteLine("===================");

				Macd m = new Macd();
				m.L = L;
				m.M = M;
				m.S = S;
				m.TimeStamp = Price[x + zeroCount].TimeStamp;
				m.Trigger = trigger[x];
				m.Line = macd[x];
				m.Histogram = hist[x] * 2;
				m.Price_Close = Price[x + zeroCount].Close;
				m.Price_High = Price[x + zeroCount].High;
				m.Price_Low = Price[x + zeroCount].Low;
				m.Price_Open = Price[x + zeroCount].Open;
				MACD.Add(m);



			}






			return MACD;
		}

		public static List<Rsi> createRSI(int N, List<Price> Price)
		{

			List<Rsi> RSI = new List<Rsi>();
			double[] _price = new double[Price.Count];
			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;
			double[] _rsi = new double[Price.Count];
			int a, b;

			Core.Rsi(0, Price.Count - 1, _price, N, out a, out b, _rsi);

			for (int x = 0; x < Price.Count - a; x++)
			{
				Rsi r = new Rsi
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					RSI = _rsi[x]

				};
				RSI.Add(r);
			}


			return RSI;
		}

		public static List<Trix> createTRIX(int N, int M, List<Price> Price)
		{
			List<Trix> TRIX = new List<Trix>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_price[x] = Price[x].Close;

			}

			double[] _trix = new double[Price.Count];
			int a, b;
			int c, d;

			Core.Trix(0, Price.Count - 1, _price, N, out a, out b, _trix);

			double[] _trma = new double[Price.Count];
			double[] _trix_2 = new double[Price.Count - a];




			for (int x = 0; x < Price.Count - a; x++)
			{
				//Debug.WriteLine(_trix[x]);
				_trix_2[x] = _trix[x];
			}


			//TRMA
			Core.Sma(0, Price.Count - a - 1, _trix_2, 11, out c, out d, _trma);

			for (int x = 0; x < Price.Count - c - a; x++)
			{
				Trix t = new Trix();
				t.TimeStamp = Price[x + a + c].TimeStamp;
				t.Price_Close = Price[x + a + c].Close;
				t.TRIX = _trix[x + c];
				t.TRMA = _trma[x];
				TRIX.Add(t);
				//Debug.WriteLine(Price[x+a+c].TimeStamp + "  TRIX : "+  _trix[x+c] + "  TRMA : " + _trma[x]);
			}


			return TRIX;
		}

		public static List<SlowStoch> createSlowStochastic(int Fast_K, int Slow_K, int Slow_D, List<Price> Price)
		{
			List<SlowStoch> SS = new List<SlowStoch>();

			double[] _close = new double[Price.Count];
			double[] _high = new double[Price.Count];
			double[] _low = new double[Price.Count];
			double[] _open = new double[Price.Count];

			double[] _ssK = new double[Price.Count];
			double[] _ssD = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_close[x] = Price[x].Close;
				_high[x] = Price[x].High;
				_low[x] = Price[x].Low;
				_open[x] = Price[x].Open;

			}

			int a, b;


			Core.Stoch(0, Price.Count - 1, _high, _low, _close, Fast_K, Slow_K, Core.MAType.Sma, Slow_D, Core.MAType.Sma, out a, out b, _ssK, _ssD);


			//Debug.WriteLine("SSk  " + _ssK.Count());
			//Debug.WriteLine("SSD  " + _ssD.Count());

			int zeroCount = 0;
			for (int x = 0; x < _ssK.Count(); x++)
			{
				if (_ssK[x] == 0) zeroCount++;
			}

			for (int x = 0; x < _ssK.Count() - zeroCount; x++)
			{
				//Debug.WriteLine("SSK " + _ssK[x]);
				//Debug.WriteLine("SSD " + _ssD[x]);
				//Debug.WriteLine(Price[x + zeroCount].TimeStamp + "   " + Price[x + zeroCount].Close);
				//Debug.WriteLine("===================");

				SlowStoch ss = new SlowStoch();
				ss.D = _ssD[x];
				ss.K = _ssK[x];
				ss.TimeStamp = Price[x + zeroCount].TimeStamp;
				ss.Fast_K = Fast_K;
				ss.Slow_D = Slow_D;
				ss.Slow_K = Slow_K;
				ss.Price_Close = Price[x + zeroCount].Close;
				ss.Price_High = Price[x + zeroCount].High;
				ss.Price_Low = Price[x + zeroCount].Low;
				ss.Price_Open = Price[x + zeroCount].Open;
				ss.InstrumentName = Price[x + zeroCount].InstrumentName;
				SS.Add(ss);
			}

			return SS;
		}

		public static List<Aroon> createAroon(int N, List<Price> Price)
		{
			List<Aroon> AROON = new List<Aroon>();

			double[] _close = new double[Price.Count];
			double[] _high = new double[Price.Count];
			double[] _low = new double[Price.Count];
			double[] _open = new double[Price.Count];

			double[] _aroonUP = new double[Price.Count];
			double[] _aroonDOWN = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_close[x] = Price[x].Close;
				_high[x] = Price[x].High;
				_low[x] = Price[x].Low;
				_open[x] = Price[x].Open;

			}

			int a, b;


			Core.Aroon(0, Price.Count - 1, _high, _low, N, out a, out b, _aroonDOWN, _aroonUP);





			for (int x = 0; x < _aroonUP.Count() - a; x++)
			{
				//Debug.WriteLine("UP " + _aroonUP[x]);
				//Debug.WriteLine("DOWN " + _aroonDOWN[x]);
				//Debug.WriteLine(Price[x + a].TimeStamp + "   " + Price[x + a].Close);
				//Debug.WriteLine("========" + x + "===========");
				int d = x + a;
				Aroon aa = new Aroon();
				aa.Aroon_Up = _aroonUP[x];
				aa.Aroon_Down = _aroonDOWN[x];
				aa.N = N;
				aa.TimeStamp = Price[d].TimeStamp;

				aa.Price_Close = Price[d].Close;
				aa.Price_High = Price[d].High;
				aa.Price_Low = Price[d].Low;
				aa.Price_Open = Price[d].Open;
				AROON.Add(aa);
			}







			return AROON;
		}

		public static List<BollingerBand> createBollingerBand(int N, double P, List<Price> Price, Core.MAType MovingAverageType)
		{
			List<BollingerBand> BB = new List<BollingerBand>();


			double[] _close = new double[Price.Count];
			double[] _high = new double[Price.Count];
			double[] _low = new double[Price.Count];
			double[] _open = new double[Price.Count];

			double[] _upperBand = new double[Price.Count];
			double[] _midBand = new double[Price.Count];
			double[] _lowerBand = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_close[x] = Price[x].Close;
				_high[x] = Price[x].High;
				_low[x] = Price[x].Low;
				_open[x] = Price[x].Open;

			}

			int a, b;


			Core.Bbands(0, Price.Count - 1, _close, N, P, P, MovingAverageType, out a, out b, _upperBand, _midBand, _lowerBand);


			for (int x = 0; x < _upperBand.Count() - a; x++)
			{
				//Debug.WriteLine("UPPER " + _upperBand[x]);
				//Debug.WriteLine("MID " + _midBand[x]);
				//Debug.WriteLine("LOWER " + _lowerBand[x]);
				//Debug.WriteLine(Price[x + a].TimeStamp + "   " + Price[x + a].Close);
				//Debug.WriteLine("========" + x + "===========");

				BollingerBand bb = new BollingerBand();
				bb.Mid = _midBand[x];
				bb.Lower = _lowerBand[x];
				bb.Upper = _upperBand[x];
				bb.N = N;
				bb.P = P;
				bb.TimeStamp = Price[x + a].TimeStamp;
				bb.Price_Close = Price[x + a].Close;
				bb.Price_High = Price[x + a].High;
				bb.Price_Low = Price[x + a].Low;
				bb.Price_Open = Price[x + a].Open;
				BB.Add(bb);
			}

			return BB;
		}

		public static List<EMA> createEMA(int N, List<Price> Price)
		{
			List<EMA> ema = new List<EMA>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;

			double[] _ema = new double[Price.Count];
			int a, b;

			Core.Ema(0, Price.Count - 1, _price, N, out a, out b, _ema);

			for (int x = 0; x < Price.Count - a; x++)
			{
				EMA e = new EMA
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					Ema = _ema[x],

				};
				ema.Add(e);
			}

			return ema;
		}

		public static List<EMA> createDEMA(int N, List<Price> Price)
		{
			List<EMA> dema = new List<EMA>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;

			double[] _ema = new double[Price.Count];
			int a, b;

			Core.Dema(0, Price.Count - 1, _price, N, out a, out b, _ema);
			for (int x = 0; x < Price.Count - a; x++)
			{
				EMA e = new EMA
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					Ema = _ema[x],

				};
				dema.Add(e);
			}

			return dema;
		}

		public static List<EMA> createEMA(int N, List<VariableIndicator> Value)
		{
			List<EMA> ema = new List<EMA>();

			double[] _price = new double[Value.Count];

			for (int x = 0; x < Value.Count; x++) _price[x] = Value[x].Value;

			double[] _ema = new double[Value.Count];
			int a, b;

			Core.Ema(0, Value.Count - 1, _price, N, out a, out b, _ema);

			for (int x = 0; x < Value.Count - a; x++)
			{
				EMA e = new EMA
				{
					TimeStamp = Value[x + a].TimeStamp,
					CustomValue = Value[x + a].Value,
					Ema = _ema[x],

				};
				ema.Add(e);
			}

			return ema;
		}

        public static List<EMA> createEMA(int N, List<double> Value)
        {
            List<EMA> ema = new List<EMA>();

            double[] _price = new double[Value.Count];

            for (int x = 0; x < Value.Count; x++) _price[x] = Value[x];

            double[] _ema = new double[Value.Count];
            int a, b;

            Core.Ema(0, Value.Count - 1, _price, N, out a, out b, _ema);

            for (int x = 0; x < Value.Count - a; x++)
            {
                EMA e = new EMA
                {                    
                    CustomValue = Value[x + a],
                    Ema = _ema[x],
                };
                ema.Add(e);
            }

            return ema;
        }

		public static List<SMA> createAdaptiveMA_KAMA(int N, List<VariableIndicator> Value)
		{
			List<SMA> sma = new List<SMA>();

			double[] _price = new double[Value.Count];

			for (int x = 0; x < Value.Count; x++) _price[x] = Value[x].Value;

			double[] _sma = new double[Value.Count];
			int a, b;


			Core.Kama(0, Value.Count - 1, _price, N, out a, out b, _sma);

			for (int x = 0; x < Value.Count - a; x++)
			{
				SMA e = new SMA
				{
					TimeStamp = Value[x + a].TimeStamp,
					CustomValue = Value[x + a].Value,
					Sma = _sma[x],

				};
				sma.Add(e);
			}

			return sma;
		}
		/// <summary>
		/// Adaptive Moving Average
		/// </summary>
		/// <param name="FastLimit">MAMA defualt = 0.05</param>
		/// <param name="SlowLimit">FAMA(Following Adaptive Moving Average) defualt=0.5</param>
		/// <param name="Var"></param>
		/// <returns></returns>
		public static List<MAMA> createAdaptiveMA_MAMA(double FastLimit, double SlowLimit, List<VariableIndicator> Var)
		{
			List<MAMA> mama = new List<MAMA>();

			double[] _price = new double[Var.Count];

			for (int x = 0; x < Var.Count; x++) _price[x] = Var[x].Value;

			double[] _mama = new double[Var.Count];
			double[] _fama = new double[Var.Count];
			int a, b;

			Core.Mama(0, Var.Count - 1, _price, FastLimit, SlowLimit, out a, out b, _mama, _fama);

			for (int x = 0; x < Var.Count - a; x++)
			{
				MAMA e = new MAMA
				{
					TimeStamp = Var[x + a].TimeStamp,
					Price_Close = Var[x + a].Price_Close,
					Price_High = Var[x + a].Price_High,
					Price_Low = Var[x + a].Price_Low,
					Price_Open = Var[x + a].Price_Open,
					Mama = _mama[x],
					Fama = _fama[x],

				};
				mama.Add(e);
			}

			return mama;
		}
		/// <summary>
		/// Adaptive Moving Average
		/// </summary>
		/// <param name="FastLimit">defualt = 0.5</param>
		/// <param name="SlowLimit">defualt=0.05</param>
		/// <param name="Var"></param>
		/// <returns></returns>
		public static List<MAMA> createAdaptiveMA_MAMA(double FastLimit, double SlowLimit, List<Price> Price)
		{
			List<MAMA> mama = new List<MAMA>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;

			double[] _mama = new double[Price.Count];
			double[] _fama = new double[Price.Count];
			int a, b;

			Core.Mama(0, Price.Count - 1, _price, FastLimit, SlowLimit, out a, out b, _mama, _fama);

			for (int x = 0; x < Price.Count - a; x++)
			{
				MAMA e = new MAMA
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					Mama = _mama[x],
					Fama = _fama[x],

				};
				mama.Add(e);
			}

			return mama;
		}

		public static List<SMA> createAdaptiveMA_KAMA(int N, List<Price> Price)
		{
			List<SMA> sma = new List<SMA>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;

			double[] _sma = new double[Price.Count];
			int a, b;

			Core.Kama(0, Price.Count - 1, _price, N, out a, out b, _sma);

			for (int x = 0; x < Price.Count - a; x++)
			{
				SMA e = new SMA
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					Sma = _sma[x],

				};
				sma.Add(e);
			}

			return sma;
		}

		public static List<SMA> createSMA(int N, List<Price> Price)
		{
			List<SMA> sma = new List<SMA>();

			double[] _price = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++) _price[x] = Price[x].Close;

			double[] _sma = new double[Price.Count];
			int a, b;

			Core.Sma(0, Price.Count - 1, _price, N, out a, out b, _sma);

			for (int x = 0; x < Price.Count - a; x++)
			{
				SMA e = new SMA
				{
					TimeStamp = Price[x + a].TimeStamp,
					Price_Close = Price[x + a].Close,
					Price_High = Price[x + a].High,
					Price_Low = Price[x + a].Low,
					Price_Open = Price[x + a].Open,
					Sma = _sma[x],

				};
				sma.Add(e);
			}

			return sma;
		}

		public static List<SMA> createSMA(int N, List<VariableIndicator> Value)
		{
			List<SMA> sma = new List<SMA>();

			double[] _price = new double[Value.Count];

			for (int x = 0; x < Value.Count; x++) _price[x] = Value[x].Value;

			double[] _sma = new double[Value.Count];
			int a, b;

			Core.Sma(0, Value.Count - 1, _price, N, out a, out b, _sma);

			for (int x = 0; x < Value.Count - a; x++)
			{
				SMA e = new SMA
				{
					TimeStamp = Value[x + a].TimeStamp,
					CustomValue = Value[x + a].Value,
					Sma = _sma[x],

				};
				sma.Add(e);
			}

			return sma;
		}

		public static List<StandardDev> creatStandardDeviation(double STDEV, int N, List<VariableIndicator> Value)
		{
			List<StandardDev> stdev = new List<StandardDev>();


			double[] _price = new double[Value.Count];

			for (int x = 0; x < Value.Count; x++) _price[x] = Value[x].Value;

			double[] _stdev = new double[Value.Count];
			int a, b;

			Core.StdDev(0, Value.Count - 1, _price, N, STDEV, out a, out b, _stdev);

			for (int x = 0; x < Value.Count - a; x++)
			{
				StandardDev e = new StandardDev
				{
					N = N,
					TimeStamp = Value[x + a].TimeStamp,
					CustomValue = Value[x + a].Value,
					StdDev = _stdev[x],
					SingleStdev = STDEV,
				};
				stdev.Add(e);
			}

			return stdev;
		}

		public static List<RegressionLine> createRegression(int N, List<VariableIndicator> Value)
		{
			List<RegressionLine> REG = new List<RegressionLine>();
			double[] _price = new double[Value.Count];

			for (int x = 0; x < Value.Count; x++) _price[x] = Value[x].Value;

			double[] _reg = new double[Value.Count];

			double[] _slope = new double[Value.Count];
			int a, b;


			Core.LinearReg(0, Value.Count - 1, _price, N, out a, out b, _reg);
			Core.LinearRegSlope(0, Value.Count - 1, _price, N, out a, out b, _slope);
			for (int x = 0; x < Value.Count - a; x++)
			{

				RegressionLine e = new RegressionLine
				{
					N = N,
					TimeStamp = Value[x + a].TimeStamp,
					CustomValue = Value[x + a].Value,
					Regression = _reg[x],
					Slope = _slope[x],
				};
				REG.Add(e);

			}

			return REG;

		}

		public static List<SAR> createSAR(double STEP, double MAXP, List<Price> Price)
		{
			List<SAR> SAR = new List<AlsiUtils.SAR>();


			double[] _close = new double[Price.Count];
			double[] _high = new double[Price.Count];
			double[] _low = new double[Price.Count];
			double[] _open = new double[Price.Count];


			double[] _sar = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_close[x] = Price[x].Close;
				_high[x] = Price[x].High;
				_low[x] = Price[x].Low;
				_open[x] = Price[x].Open;

			}

			int a, b;

			Core.Sar(0, Price.Count - 1, _high, _low, STEP, MAXP, out a, out b, _sar);
			// Core.SarExt(0, Price.Count - 1, _high, _low, 30000, 2, 0.02, 0.02, 2, 2, 0.02, 2, out a, out b, _sar);
			for (int x = 0; x < _sar.Count() - a; x++)
			{
				//Debug.WriteLine("SAR " + _sar[x]);
				//Debug.WriteLine(Price[x + a].TimeStamp + "   " + Price[x + a].Close);
				//Debug.WriteLine("========" + x + "===========");

				Debug.WriteLine(Price[x + a].Open + "," + Price[x + a].High + "," + Price[x + a].Low + "," + Price[x + a].Close + "," + _sar[x]);
				SAR ss = new AlsiUtils.SAR();
				ss.StopAndReverse = _sar[x];
				ss.TimeStamp = Price[x + a].TimeStamp;
				ss.Price_Close = Price[x + a].Close;
				ss.Price_High = Price[x + a].High;
				ss.Price_Low = Price[x + a].Low;
				ss.Price_Open = Price[x + a].Open;
				SAR.Add(ss);
			}

			return SAR;
		}

		public static List<ATR> createATR(int N, List<Price> Price)
		{
			List<ATR> ATR = new List<ATR>();


			double[] _close = new double[Price.Count];
			double[] _high = new double[Price.Count];
			double[] _low = new double[Price.Count];
			double[] _open = new double[Price.Count];


			double[] _atr = new double[Price.Count];

			for (int x = 0; x < Price.Count; x++)
			{
				_close[x] = Price[x].Close;
				_high[x] = Price[x].High;
				_low[x] = Price[x].Low;
				_open[x] = Price[x].Open;

			}

			int a, b;

			Core.Atr(0, Price.Count - 1, _high, _low, _close, N, out a, out b, _atr);
			for (int x = 0; x < _atr.Count() - a; x++)
			{
				//Debug.WriteLine("ATR " + _atr[x]);
				//Debug.WriteLine(Price[x + a].TimeStamp + "   " + Price[x + a].Close);
				//Debug.WriteLine("========" + x + "===========");

				//  Debug.WriteLine(Price[x + a].Open + "," + Price[x + a].High + "," + Price[x + a].Low + "," + Price[x + a].Close + "," + _atr[x]);
				ATR ss = new ATR();
				ss.AvgTrueRange = _atr[x];
				ss.TimeStamp = Price[x + a].TimeStamp;
				ss.Price_Close = Price[x + a].Close;
				ss.Price_High = Price[x + a].High;
				ss.Price_Low = Price[x + a].Low;
				ss.Price_Open = Price[x + a].Open;
				ATR.Add(ss);
			}

			return ATR;
		}
	}
}
