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
            var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_MasterMinute(12000, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                           {
                               A_EMA1 = 10,
                               A_EMA3 = 1,
                               A_EMA4 = 1,
                               A_EMA5 = 1,
                               A_EMA6 = 14,
                               B_EMA1 = 24,
                               B_EMA2 = 1,
                               B_EMA3 = 1,
                               B_EMA4 = 1,
                               B_EMA5 = 1,
                               B_EMA6 = 28,
                               C_EMA = 30,
                               TakeProfit = 208,
                               StopLoss = -208,
                               CloseEndofDay = false,
                           };


            AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices);



            //for (int x = 9; x <= 16; x++)
            //{
            //    for (int y = 2; y <= 10; y++)
            //    {
            //        for (int z = 20; z <= 70; z++)
            //        {

            //            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            //            {
            //                A_EMA1 = x,
            //                A_EMA3 = 1,
            //                A_EMA4 = 1,
            //                A_EMA5 = 1,
            //                A_EMA6 = x + y,
            //                B_EMA1 = x + y + x,
            //                B_EMA2 = 1,
            //                B_EMA3 = 1,
            //                B_EMA4 = 1,
            //                B_EMA5 = 1,
            //                B_EMA6 = x + y + x + y,
            //                C_EMA = z,
            //                TakeProfit = 250,
            //                StopLoss = -250,
            //                CloseEndofDay = false,
            //            };


            //            AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices);

            //        }
            //    }
            //}
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            button1.PerformClick();
            Close();
        }
    }
}
