using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using AlsiUtils.Data_Objects;
using AlsiUtils;

namespace NinjaTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunMultiple();


        }

        private static void RunMultiple()
        {

            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            DateTime s = new DateTime(2012, 01, 02);
            DateTime end = new DateTime(2012, 12, 15);




            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, end, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            for (int x = 21; x <= 24; x++)
            {
                for (int y = 8; y <= 28; y++)
                {

                    for (int a = 27; a <= 50; a++)
                    {

                        for (int b = 8; b <= 50; b++)
                        {

                            for (int z = 20; z <= 70; z++)
                            {

                                if (x < y && y < a && a < b && b < z)
                                {
                                    AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                                    {
                                        A_EMA1 = x,
                                        A_EMA2 = y,
                                        B_EMA1 = a,
                                        B_EMA2 = b,
                                        C_EMA = z,
                                        TakeProfit = 250,
                                        StopLoss = -250,
                                        CloseEndofDay = false,
                                        Period = 2012,
                                    };


                                    AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);
                                }
                            }
                        }
                    }
                }
            }
        }



        private static void RunSingle()
        {
            //Laptop
            // string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;



            DateTime s = new DateTime(2006, 01, 01);
            DateTime e = new DateTime(2012, 12, 15);


            var prices = AlsiUtils.DataBase.readDataFromDataBase(GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.AllHistory,
               s, e, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            //for (int x = 50; x < 500; x+=50)
            //{
            //    for (int y = 50; y < 500; y+=50)
            //    {
                    AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                    {
                        A_EMA1 = 16,
                        A_EMA2 = 17,
                        B_EMA1 = 43,
                        B_EMA2 = 45,
                        C_EMA = 52,
                        TakeProfit = 450,
                        StopLoss = -300,
                        CloseEndofDay = false,
                        Period = prices.Count,
                       
                    };
                    takep = E.TakeProfit;
                    takel = E.StopLoss;
                    AlsiUtils.Statistics S = new AlsiUtils.Statistics();            
                    S.OnStatsCaculated += new AlsiUtils.Statistics.StatsCalculated(S_OnStatsCaculated);
                    var Trades = AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices, false);

                    Trades = S.CalcBasicTradeStats(Trades);
                    var NewTrades = AlsiUtils.Strategies.TradeStrategy.Expansion.ApplyRegressionFilter(10,Trades);
                    Trades = AlsiUtils.Strategies.TradeStrategy.Expansion.MergeNewTrades(Trades, NewTrades);

                    PrintTradesonly(Trades);
                //}
            //}
        }

      private  static double maxprof = 0;
      private static int takep = 0;
        private static int takel=0;
        static void S_OnStatsCaculated(object sender, AlsiUtils.Statistics.StatsCalculatedEvent e)
        {
            if (e.SumStats.TotalProfit > maxprof)
            {
                maxprof = e.SumStats.TotalProfit;


                Debug.WriteLine("============STATS==============");
                Debug.WriteLine("Total PL " + e.SumStats.TotalProfit);
                Debug.WriteLine("# Trades " + e.SumStats.TradeCount);
                Debug.WriteLine("Tot Avg PL " + e.SumStats.Total_Avg_PL);
                Debug.WriteLine("Prof % " + e.SumStats.Pct_Prof);
                Debug.WriteLine("Loss % " + e.SumStats.Pct_Loss);
                Debug.WriteLine("Take P " + takep);
                Debug.WriteLine("Take L " + takel);
            }
        }

        private static void PrintTradesonly(List<Trade>Trades)
        {
            DateTime s = new DateTime(2006, 01, 12);
            DateTime e = new DateTime(2006, 02, 01);

            foreach (var t in Trades)
            {
                if(t.TimeStamp>s && t.TimeStamp<e)
                Debug.WriteLine(t.TimeStamp + "," 
                    + t.TradedPrice + "," 
                    + t.TotalPL + "," 
                    + t.Extention.Regression + "," 
                    + t.Extention.Slope + "," 
                    + t.Extention.OrderVol
                    );
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunSingle();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            RunSingle();
            Close();
        }


    }

}