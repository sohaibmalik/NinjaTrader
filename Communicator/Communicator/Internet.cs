using System;
using System.Net;
namespace Communicator
{
    public class Internet
    {
        private bool _Connected;

        public bool Connected
        {
            get
            {
                return _Connected;
            }
            set
            {
                if (value != _Connected)
                {
                    var e = new ConnectionStatusEventArgs();
                    e.Connected = value;
                    OnConnectionStatusChange(this, value);
                }
                _Connected = value;
            }
        }

        public bool CheckConnection()
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
