using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoSecondLayer;
namespace SecondAlgo
{
    class Program
    {
        #region RSI_SlowStoc

        // var con = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
        // var con = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
        // var con = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";
        #endregion              

        #region RSI_ADX Stop
        //static void Main(string[] args)
        //{
        //    Algo.LoadPrice();
        //    Algo.P.ADX_period = 75;
        //    Algo.P.RSI_period = 25;
        //    Algo.P.Mamapar1 = 0.01;
        //    Algo.P.Mamapar2 = 0.01;
        //    Algo.P.RSI_EMA = 28;
        //    Algo.P.RSI_Slope_period = 6;
        //    Algo.P.RSI_TOP = 71;
        //    Algo.P.RSI_BOTTOM = 33;
        //    Algo.P.Di_DOWN_avg_period = 8;
        //    Algo.P.Di_UP_avg_period = 12;
        //    var profit = Algo.CalculatePL(Algo.CalcIndicators());

        //    Console.WriteLine(CountCalcs());
        //    Console.ReadLine();
        //}

        //private static void RunSIM()
        //{
        //    Algo.LoadPrice();

        //    var dc = new RSI_ADX_SIMDataContext();
        //    var s = dc.tblSettings.First();
        //    double count = 0;

        //    double total = CountCalcs();
        //    double progress = 0;

        //    for (int ADX = 5; ADX <= 100; ADX += 1)
        //        for (int RSI = 5; RSI <= 30; RSI += 1)
        //            for (double MAMA1 = 0.01; MAMA1 <= 0.5; MAMA1 += 0.055)
        //                for (double MAMA2 = 0.01; MAMA2 <= 0.5; MAMA2 += 0.055)
        //                    for (int RSI_EMA = 2; RSI_EMA <= 40; RSI_EMA += 1)
        //                        for (int RSI_SLOPE = 2; RSI_SLOPE <= 20; RSI_SLOPE += 1)
        //                            for (int RSI_TOP = 50; RSI_TOP <= 85; RSI_TOP += 1)
        //                                for (int RSI_BOTTOM = 50; RSI_BOTTOM >= 15; RSI_BOTTOM -= 1)
        //                                    for (int DMI_P_EMA = 2; DMI_P_EMA <= 30; DMI_P_EMA += 1)
        //                                        for (int DMI_M_EMA = 2; DMI_M_EMA <= 30; DMI_M_EMA += 1)
        //                                        {
        //                                            count++;

        //                                            Algo.P.ADX_period = ADX;
        //                                            Algo.P.RSI_period = RSI;
        //                                            Algo.P.Mamapar1 = MAMA1;
        //                                            Algo.P.Mamapar2 = MAMA2;
        //                                            Algo.P.RSI_EMA = RSI_EMA;
        //                                            Algo.P.RSI_Slope_period = RSI_SLOPE;
        //                                            Algo.P.RSI_TOP = RSI_TOP;
        //                                            Algo.P.RSI_BOTTOM = RSI_BOTTOM;
        //                                            Algo.P.Di_DOWN_avg_period = DMI_M_EMA;
        //                                            Algo.P.Di_UP_avg_period = DMI_P_EMA;
        //                                            var profit = Algo.CalculatePL(Algo.CalcIndicators());
        //                                            if (profit > 13000 || profit < -13000)
        //                                            {
        //                                                // SAVE RESULTS IN DATABASE
        //                                                var r = new tbl5Minute()
        //                                                {
        //                                                    ADX = ADX,
        //                                                    RSI = RSI,
        //                                                    MAMA_1 = MAMA1,
        //                                                    MAMA_2 = MAMA2,
        //                                                    RSI_EMA = RSI_EMA,
        //                                                    RSI_SLOPE = RSI_SLOPE,
        //                                                    RSI_BOTTOM = RSI_BOTTOM,
        //                                                    RSI_TOP = RSI_TOP,
        //                                                    DMI_MINUS = DMI_M_EMA,
        //                                                    DMI_PLUS = DMI_P_EMA,
        //                                                    Profit = (int)profit,
        //                                                };
        //                                                dc.tbl5Minutes.InsertOnSubmit(r);
        //                                                dc.SubmitChanges();
        //                                                Console.WriteLine("Profit " + profit + "     " + progress + " %");
        //                                            }
        //                                            if (count % 10000 == 0)
        //                                            {
        //                                                progress = Math.Round((count / total * 100), 2);
        //                                                Console.WriteLine("Progress " + progress + " %");
        //                                            }
        //                                        }



        //    Console.ReadLine();
        //}

        //private static double CountCalcs()
        //{
        //    var dc = new RSI_ADX_SIMDataContext();
        //    var s = dc.tblSettings.First();
        //    double count = 0;

        //    for (int ADX = 5; ADX <= 65; ADX += 2)
        //        for (int RSI = 5; RSI <= 30; RSI += 1)
        //            for (double MAMA1 = 0.01; MAMA1 <= 0.5; MAMA1 += 0.055)
        //                for (double MAMA2 = 0.01; MAMA2 <= 0.5; MAMA2 += 0.055)
        //                    for (int RSI_EMA = 2; RSI_EMA <= 40; RSI_EMA += 1)
        //                        for (int RSI_SLOPE = 2; RSI_SLOPE <= 20; RSI_SLOPE += 1)
        //                            for (int RSI_TOP = 50; RSI_TOP <= 85; RSI_TOP += 1)
        //                                for (int RSI_BOTTOM = 50; RSI_BOTTOM >= 15; RSI_BOTTOM -= 1)
        //                                    for (int DMI_P_EMA = 2; DMI_P_EMA <= 30; DMI_P_EMA += 2)
        //                                        for (int DMI_M_EMA = 2; DMI_M_EMA <= 30; DMI_M_EMA += 2)
        //                                            count++;
        //    return count;
        //}

        #endregion



        static void Main(string[] args)
        {

            Console.WindowWidth = 100;
            Console.WriteLine("Getting Prices...");
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";

            AlsiUtils.Data_Objects.GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
            Console.WriteLine("Done.");
            //var sim = new AlsiSim();
            //sim.Start();

            var sim = new AlsiPOP();
            sim.Start();
        }


        
    }

    public class AlsiPOP
{

    internal void Start()
    {
        Console.WriteLine("Starting Calculations..");
        var s = new StochPOP();
        s.Start();
        Console.WriteLine("Done");
    }
}
    public class AlsiSim
    {
        private RSI_SS_StopLoss a;
        private string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";

        public void Start()
        {
            while (true)
            {
                a = new RSI_SS_StopLoss();
                // a.Done += a_Done;

                a.Start(SIMcontext);
                Environment.Exit(0);
            }
           
        }

      
    }
}
