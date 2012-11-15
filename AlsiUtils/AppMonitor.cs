using System;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace AlsiUtils
{
	public class AppMonitor
	{




		public static void CheckifResponding()
		{
			List<string> procstoWatch = new List<string>();
			procstoWatch.Add("AlsiTrade");
			procstoWatch.Add("AlsiDataImport");
			procstoWatch.Add("Estuary.Solutions.OTS");



			Process[] prolist = Process.GetProcesses();

			foreach (Process p in prolist)
			{

				foreach (string name in procstoWatch)
				{
					if (p.ProcessName == name) Debug.WriteLine(p.ProcessName + "  Responing " + p.Responding);
					if (!p.Responding) p.Kill();
				}

			}
		}


	}
}
