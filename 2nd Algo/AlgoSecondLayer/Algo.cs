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
             var ohlc=Trades.Select(z => z.OHLC);
             var OHLC_LIST = ohlc.ToList();           
            var volume = ohlc.ToDictionary(x => x.TimeStamp, x => x.Volume);
            var trigger=Trades.ToDictionary(x=>x.TimeStamp,x=>x.TradeTrigger);
            var EM = AlsiUtils.Factory_Indicator.createAdaptiveMA_MAMA(0.01, 0.01, OHLC_LIST).ToDictionary(x => x.TimeStamp, x => x.Mama);

            var adx_DMI = AlsiUtils.Factory_Indicator.createADX(75, OHLC_LIST);
            var DMI_UP=adx_DMI.ToDictionary(x => x.TimeStamp, x => x.DI_Up);
            var DMI_Down = adx_DMI.ToDictionary(x => x.TimeStamp, x => x.DI_Down);
           
            var RSI = AlsiUtils.Factory_Indicator.createRSI(20, OHLC_LIST).ToDictionary(x=>x.TimeStamp,x=>x.RSI);

            var sw = new StreamWriter(@"d:\Indicators.csv");
            sw.WriteLine("Trade" + "," + "Stamp" + "," + "PriceClose" + "," + "MAMA" + "," + "DI-UP" + "," + "DI-DOWN" + "," + "VOL" + "," + "extra 1" + "," + "extra 2" + "," + "extra 3" + "," + "extra 4");
            foreach (var r in adx_DMI)
            {
                double em =0;
                EM.TryGetValue(r.TimeStamp,out em);

                double rs = 0;
                RSI.TryGetValue(r.TimeStamp, out rs);

                double vol = 0;
                volume.TryGetValue(r.TimeStamp,out vol);

                double DI_UP=0;
                DMI_UP.TryGetValue(r.TimeStamp,out DI_UP);

                double DI_DOWN=0;
                DMI_Down.TryGetValue(r.TimeStamp,out DI_DOWN);

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
