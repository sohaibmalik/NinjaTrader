using System.Windows.Forms;
using AlsiUtils.Strategies;
using ExcelLink;


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

            var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_FullHistory(10000, false);

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


            for (int rsi = 3; rsi <= 30; rsi++)
            {
                rsiLabel.Text = rsi.ToString() + " / 30";
                for (int ma = 3; ma <= 30; ma++)
                {
                    maLabel.Text = ma.ToString() + " / 30";
                    for (int ma2 = 3; ma2 <= 30; ma2++)
                    {
                        ma2Label.Text = ma2.ToString() + " / 30";
                        for (int midL = 40; midL <= 55; midL++)
                        {
                            midLLabel.Text = midL.ToString() + " / 55";
                            for (int midS = 35; midS <= 50; midS++)
                            {
                                midSlabel.Text = midS.ToString() + " / 50";
                                for (int cL = 50; cL <= 70; cL++)
                                {
                                    cLLabel.Text = cL.ToString() + " / 70";
                                    for (int cS = 30; cS <= 50; cS++)
                                    {
                                        cSLabel.Text = cS.ToString() + " / 50";
                                        p.RSI = rsi;
                                        p.RSI_MA = ma;
                                        p.RSI_MA2 = ma2;
                                        p.RSI_MidLine_Long = midL;
                                        p.RSI_MidLine_Short = midS;
                                        p.RSI_CloseLong = cL;
                                        p.RSI_CloseShort = cS;
                                        Db(Startegy_SSPOP_RSI.SsPopStrategy(p, prices), p);


                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Db(SumStats S, Parameter_SS_RSI P)
        {
            SimDbDataContext dc = new SimDbDataContext();

            SS_RSI t = new SS_RSI()
            {
                Total_Profit = (long)S.TotalProfit,
                Total_avg_PL = (long)S.Total_Avg_PL,
                Trade_Count = (int)S.TradeCount,
                PL_Ratio = (decimal)S.PL_Ratio,
                Avg_Profit = (decimal)S.Avg_Prof,
                Avg_Loss = (decimal)S.Avg_Loss,
                Pct_Profit = (decimal)S.Pct_Prof,
                Pct_Loss = (decimal)S.Pct_Loss,

                RSI = P.RSI,
                RSI_MA = P.RSI_MA,
                RSI_MA2 = P.RSI_MA2,
                Mid_Long = P.RSI_MidLine_Long,
                Mid_Short = P.RSI_MidLine_Short,
                CloseLong = P.RSI_CloseLong,
                CloseShort = P.RSI_CloseShort,

            };
            dc.SS_RSIs.InsertOnSubmit(t);
            dc.SubmitChanges();
        }

        private static void BolBand()
        {
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";


            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_FullHistory(500, false);

            Parameter_Bollinger P = new Parameter_Bollinger();
            P.N = 20;
            P.P = 2;

            // Strategy_Bollinger.BollingerStrategy(P,prices);

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {




        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ExcelOrder eo = new ExcelOrder();
            xlTradeOrder o = new xlTradeOrder()
                                 {
                                     BS = xlTradeOrder.BuySell.Buy,
                                     Price = 32800,
                                     Volume = 1,
                                     Contract = "FZC20 ALSI",
                                 };
            eo.Connect();
            eo.WriteOrder(o);
            eo.Disconnect();
        }

        private void BW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            StartSIM();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            BW.RunWorkerAsync();
        }




    }
}
