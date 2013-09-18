using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
namespace AutoDatabaseBackup
{
	class Program
	{
		private static string constring = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
	

		static void Main(string[] args)
		{
			
	
			if(args.Count()>0)
				if(args.Contains("PC"))
                    constring = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";

			if(CreateOrVerifyPath(new DirectoryInfo(@"C:\AlsiTradeDatabseBACKUP")))
			{
			
			}
			else
			{
					 CreateOrVerifyPath(new DirectoryInfo(@"C:\AlsiTradeDatabseBACKUP"));
			}

		

			
		}

		private static bool CreateOrVerifyPath(DirectoryInfo D)
		{
			StringBuilder filename = new StringBuilder();
			filename.Append(DateTime.Now.Date.Day+"-");
			filename.Append(DateTime.Now.Date.Month + "-");
			filename.Append(DateTime.Now.Date.Year + "-");
			filename.Append("AlsiTrade.Bak");

			if (D.Exists)
			{
				Console.WriteLine("Checking directory ...\n" + D.FullName + " does exist");
				Console.WriteLine("Starting Backup " + filename.ToString());
				RunBackup(constring,D.FullName+@"\"+filename.ToString());
				return true;
			}
			else
			{
				Console.WriteLine("Creating directory..");
				Directory.CreateDirectory(D.FullName);
				return false;
			}
		}

		private static void RunBackup(string constring,string path)
		{

			using (SqlConnection Con = new SqlConnection(constring))
			{
				Con.Open();

				var command = Con.CreateCommand();
				command.CommandText = @"BACKUP DATABASE AlsiTrade TO disk = '"+path+"'";
				command.ExecuteNonQuery();


			}
		}





	}
}
