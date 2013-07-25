using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineDataVerify
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Checking last few days:\n");
            AlsiUtils.WebSettings.GetSettings();
            var data = AlsiTrade_Backend.HiSat.HistData.GetHistoricalMINUTE_FromWEB(DateTime.Now.AddDays(-10),
                DateTime.Now, 1, AlsiUtils.WebSettings.General.HISAT_INST);




            for (int x = 0; x < 10; x++)
            {
                if (DateTime.Now.AddDays(-x).DayOfWeek != DayOfWeek.Saturday)
                    if (DateTime.Now.AddDays(-x).DayOfWeek != DayOfWeek.Sunday)
                    {
                        var T = data.Where(z => z.TimeStamp.Date == DateTime.Now.Date.AddDays(-x));
                        var count=T.Count() == 0;
                        Console.WriteLine("{0} {1} {2}", DateTime.Now.Date.AddDays(-x).DayOfWeek.ToString().PadRight(10),
                            DateTime.Now.Date.Date.ToShortDateString().PadRight(10), count ? "!!!" : T.Count().ToString());
                        if (count)
                        {
                            Console.WriteLine("Error Found");
                            Communicator.Gmail.SendEmail("Pieterf33@gmail.com", "HiSat Data Failed",
                                "This is an automated email.\nThe alsi future data is not available for "
                            + DateTime.Now.Date.AddDays(-x).ToShortDateString(),
                                null, "Pieterf33@gmail.com", "1rachelle", "Alsi Trading System", true);
                        }

                    }


            }


        
        }
    }
}
