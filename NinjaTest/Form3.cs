using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


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
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            //for (int per = 10000; per < 300000; per += 10000)
            //{
            //    int period = per;

            //    var prices = AlsiUtils.DataBase.readDataFromDataBase_2_MIN_AllHistory(period, false);
            //    Debug.WriteLine("Start Date " + prices[0].TimeStamp);
               

            //    //for (int x = 8; x <= 30; x++)
            //    //{
            //    //    for (int y = 8; y <= 40; y++)
            //    //    {
            //    //        for (int z = 20; z <= 70; z++)
            //    //        {

            //    //            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            //    //            {
            //    //                A_EMA1 = x,
            //    //                A_EMA3 = 1,
            //    //                A_EMA4 = 1,
            //    //                A_EMA5 = 1,
            //    //                A_EMA6 = x + y,
            //    //                B_EMA1 = x + y + x,
            //    //                B_EMA2 = 1,
            //    //                B_EMA3 = 1,
            //    //                B_EMA4 = 1,
            //    //                B_EMA5 = 1,
            //    //                B_EMA6 = x + y + x + y,
            //    //                C_EMA = z,
            //    //                TakeProfit = 250,
            //    //                StopLoss = -250,
            //    //                CloseEndofDay = true,
            //    //                Period = per,
            //    //            };


            //    //            AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices);
            //    //        }
            //    //    }
            //    //}
            //}

        }



        private static void RunSingle()
        {
              //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;


            DateTime s = new DateTime(2007, 01, 01);
            DateTime e = new DateTime(2012, 12, 15);


            var prices = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.DataBase.timeframe.minute_5, AlsiUtils.DataBase.dataTable.AllHistory,
               s, e, false);
                Debug.WriteLine("Start Date " + prices[0].TimeStamp);
              
                    AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                    {
                        A_EMA1 = 24,
                        A_EMA3 = 1,
                        A_EMA4 = 1,
                        A_EMA5 = 1,
                        A_EMA6 = 44,
                        B_EMA1 = 70,
                        B_EMA2 = 1,
                        B_EMA3 = 1,
                        B_EMA4 = 1,
                        B_EMA5 = 1,
                        B_EMA6 = 68,
                        C_EMA = 88,
                        TakeProfit = 500,
                        StopLoss = -500,
                        CloseEndofDay = false,
                        Period = prices.Count,
                        
                    };


                    AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices);
                
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