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
        static void Main(string[] args)
        {
            Algo.LoadPrice();
             Algo.P.ADX_period = 75;
                                                    Algo.P.RSI_period = 25;
                                                    Algo.P.Mamapar1 = 0.01;
                                                    Algo.P.Mamapar2 = 0.01;
                                                    Algo.P.RSI_EMA = 28;
                                                    Algo.P.RSI_Slope_period = 6;
                                                    Algo.P.RSI_TOP = 71;
                                                    Algo.P.RSI_BOTTOM = 33;
                                                    Algo.P.Di_DOWN_avg_period = 8;
                                                    Algo.P.Di_UP_avg_period = 12;
                                                    var profit = Algo.CalculatePL(Algo.CalcIndicators());

        }

        private static void RunSIM()
        {
            Algo.LoadPrice();

            var dc = new RSI_ADX_SIMDataContext();
            var s = dc.tblSettings.First();
            double count = 0;

            double total = CountCalcs();
            double progress = 0;

            for (int ADX = (int)s.ADX_START; ADX <= s.ADX_END; ADX += 2)
                for (int RSI = (int)s.RSI_START; RSI <= s.RSI_END; RSI += 2)
                    for (double MAMA1 = (double)s.MAMA_1_START; MAMA1 <= s.MAMA_1_END; MAMA1 += 0.055)
                        for (double MAMA2 = (double)s.MAMA_2_START; MAMA2 <= s.MAMA_2_END; MAMA2 += 0.055)
                            for (int RSI_EMA = (int)s.RSI_EMA_START; RSI_EMA <= s.RSI_EMA_END; RSI_EMA += 2)
                                for (int RSI_SLOPE = (int)s.RSI_SLOPE_START; RSI_SLOPE <= s.RSI_SLOPE_END; RSI_SLOPE += 2)
                                    for (int RSI_TOP = (int)s.RSI_TOP_START; RSI_TOP <= s.RSI_TOP_END; RSI_TOP += 2)
                                        for (int RSI_BOTTOM = (int)s.RSI_BOTTOM_START; RSI_BOTTOM >= s.RSI_BOTTOM_END; RSI_BOTTOM -= 2)
                                            for (int DMI_P_EMA = (int)s.DMI_PLUS_START; DMI_P_EMA <= s.DMI_PLUS_END; DMI_P_EMA += 2)
                                                for (int DMI_M_EMA = (int)s.DMI_MINUS_START; DMI_M_EMA <= s.DMI_MINUS_END; DMI_M_EMA += 2)
                                                {
                                                    count++;

                                                    Algo.P.ADX_period = ADX;
                                                    Algo.P.RSI_period = RSI;
                                                    Algo.P.Mamapar1 = MAMA1;
                                                    Algo.P.Mamapar2 = MAMA2;
                                                    Algo.P.RSI_EMA = RSI_EMA;
                                                    Algo.P.RSI_Slope_period = RSI_SLOPE;
                                                    Algo.P.RSI_TOP = RSI_TOP;
                                                    Algo.P.RSI_BOTTOM = RSI_BOTTOM;
                                                    Algo.P.Di_DOWN_avg_period = DMI_M_EMA;
                                                    Algo.P.Di_UP_avg_period = DMI_P_EMA;
                                                    var profit = Algo.CalculatePL(Algo.CalcIndicators());
                                                    if (profit > 13000)
                                                    {
                                                        var r = new tbl5Minute()
                                                        {
                                                            ADX = ADX,
                                                            RSI = RSI,
                                                            MAMA_1 = MAMA1,
                                                            MAMA_2 = MAMA2,
                                                            RSI_EMA = RSI_EMA,
                                                            RSI_SLOPE = RSI_SLOPE,
                                                            RSI_BOTTOM = RSI_BOTTOM,
                                                            RSI_TOP = RSI_TOP,
                                                            DMI_MINUS = DMI_M_EMA,
                                                            DMI_PLUS = DMI_P_EMA,
                                                            Profit = (int)profit,
                                                        };
                                                        dc.tbl5Minutes.InsertOnSubmit(r);
                                                        dc.SubmitChanges();
                                                        Console.WriteLine("Profit " + profit + "     " + progress + " %");
                                                    }
                                                    if (count % 10000 == 0)
                                                    {
                                                        progress = Math.Round((count / total * 100), 2);
                                                        Console.WriteLine("Progress " + progress + " %");
                                                    }
                                                }

            // Console.WriteLine("COUNT " + count);
            //    profit = Algo.CalculatePL(Algo.CalcIndicators());


            Console.ReadLine();
        }

        private static double CountCalcs()
        {
            var dc = new RSI_ADX_SIMDataContext();
            var s = dc.tblSettings.First();
            double count = 0;

            for (int ADX = (int)s.ADX_START; ADX <= s.ADX_END; ADX += 2)
                for (int RSI = (int)s.RSI_START; RSI <= s.RSI_END; RSI += 2)
                    for (double MAMA1 = (double)s.MAMA_1_START; MAMA1 <= s.MAMA_1_END; MAMA1 += 0.055)
                        for (double MAMA2 = (double)s.MAMA_2_START; MAMA2 <= s.MAMA_2_END; MAMA2 += 0.055)
                            for (int RSI_EMA = (int)s.RSI_EMA_START; RSI_EMA <= s.RSI_EMA_END; RSI_EMA += 2)
                                for (int RSI_SLOPE = (int)s.RSI_SLOPE_START; RSI_SLOPE <= s.RSI_SLOPE_END; RSI_SLOPE += 2)
                                    for (int RSI_TOP = (int)s.RSI_TOP_START; RSI_TOP <= s.RSI_TOP_END; RSI_TOP += 2)
                                        for (int RSI_BOTTOM = (int)s.RSI_BOTTOM_START; RSI_BOTTOM >= s.RSI_BOTTOM_END; RSI_BOTTOM -= 2)
                                            for (int DMI_P_EMA = (int)s.DMI_PLUS_START; DMI_P_EMA <= s.DMI_PLUS_END; DMI_P_EMA += 2)
                                                for (int DMI_M_EMA = (int)s.DMI_MINUS_START; DMI_M_EMA <= s.DMI_MINUS_END; DMI_M_EMA += 2)
                                                    count++;
            return count;
        }
    }
}
