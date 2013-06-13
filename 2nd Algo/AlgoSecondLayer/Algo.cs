using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsiUtils;
using System.IO;
using System.Diagnostics;

namespace AlgoSecondLayer
{
    public class Algo
    {
        private static List<Trade> Trades = new List<Trade>();
        public static void LoadPrice()
        {

            var path = @"D:\RawAlgoOHLCV.csv";
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    var s = sr.ReadLine().Split(',');

                    var d = DateTime.Parse(s[0]);
                    var t = DateTime.Parse(s[1]);
                    var stamp = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 00);
                    var trade = new Trade()
                    {
                        TimeStamp = stamp,
                        OHLC = new Price(stamp, double.Parse(s[2]), double.Parse(s[3]), double.Parse(s[4]), double.Parse(s[5]), "Test"),
                        TradeVolume = int.Parse(s[6]),
                        TradeTrigger = GetTrigger(s[7]),
                    };
                    Trades.Add(trade);
                }
            }
            var T = Trades.Last();
            Console.WriteLine(Trades.Count + " prices imported");
            Console.WriteLine("Last trade : " + T.TimeStamp + "\nOpen - " + T.OHLC.Open + "\nHigh - " + T.OHLC.High + "\nLow - " + T.OHLC.Low + "\nClose - " + T.OHLC.Close + "\nVol - " + T.TradeVolume + "\nTrigger - " + T.TradeTrigger);
            Console.WriteLine("\n");
        }
        private static Trade.Trigger GetTrigger(string trigger)
        {
            switch (trigger)
            {
                case "None": return Trade.Trigger.None;
                case "OpenLong": return Trade.Trigger.OpenLong;
                case "OpenShort": return Trade.Trigger.OpenShort;
                case "CloseLong": return Trade.Trigger.CloseLong;
                case "CloseShort": return Trade.Trigger.CloseShort;
                default: return Trade.Trigger.None;


            }
        }

        public static Parameters P = new Parameters();

        public static List<IndicatorBasket> CalcIndicators()
        {
         //   Console.WriteLine("Calculating Indicators...");
            var Indicators = new List<IndicatorBasket>();

            //Base Level Calcs
            var ohlc = Trades.Select(z => z.OHLC);
            var OHLC_LIST = ohlc.ToList();
            var volume = Trades.ToDictionary(x => x.TimeStamp, x => x.TradeVolume);
            var trigger = Trades.ToDictionary(x => x.TimeStamp, x => x.TradeTrigger);
            var EM = AlsiUtils.Factory_Indicator.createAdaptiveMA_MAMA(P.Mamapar1, P.Mamapar2, OHLC_LIST).ToDictionary(x => x.TimeStamp, x => x.Mama);
            var adx_DMI = AlsiUtils.Factory_Indicator.createADX(P.ADX_period, OHLC_LIST);
            var RSI = AlsiUtils.Factory_Indicator.createRSI(P.RSI_period, OHLC_LIST);


            //Second Level Clacs
            //EMA OF DMI+ and DMI-
            List<VariableIndicator> varDmiUP = new List<VariableIndicator>();
            List<VariableIndicator> varDmiDOWN = new List<VariableIndicator>();
            foreach (var f in adx_DMI)
            {
                var vdown = new VariableIndicator()
                    {
                        TimeStamp = f.TimeStamp,
                        Value = f.DI_Down,
                    };
                var vup = new VariableIndicator()
                {
                    TimeStamp = f.TimeStamp,
                    Value = f.DI_Up,
                };

                varDmiUP.Add(vup);
                varDmiDOWN.Add(vdown);
            }

            var Dmi_Up_EMA = Factory_Indicator.createEMA(P.Di_UP_avg_period, varDmiUP);
            var Dmi_Down_EMA = Factory_Indicator.createEMA(P.Di_DOWN_avg_period, varDmiDOWN);

            //EMA OF RSI
            List<VariableIndicator> varRsi = new List<VariableIndicator>();
            foreach (var r in RSI)
            {
                var rsi = new VariableIndicator()
                {
                    TimeStamp = r.TimeStamp,
                    Value = r.RSI,
                };
                varRsi.Add(rsi);
            }

            var RSI_EMA = Factory_Indicator.createEMA(P.RSI_EMA, varRsi);

            //slope OF RSI
            var rsi_slope = Factory_Indicator.createRegression(P.RSI_Slope_period, varRsi);

            //CREATE DICTIONARY 
            var DMI_UP_DIC = Dmi_Up_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
            var DMI_Down_DIC = Dmi_Down_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
            var RSI_DIC = RSI_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
            var RSI_SLOPE = rsi_slope.ToDictionary(x => x.TimeStamp, x => x.Slope);



          //  var sw = new StreamWriter(@"d:\Indicators.csv");
           // sw.WriteLine("Trade" + "," + "Stamp" + "," + "PriceClose" + "," + "MAMA" + "," + "DI-UP" + "," + "DI-DOWN" + "," + "VOL" + "," + "RSI" + "," + "RSI SLOPE" + "," + "extra 3" + "," + "extra 4");
            foreach (var r in adx_DMI)
            {
                double em = 0;
                EM.TryGetValue(r.TimeStamp, out em);

                double rs = 0;
                RSI_DIC.TryGetValue(r.TimeStamp, out rs);

                double rsslope = 0;
                RSI_SLOPE.TryGetValue(r.TimeStamp, out rsslope);

                int vol = 0;
                volume.TryGetValue(r.TimeStamp, out vol);

                double DI_up = 0;
                DMI_UP_DIC.TryGetValue(r.TimeStamp, out DI_up);

                double DI_Down = 0;
                DMI_Down_DIC.TryGetValue(r.TimeStamp, out DI_Down);

                Trade.Trigger Trig;
                trigger.TryGetValue(r.TimeStamp, out Trig);



                var topband = DI_up;
                var midband = em;
                var lowband = DI_Down;

                var b1 = rs;
                var adjVol = (vol == null) ? 0 : vol;
                var b2 = rsslope; ;
                var b3 = 0;
                var b4 = 0;
                var trade = Trig;

                var i = new IndicatorBasket()
                {
                    Action = trade,
                    Timestamp = r.TimeStamp,
                    DI_DOWN = DI_Down,
                    DI_UP = DI_up,
                    Mama = em,
                    RSI = rs,
                    RSI_SLOPE = rsslope,
                    Price = r.Price_Close,
                    Volume = adjVol,

                };
                Indicators.Add(i);
             //   sw.WriteLine(trade + "," + r.TimeStamp + "," + r.Price_Close + "," + midband + "," + topband + "," + lowband + "," + adjVol + "," + b1 + "," + b2 + "," + b3 + "," + b4);
            }
         //   sw.Close();
           
            return Indicators;
        }

        public static double CalculatePL(List<IndicatorBasket> I)
        {
           // Console.WriteLine("Calculating Profit and Loss");
            var S = I[0];
            S.Direction = NORMALorREVERSE.Normal;
            S.Position = INorOUT.In;

          //  var sw = new StreamWriter(@"d:\PLoutput.csv");

            for (int x = 1; x < I.Count; x++)
            {
                I[x].BottomUpTurn = (I[x].RSI < P.RSI_BOTTOM
                    && I[x - 1].RSI_SLOPE > I[x].RSI_SLOPE
                    && I[x-1].RSI_SLOPE > 0
                    && I[x].RSI_SLOPE<0);

                I[x].TopDownTurn = (I[x].RSI > P.RSI_TOP
                    && I[x - 1].RSI_SLOPE < I[x].RSI_SLOPE
                     && I[x].RSI_SLOPE > 0
                     && I[x - 1].RSI_SLOPE < 0);


                I[x].enterRevereseRAW = (I[x].Price < I[x].Mama
                    && I[x].DI_DOWN > I[x].DI_UP
                    && I[x].RSI < 45);


                I[x].exitReverseRAW = (I[x].Price > I[x].Mama
                     || I[x].DI_DOWN < I[x].DI_UP
                     || I[x].BottomUpTurn);
                               

                I[x].EnterReverseTrigger = (I[x].enterRevereseRAW && !I[x - 1].enterRevereseRAW);
                I[x].ExitReverseTrigger = (I[x].exitReverseRAW && !I[x - 1].exitReverseRAW);
                I[x].StopTrigger = I[x].TopDownTurn;

                //Direction
                if (I[x].EnterReverseTrigger) I[x].Direction = NORMALorREVERSE.Reverse;
                else
                    if (I[x].ExitReverseTrigger) I[x].Direction = NORMALorREVERSE.Normal;
                    else 
                        I[x].Direction = I[x - 1].Direction;

                //Position
                if (I[x].StopTrigger) I[x].Position = INorOUT.Out;
                else
                    if (I[x].Action == Trade.Trigger.OpenLong
                        || I[x].Action == Trade.Trigger.OpenShort)
                        I[x].Position = INorOUT.In;
                    else
                        I[x].Position = I[x - 1].Position;


                var ProfitChange = I[x].Price - I[x - 1].Price;
                var InOutMultiplier = (I[x].Position == INorOUT.In) ? 1 : 0;
                var NormalReverseMultiplier = (I[x].Direction == NORMALorREVERSE.Normal) ? 1 : -1;
                var InOut_NormalReverse = InOutMultiplier * NormalReverseMultiplier * ProfitChange;

                //what the fuck multiplier ????
               var WTF_Multiplier = (I[x].Direction != I[x - 1].Direction) ? -1 : 1;

                //Profit

                I[x].Profit = WTF_Multiplier * InOut_NormalReverse;
                I[x].RunningProfit = I[x].Profit + I[x - 1].RunningProfit;

                
               // sw.WriteLine(I[x].Timestamp.ToShortDateString() + "      This Profit " +I[x].Profit + "    " + I[x].RunningProfit + "   " + I[x].Position);
            //    sw.WriteLine(I[x].Timestamp + "," + I[x].Profit + "," + I[x].Price + "," + I[x].RunningProfit + "," + I[x].Position + "," + I[x].Direction + "," + I[x].BottomUpTurn);
            }
         //   sw.Close();

            return I.Last().RunningProfit;
        }

        public class IndicatorBasket
        {
            public DateTime Timestamp { get; set; }
            public double Price { get; set; }
            public int Volume { get; set; }
            public double Profit { get; set; }
            public double RunningProfit { get; set; }
            public Trade.Trigger Action { get; set; }
            public double Mama { get; set; }
            public double DI_UP { get; set; }
            public double DI_DOWN { get; set; }
            public double RSI { get; set; }
            public double RSI_SLOPE { get; set; }
            public bool TopDownTurn { get; set; }
            public bool BottomUpTurn { get; set; }
            public bool exitReverseRAW { get; set; }
            public bool enterRevereseRAW { get; set; }
            public bool stopTriggerRAW { get; set; }
            public bool EnterReverseTrigger { get; set; }
            public bool ExitReverseTrigger { get; set; }
            public bool StopTrigger { get; set; }
            public NORMALorREVERSE Direction { get; set; }
            public INorOUT Position { get; set; }
        }

        public class Parameters
        {
            public double Mamapar1 { get; set; } //0.05
            public double Mamapar2 { get; set; }//0.05
            public int RSI_period{get;set;}//20
            public int RSI_EMA { get; set; }//25
            public int RSI_Slope_period { get; set; }//5
            public int RSI_TOP { get; set; }//75
            public int RSI_BOTTOM { get; set; }//35
            public int ADX_period{ get; set; }//75
            public int Di_DOWN_avg_period { get; set; }//12
            public int Di_UP_avg_period { get; set; }//12

        }

        public enum NORMALorREVERSE
        {
            Normal,
            Reverse,
        }
        public enum INorOUT
        {
            In,
            Out,
        }
    }

}
