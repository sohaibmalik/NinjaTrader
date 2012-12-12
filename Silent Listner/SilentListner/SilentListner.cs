using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace SilentListner
{
    public partial class SilentListner : Form
    {
        AlsiWebService.AlsiNotifyService service = new AlsiWebService.AlsiNotifyService();

        private bool StartupTradeApps, StartupOTS;

        public SilentListner()
        {
            InitializeComponent();
        }


        private void SilentListner_Load(object sender, EventArgs e)
        {
            try
            {
                service.SendCommand(AlsiWebService.Command.Idle);
                Debug.WriteLine(" Get First Command and ignore : " + service.GetCommand());
                
            }
            catch (Exception ex)
            {

            }
            updateTimer.Interval = 1000;
            updateTimer.Enabled = true;
            RunStartups();
        }

        private void RunStartups()
        {
            var d = DateTime.Now;

            if (d.Hour == 7 && d.Minute < 59) ;
            else
            {
                StartTradingApps();
                StartOTS();
            }
        }



        private void RestartPC()
        {
            Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\restartPC.bat");
        }

        private void StartOTS()
        {
            var d = new Stopwatch();
            d.Start();
            Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\startOTS.bat");
            while (d.Elapsed.Seconds < 58)
            {

            }
        }

        private void StartTradingApps()
        {
            var d = new Stopwatch();
            d.Start();
            Process.Start(@"C:\Users\Pieter\Dropbox\Alsi Trade App\Batch Commands\startTradingApps.bat");
            while (d.Elapsed.Seconds < 58)
            {

            }
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var c = service.GetCommand();
                Debug.WriteLine(c);
                if (c == AlsiWebService.Command.RestartPC)
                {
                    RestartPC();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
