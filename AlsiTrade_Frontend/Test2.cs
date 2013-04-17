using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiUtils.Data_Objects;
using AlsiUtils;
using AlsiUtils.Strategies;
using System.IO;
using System.Diagnostics;

namespace FrontEnd
{
    public partial class Test2 : Form
    {
        private Statistics _Stats = new Statistics();
        private List<Trade> _FullTradeList;
        private List<CompletedTrade> NewTrades;
        private List<Trade> _TradeOnlyList;

        private static double TPF;
        private static double SLF;

        public Test2()
        {
            InitializeComponent();
        }

        private void Test2_Load(object sender, EventArgs e)
        {

           

         
            start();

            var tp = new TakeProfit(_FullTradeList, NewTrades);
            tp.Calculate();
            Close();
        }

        List<int> Per = new List<int>();
        List<int> TP = new List<int>();
        private void GetParams()
        {


            using (StreamReader reader = new StreamReader(@"d:\testdata2.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    TP.Add(int.Parse(parts[1]));
                    Per.Add(int.Parse(parts[3]));
                    // Debug.WriteLine(line); // Write to console.
                }
            }

        }


        private void start()
        {
            Cursor = Cursors.WaitCursor;
            GlobalObjects.TimeInterval t = GlobalObjects.TimeInterval.Minute_5;
						DataBase.dataTable dt = DataBase.dataTable.MasterMinute;
            //_FullTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(GetParametersSAR_EMA(), t, false, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), dt);
            _FullTradeList = AlsiTrade_Backend.RunCalcs.RunMAMAScalp(GetParametersMAMA(), t, false, new DateTime(2010, 02, 02), new DateTime(2014, 04, 15), dt);
            _FullTradeList = _Stats.CalcBasicTradeStats_old(_FullTradeList);
            NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(11, _FullTradeList);
            NewTrades = _Stats.CalcExpandedTradeStats(NewTrades);
            _TradeOnlyList = CompletedTrade.CreateList(NewTrades);
            Cursor = Cursors.Default;

        }

        public static Parameter_MAMA GetParametersMAMA()
        {
            AlsiUtils.Strategies.Parameter_MAMA E = new AlsiUtils.Strategies.Parameter_MAMA()
            {


                //A_EMA1 = WebSettings.Indicators.EmaScalp.A1,
                //A_EMA2 = WebSettings.Indicators.EmaScalp.A2,
                //B_EMA1 = WebSettings.Indicators.EmaScalp.B1,
                //B_EMA2 = WebSettings.Indicators.EmaScalp.B2,
                //C_EMA = WebSettings.Indicators.EmaScalp.C1,
                //TakeProfit = WebSettings.General.TAKE_PROFIT,
                //StopLoss = WebSettings.General.STOPLOSS,
                //CloseEndofDay = false,

                Fast = 0.5,//0.1
                Slow = 0.05,//0.01

                A_EMA1 = 16,
                A_EMA2 = 17,
                B_EMA1 = 43,
                B_EMA2 = 45,
                C_EMA = 52,
                TakeProfit = 450,
                StopLoss = -350,
                TakeProfitFactor = TPF,
                StoplossFactor = SLF,
                CloseEndofDay = false,
            };

            return E;
        }
    }
}
