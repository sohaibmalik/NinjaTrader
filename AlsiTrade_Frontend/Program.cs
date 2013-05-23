using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace FrontEnd
{
    static class Program
    {
        //http://odetocode.com/blogs/scott/archive/2004/08/20/the-misunderstood-mutex.aspx
        static string appGuid = "d4ca826e-0ad2-47d4-9430-8576f2b374d4";
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Instance already running");
                    Environment.Exit(0);
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //var con = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
                var con = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
                AlsiUtils.DataBase.SetConnectionString(con);


                //  Application.Run(new Chart());
               // Application.Run(new StartupForm());
                 Application.Run(new Test3());
            }
        }
    }
}
