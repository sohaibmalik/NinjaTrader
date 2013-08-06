using AlsiUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class TrailingStop
    {
        private List<TrailTrade> _Trades = new List<TrailTrade>();

        public TrailingStop()
        {
            Algo.LoadPrice();
            foreach (var t in Algo.Trades)
            {
                _Trades.Add(TrailTrade.GetTrailTrade(t));

            }
        }

        public void SetTriggers(int triggerPTS_Start, int triggerPTS_Stop, int trailSize_Start, int trailSize_Stop)
        {
            var sr = new StreamWriter(@"D:\Trailingstop");
            for (int pts = triggerPTS_Start; pts < triggerPTS_Stop; pts += 5)
                for (int sz = trailSize_Start; sz < trailSize_Stop; sz += 5)
                    sr.WriteLine(TrailTrade.SetTrailTrigger(pts, sz, _Trades));

            sr.Close();
            Console.WriteLine("COMPLETE");
        }

        public void SetTriggers()
        {
            TrailTrade.SetTrailTrigger(100, 50, _Trades);     
        }

       

    }

    public class TrailTrade : Trade
    {
        public bool TrailTriggered { get; set; }
        public double TrailProfit { get; set; }

        public static TrailTrade GetTrailTrade(Trade trade)
        {
            var tl = new TrailTrade();
            CopyProperties(tl, trade);
            return tl;
        }

        static void CopyProperties(object dest, object src)
        {
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                item.SetValue(dest, item.GetValue(src));
            }
        }

        public static string SetTrailTrigger(int triggerPTS, int trailSize, List<TrailTrade> Trades)
        {

            int Li = int.Parse(Trades.Last().Notes);
            double totalProfDiff = 0;

            double triggerlevel = 0;
            bool tt = false;
            bool tookprofit = false;
            double trailProfit = 0;
            double thistradeDiff = 0;

            for (int i = 0; i < Li; i++)
            {

                triggerlevel = 0;
                tt = false;
                tookprofit = false;
                trailProfit = 0;


                foreach (var t in Trades.Where(x => x.Notes == i.ToString()))
                {
                    if (t.OHLC.Close >= triggerPTS)
                    {
                        if (triggerlevel <= t.OHLC.Close) triggerlevel = t.OHLC.Close;
                        tt = true;
                    }
                    t.TrailTriggered = tt;

                    if (!tookprofit && t.OHLC.Close < (triggerlevel - trailSize) && (triggerlevel - trailSize)>0)
                    {
                        tookprofit = true;
                        t.TrailProfit = t.OHLC.Close;
                        trailProfit = t.OHLC.Close;

                    }

                    thistradeDiff = trailProfit - t.OHLC.Close;
                    if (!tt) thistradeDiff = 0;
                    //Console.WriteLine("Trade: {0} Current PL {1} trig {2} Tl {3} SL {4} LP {5} diff {6}",
                    //    t.Notes, t.OHLC.Close, tt, triggerlevel, (triggerlevel - trailSize), trailProfit, thistradeDiff);
                   
                }
                if (trailProfit != 0) totalProfDiff += thistradeDiff;
                thistradeDiff = 0;
                //Console.WriteLine("TOTAL : " + totalProfDiff);
                //Console.ReadLine();
            }
            var st = ("TOTAL : " + totalProfDiff + "  triggerPTS " + triggerPTS + "  trailSize " + trailSize);
            Console.WriteLine(st);
            return st;

        }

    }
}
