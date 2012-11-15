using System.Windows.Forms;
using AlsiUtils.Strategies;


namespace NinjaTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private static void SlowsTOCHpoP()
        {
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css =@"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";


            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_FullHistory(10000, false);

            AlsiUtils.Strategies.Parameter_SlowStoch p = new Parameter_SlowStoch();
            p.Close_20 = 15;
            p.Close_80 = 60;
            p.Fast_K = 3;
            p.Slow_K = 6;
            p.Slow_D = 4;
            p.Open_20 = 30;
            p.Open_80 = 50;
            p.StopLoss = -250;
            p.TakeProfit = 250;

            Strategy_SSPOP.SsPopStrategy(p,prices);
           
        }

        private static void BolBand()
        {
            //Laptop
            //string css = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

            //PC
            string css =@"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";


            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = css;

            var prices = AlsiUtils.DataBase.readDataFromDataBase_10_MIN_FullHistory(500, false);

            Parameter_Bollinger P = new Parameter_Bollinger();
            P.N = 20;
            P.P = 2;

           // Strategy_Bollinger.BollingerStrategy(P,prices);

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
           
           // BolBand();
            SlowsTOCHpoP();
         
            Close();
        }
    }
}
