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
            var path= @"D:\RawAlgoOHLCV.csv";
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
            var T=Trades.Last();
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

        public static void CalcIndicators()
        {
            Console.WriteLine("Calculating Indicators...");

            //Base Level Calcs
             var ohlc=Trades.Select(z => z.OHLC);
             var OHLC_LIST = ohlc.ToList();           
            var volume = Trades.ToDictionary(x => x.TimeStamp, x=>x.TradeVolume);
            var trigger=Trades.ToDictionary(x=>x.TimeStamp,x=>x.TradeTrigger);
            var EM = AlsiUtils.Factory_Indicator.createAdaptiveMA_MAMA(0.05, 0.05, OHLC_LIST).ToDictionary(x => x.TimeStamp, x => x.Mama);
            var adx_DMI = AlsiUtils.Factory_Indicator.createADX(75, OHLC_LIST);
            var RSI = AlsiUtils.Factory_Indicator.createRSI(20, OHLC_LIST);


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
            
            var Dmi_Up_EMA = Factory_Indicator.createEMA(12,varDmiUP);
            var Dmi_Down_EMA = Factory_Indicator.createEMA(12,varDmiDOWN);
            
            //EMA OF RSI
            List<VariableIndicator> varRsi = new List<VariableIndicator>();
            foreach (var r in RSI)
            {
                var rsi = new VariableIndicator()
                {
                    TimeStamp=r.TimeStamp,
                    Value=r.RSI,
                };
                varRsi.Add(rsi);
            }

            var RSI_EMA = Factory_Indicator.createEMA (25, varRsi);


            //CREATE DICTIONARY 
            var DMI_UP_DIC=Dmi_Up_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
            var DMI_Down_DIC = Dmi_Down_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
            var RSI_DIC = RSI_EMA.ToDictionary(x => x.TimeStamp, x => x.Ema);
         



            var sw = new StreamWriter(@"d:\Indicators.csv");
            sw.WriteLine("Trade" + "," + "Stamp" + "," + "PriceClose" + "," + "MAMA" + "," + "DI-UP" + "," + "DI-DOWN" + "," + "VOL" + "," + "RSI" + "," + "extra 2" + "," + "extra 3" + "," + "extra 4");
            foreach (var r in adx_DMI)
            {
                double em =0;
                EM.TryGetValue(r.TimeStamp,out em);

                double rs = 0;
                RSI_DIC.TryGetValue(r.TimeStamp, out rs);

                int vol = 0;
                volume.TryGetValue(r.TimeStamp,out vol);

                double DI_UP=0;
                DMI_UP_DIC.TryGetValue(r.TimeStamp, out DI_UP);

                double DI_DOWN=0;
                DMI_Down_DIC.TryGetValue(r.TimeStamp, out DI_DOWN);

                Trade.Trigger Trig;
                trigger.TryGetValue(r.TimeStamp,out Trig );


                var topband = DI_UP;
                var midband = em;
                var lowband = DI_DOWN;

                var b1 = rs;
                var adjVol = (vol == null) ? 0 : vol;
                var b2 = 0;
                var b3 = 0;
                var b4 = 0;
                var trade = Trig;

                sw.WriteLine(trade + "," + r.TimeStamp + "," + r.Price_Close + "," + midband + "," + topband + "," + lowband + "," + adjVol + "," + b1 + "," + b2 + "," + b3 + "," + b4);
            }
            sw.Close();
            Console.Write("Done");

        }
    }

}
