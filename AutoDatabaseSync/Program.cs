using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiTrade_Backend;
using System.Timers;

namespace AutoDatabaseSync
{
	public class Program
	{
		private static bool Completed;

		static void Main(string[] args)
		{
            if (!VerifyOnlineData()) return;
			string con = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
			Console.WriteLine("===Database Synchronization===");
			Console.WriteLine("Parameters : Laptop or PC");
			Console.WriteLine("Defualt : Laptop \n" + con);
			Console.WriteLine("==================");

			if (args.Count() > 0)
				if (args.Contains("Laptop"))
				{
					Console.WriteLine("Welcome Laptop");
					con = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
				}

			if (args.Contains("PC"))
			{
				Console.WriteLine("Welcome PC");
				con = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
			}

			AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = con;

			Timer t = new Timer();
			var sdb = new SyncDB();
			sdb.OnUpdatedComplete += new SyncDB.Updated(sdb_OnUpdatedComplete);
			t.Elapsed += new ElapsedEventHandler(t_Elapsed);
			t.Interval = 1000;
			t.Start();
			Console.Write("Starting synchronization ");
			sdb.StartSync();
			Console.ReadLine();

		}

		static void t_Elapsed(object sender, ElapsedEventArgs e)
		{
			Console.Write(".");
			if (Completed) Environment.Exit(0);
		}

		static void sdb_OnUpdatedComplete(object sender, SyncDB.UpdatedEventArgs e)
		{
			Console.WriteLine("\nSync Complete");			
			System.Threading.Thread.Sleep(2000);
			Completed = true;

		}

        private static bool VerifyOnlineData()
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
                        var count = T.Count() == 0;
                        Console.WriteLine("{0} {1} {2}", DateTime.Now.Date.AddDays(-x).DayOfWeek.ToString().PadRight(10),
                            DateTime.Now.Date.Date.ToShortDateString().PadRight(10), count ? "!!!" : T.Count().ToString());
                        if (count)
                        {
                            var emails = new List<string>();
                            emails.Add("pieterf33@gmail.com");
                            emails.Add("johan@bizinsa.com");
                            Console.WriteLine("Error Found");
                            Communicator.Gmail.SendEmail(emails, "HiSat Data Failed",
                                "This is an automated email.\nThe alsi future data is not available for "
                            + DateTime.Now.Date.AddDays(-x).ToShortDateString(),
                                null, "Pieterf33@gmail.com", "1rachelle", "Alsi Trading System", true);

                            return false;
                        }
                    }
            }
            return true;

        }

	}
}
