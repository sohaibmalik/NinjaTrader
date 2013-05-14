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

	}
}
