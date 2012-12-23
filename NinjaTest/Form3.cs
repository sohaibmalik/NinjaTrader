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




            var prices = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.DataBase.timeframe.minute_5, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, end, false);
            Debug.WriteLine("Start Date " + prices[0].TimeStamp);

            for (int x = 21; x <= 30; x++)
            {
                for (int y = 8; y <= 40; y++)
                {
                   
                    for (int a = 27; a <= 50; a++)
                    {
                      
                        for (int b = 8; b <= 50; b++)
                        {
                         
                            for (int z = 20; z <= 70; z++)
                            {

                                if (x < y &&  y<a  && a < b && b < z)
                                {
                                    AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
                                    {
                                        A_EMA1 = x,
                                        A_EMA3 = 1,
                                        A_EMA4 = 1,
                                        A_EMA5 = 1,
                                        A_EMA6 = y,
                                        B_EMA1 = a,
                                        B_EMA2 = 1,
                                        B_EMA3 = 1,
                                        B_EMA4 = 1,
                                        B_EMA5 = 1,
                                        B_EMA6 = b,
                                        C_EMA = z,
                                        TakeProfit = 250,
                                        StopLoss = -250,
                                        CloseEndofDay = false,
                                        Period = 2012,
                                    };


                                    AlsiUtils.Strategies.EMA_Scalp.EmaScalp(E, prices);
                                }
                            }
                        }
                    }
                }
            }
        }



        private static void RunSingle()
        {
              //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;


            DateTime s = new DateTime(2012, 02, 02);
            DateTime e = new DateTime(2012, 12, 15);


            var prices = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.DataBase.timeframe.minute_5, AlsiUtils.DataBase.dataTable.MasterMinute,
               s, e, false);
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
            //RunSingle();
            //Close();
        }


    }

}