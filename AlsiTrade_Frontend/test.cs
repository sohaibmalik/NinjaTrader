using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Communicator;
using System.Diagnostics;
using AlsiUtils.Strategies;
using AlsiUtils;
using AlsiUtils.Data_Objects;

namespace FrontEnd
{
    public partial class Test : Form
    {

        private Statistics _Stats = new Statistics();
        private List<Trade> _FullTradeList;
        private List<CompletedTrade> NewTrades;
        private List<Trade> _TradeOnlyList;


        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            AlsiUtils.DataBase.SetConnectionString(@"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True");
          //  WebSettings.GetSettings();
            start();

            //foreach (var t in _FullTradeList.Where(t=>t.TimeStamp.Day==22))
            //{
            //    Debug.WriteLine(t.TimeStamp + "  " + "TradedPrice " + t.TradedPrice +"  CurrntPrice " + t.CurrentPrice + "    "   +  t.CurrentDirection  +"   " +t.RunningProfit);
            //}


            //var sumstats = Statistics.SummaryProfitLoss(_FullTradeList, Period.Monthly);
            //foreach (var s in sumstats)
            //    Debug.WriteLine(s.Year + " / " + s.Month + "  = " + s.Sum);


            ExitTakeProf();
        }

        private void ExitTakeProf()
        {
            double max = 0;
            int MD = 0;
            int MSK = 0;
            int MFK = 0;

            for (int H = 95; H > 55; H--)
            for (int L = 5; L < 45;L++ )
                for (int D = 3; D < 30; D += 1)
                    for (int SK = 3; SK < 30; SK += 1)
                        for (int FK = 3; FK < 30; FK += 1)
                        {
                            var TempList = new List<Trade>();
                            foreach (var t in _FullTradeList)
                            {
                                Trade newTrade = (Trade)t.Clone();
                                TempList.Add(newTrade);
                            }


                            double pl = 0;
                            var Ex = Statistics.TakeProfit_Exiguous_SlowStoch(_FullTradeList, FK, SK, D,H, L);
                            // var Ex = Statistics.TakeProfit_Exiguous_GoldenBoil(TempList , n, stdev, 4);
                            // var Ex = Statistics.TakeProfit_Exiguous(TempList, n, -1000);
                            foreach (var v in Ex)
                            {
                                pl += v.CloseTrade.RunningProfit;

                                // Debug.WriteLine("Open " + v.OpenTrade.TimeStamp + "  " + v.OpenTrade.Reason + "  " + v.OpenTrade.TradedPrice  + "  " + v.OpenTrade.Position  + "  " + v.OpenTrade.RunningProfit );
                                //  Debug.WriteLine("Close " + v.CloseTrade.TimeStamp + "  " + v.CloseTrade.Reason + "  " + v.CloseTrade.TradedPrice + "  " + v.CloseTrade.Position + "  " + v.CloseTrade.RunningProfit + "     " + pl);
                            }
                            //  Debug.WriteLine("TOTAL PROFIT : P(" + n + ") stdev("+stop+")  = " + pl);

                            if (pl > max)
                            {
                                max = pl;
                                MSK = SK;
                                MFK = FK;
                                MD = D;

                                Debug.WriteLine("TOTAL PROFIT : (" + FK + " " + SK + "  " + D + ")   = " + pl + "       MAX " + max + "(" + MFK + " " + MSK + "  " + MD + " L " +L + " H " + H+")");
                            }
                        }
        }

        private void printOHLC()
        {
            var OHLC = Statistics.IntratradeToCandle(_FullTradeList);
            foreach (var v in OHLC)
            {
                //  Debug.WriteLine("Open " + v.OpenTrade.TimeStamp + "  " + v.OpenTrade.OHLC.Open + "  " + v.OpenTrade.OHLC.High + "  " + v.OpenTrade.OHLC.Low + "  " + v.OpenTrade.OHLC.Close);
                Debug.WriteLine("Close " + v.CloseTrade.TimeStamp + "  " + v.CloseTrade.OHLC.Open + "  " + v.CloseTrade.OHLC.High + "  " + v.CloseTrade.OHLC.Low + "  " + v.CloseTrade.OHLC.Close);
            }
        }


        public static Parameter_EMA_SAR  GetParametersSAR_EMA()
        {
            AlsiUtils.Strategies.Parameter_EMA_SAR E = new AlsiUtils.Strategies.Parameter_EMA_SAR()
            {                
                SAR_STEP=0.02,
                SAR_MAXP=0.2,
                
                //A_EMA1 = WebSettings.Indicators.EmaScalp.A1,
                //A_EMA2 = WebSettings.Indicators.EmaScalp.A2,
                //B_EMA1 = WebSettings.Indicators.EmaScalp.B1,
                //B_EMA2 = WebSettings.Indicators.EmaScalp.B2,
                //C_EMA = WebSettings.Indicators.EmaScalp.C1,
                //TakeProfit = WebSettings.General.TAKE_PROFIT,
                //StopLoss = WebSettings.General.STOPLOSS,
                //CloseEndofDay = false,


                A_EMA1 = 16,
                A_EMA2 = 17,
                B_EMA1 = 43,
                B_EMA2 = 45,
                C_EMA = 52,
                TakeProfit = 450,
                StopLoss = -350,
                CloseEndofDay = false,
            };

            return E;
        }

        private void start()
        {
            Cursor = Cursors.WaitCursor;
            GlobalObjects.TimeInterval t = GlobalObjects.TimeInterval.Minute_5;
            DataBase.dataTable dt = DataBase.dataTable.MasterMinute;
          //  _FullTradeList = AlsiTrade_Backend.RunCalcs.RunEMASAR(GetParametersSAR_EMA(), t, false, new DateTime(2013, 2, 20), new DateTime(2013, 03, 27), dt);
            _FullTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(GetParametersSAR_EMA(), t, false, new DateTime(2013, 1, 01), new DateTime(2013, 03, 27), dt);
            _FullTradeList = _Stats.CalcBasicTradeStats_old(_FullTradeList);
            NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(11, _FullTradeList);
            NewTrades = _Stats.CalcExpandedTradeStats(NewTrades);
            _TradeOnlyList = CompletedTrade.CreateList(NewTrades);
            Cursor = Cursors.Default;

        }


       

    }
}
