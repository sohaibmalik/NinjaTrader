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
			var data = AlsiTrade_Backend.HiSat.HistData.GetHistoricalMINUTE_FromWEB(DateTime.Now.AddDays(-12), DateTime.Now, 1, AlsiUtils.WebSettings.General.HISAT_INST);


			

			for (int x = 0; x < 10; x++)
			{
				if (DateTime.Now.AddDays(-x).DayOfWeek != DayOfWeek.Saturday || DateTime.Now.AddDays(-x).DayOfWeek != DayOfWeek.Sunday)
				{
					var T = data.Where(z => z.TimeStamp.Date == DateTime.Now.Date.AddDays(-x));
					Console.WriteLine("{0} {1} {2}", DateTime.Now.Date.AddDays(-x).DayOfWeek.ToString().PadRight(10), 
						DateTime.Now.Date.Date.ToShortDateString().PadRight(10),T.Count()==0?"!!!":T.Count().ToString());

				
				}

				
			}


			Console.ReadLine();

		}
	}
}
