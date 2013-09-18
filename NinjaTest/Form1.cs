using System.Windows.Forms;
using AlsiUtils.Strategies;
using System;


namespace NinjaTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }



        private void StartSIM()
        {
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";


            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;


            DateTime s = new DateTime(2012, 12, 1);
            DateTime e = new DateTime(2013, 12, 29);

            var prices = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, s, e, false);

            AlsiUtils.Strategies.Parameter_SS_RSI p = new AlsiUtils.Strategies.Parameter_SS_RSI();
            p.Close_20 = 15;
            p.Close_80 = 60;
            p.Fast_K = 3;
            p.Slow_K = 6;
            p.Slow_D = 4;
            p.Open_20 = 30;
            p.Open_80 = 50;
            p.StopLoss = -250;
            p.TakeProfit = 250;


            for (int rsi = (int)rsiStart.Value; rsi <= (int)rsiEnd.Value; rsi += 2)
            {
                rsiLabel.Text = rsi.ToString() + " / " + rsiEnd.Value.ToString();
                for (int ma = (int)maStart.Value; ma <= (int)maEnd.Value; ma += 2)
                {
                    maLabel.Text = ma.ToString() + " / " + maEnd.Value.ToString();
                    for (int ma2 = (int)ma2Start.Value; ma2 <= (int)ma2End.Value; ma2 += 2)
                    {
                        ma2Label.Text = ma2.ToString() + " / " + ma2End.Value.ToString();
                        for (int midL = 40; midL <= 55; midL += 2)
                        {
                            midLLabel.Text = midL.ToString() + " / 55";
                            for (int midS = 35; midS <= 50; midS += 2)
                            {
                                midSlabel.Text = midS.ToString() + " / 50";
                                for (int cL = 50; cL <= 70; cL += 5)
                                {
                                    cLLabel.Text = cL.ToString() + " / 70";
                                    for (int cS = 30; cS <= 50; cS += 5)
                                    {
                                        cSLabel.Text = cS.ToString() + " / 50";
                                        p.RSI = rsi;
                                        p.RSI_MA = ma;
                                        p.RSI_MA2 = ma2;
                                        p.RSI_MidLine_Long = midL;
                                        p.RSI_MidLine_Short = midS;
                                        p.RSI_CloseLong = cL;
                                        p.RSI_CloseShort = cS;
                                      //  Db(Startegy_SSPOP_RSI.SsPopStrategy(p, prices), p);
                                        AlsiUtils.Strategies.Startegy_SSPOP_RSI.SsPopStrategy(p, prices);
                                        Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        //private static void Db(SumStats S, Parameter_SS_RSI P)
        //{
        //    //SimDbDataContext dc = new SimDbDataContext();
        //    //if (S.TotalProfit < 10000) return;


        //    //tblRSI t = new tblRSI()
        //    //{
        //    //    Total_Profit = (long)S.TotalProfit,
        //    //    Total_avg_PL = S.Total_Avg_PL,
        //    //    Trade_Count = (int)S.TradeCount,
        //    //    PL_Ratio = S.PL_Ratio,
        //    //    Avg_Profit = S.Avg_Prof,
        //    //    Avg_Loss = S.Avg_Loss,
        //    //    Pct_Profit = S.Pct_Prof,
        //    //    Pct_Loss = S.Pct_Loss,

        //    //    RSI = P.RSI,
        //    //    RSI_MA = P.RSI_MA,
        //    //    RSI_MA2 = P.RSI_MA2,
        //    //    Mid_Long = P.RSI_MidLine_Long,
        //    //    Mid_Short = P.RSI_MidLine_Short,
        //    //    CloseLong = P.RSI_CloseLong,
        //    //    CloseShort = P.RSI_CloseShort,

        //    //};


        //    //dc.tblRSIs.InsertOnSubmit(t);
        //    //dc.SubmitChanges();
        //}

        private static void BolBand()
        {
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";


            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

          //  var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_MasterMinute(500, false);

            Parameter_Bollinger P = new Parameter_Bollinger();
            P.N = 20;
            P.P = 2;

            // Strategy_Bollinger.BollingerStrategy(P,prices);

        }

       



        private void BW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            StartSIM();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            ma2Start.Enabled = false;
            ma2End.Enabled = false;
            rsiStart.Enabled = false;
            rsiEnd.Enabled = false;
            maStart.Enabled = false;
            maEnd.Enabled = false;
            BW.RunWorkerAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            maStart.Value = 10;
            maEnd.Value = 20;
            rsiStart.Value = 8;
            rsiEnd.Value = 12;
            ma2Start.Value = 12;
            ma2End.Value = 15;
            BW.RunWorkerAsync();
        }

       




    }
}
