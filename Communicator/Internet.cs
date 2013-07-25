using System;
using System.Net;
using System.Timers;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
using System.Runtime.InteropServices;
namespace Communicator
{
    public class Internet
    {
        Timer T;




        public Internet(int Interval)
        {


            T = new Timer();
            T.Interval = Interval;
            T.Elapsed += new ElapsedEventHandler(T_Elapsed);
            T.Start();
        }
        void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckConnection();
        }





        public static  bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                  
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }





        public event ConnectionStatus OnConnectionStatusChange;
        public delegate void ConnectionStatus(object Sender, ConnectionStatusEventArgs e);
        public class ConnectionStatusEventArgs : EventArgs
        {
            public bool Connected;

        }
    }

}
