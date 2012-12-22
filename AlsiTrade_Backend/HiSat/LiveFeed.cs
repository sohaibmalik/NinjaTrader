using System.Diagnostics;
using HISAT_API;

namespace AlsiTrade_Backend.HiSat
{

    public class LiveFeed
    {
        private bool _ConnectedToLiveFeed;
        DataFeed datafeed;
        bool connectedtoHISAT = false;
        string CN;

        public LiveFeed(string InstrumentName)
        {
            datafeed = new DataFeed();
            CN = InstrumentName;
            createHiSatEvents();
            Login();

        }

        private void createHiSatEvents()
        {
            datafeed.StatusUpdate += new DataFeed.StatusUpdateEventHandler(datafeed_StatusUpdate);
            datafeed.onTrade += new DataFeed.onTradeEventHandler(datafeed_onTrade);
            datafeed.onAsk += new DataFeed.onAskEventHandler(datafeed_onAsk);
            datafeed.onBid += new DataFeed.onBidEventHandler(datafeed_onBid);
            datafeed.onBidask += new DataFeed.onBidaskEventHandler(datafeed_onBidask);
        }

        /// <summary>
        /// Login to HiSat to receive Live Data.
        /// </summary>
        private void Login()
        {

            datafeed.LogOnToHisat("Johan", "Hisat");

            int Timeout = 0;
            while (!connectedtoHISAT)
            {
                Timeout++;
                if (Timeout == 20) break;
                System.Threading.Thread.Sleep(100);
                Debug.WriteLine("Waiting to Connect -- Timeout = " + Timeout);
            }

            if (connectedtoHISAT)
            {
                Debug.WriteLine("Connected to Hisat");
                //ddeStatusLabel.Text = "HiSat Connected";
                // StartFeed(Properties.Settings.Default.HiSat_InstrumentName);
               // startFeedToolStripMenuItem.Enabled = false;
                // UpdateLog("Status", "Connected to HiSat Live Feed", Color.Blue, true);
            }
            else
            {
                Debug.WriteLine("Failed to  Connect to Hisat");
                //  UpdateLog("Error", "Could not Connect to HiSAT", Color.Red, true);
            }
        }
        /// <summary>
        /// Start Live Streaming once Logged In
        /// </summary>
        /// <param name="Instrument">instrument Name  - eg SEP12ALSI</param>
        private void StartFeed(string Instrument)
        {
            datafeed.RequestInstrumentUpdate(Instrument);
            Debug.WriteLine("Requested Feed " + Instrument);
        }

        private void datafeed_StatusUpdate(object sender, DataFeed.HSEventArgs e)
        {
            Debug.WriteLine("STATUS UPDATE " + e.theString);
            if (e.theString == "Logged on to server") _ConnectedToLiveFeed = true;
        }

        private void datafeed_onTrade(object sender, DataFeed.aTrade e)
        {
            // DataBase.insertTicks(e.TradeTime, Convert.ToInt32(e.TradePrice));
        }

        private void datafeed_onAsk(object sender, DataFeed.aAsk e)
        {
            // liveOfferLabel.Text = e.AskPrice.ToString();
            //liveVolOfferLabel.Text = e.AskVol.ToString();

        }

        private void datafeed_onBid(object sender, DataFeed.aBid e)
        {
            //liveBidLabel.Text = e.BidPrice.ToString();
            //liveVolBidLabel.Text = e.BidVol.ToString();
        }

        private void datafeed_onBidask(object sender, DataFeed.aBidAsk e)
        {
            //liveBidLabel.Text = e.BidPrice.ToString();
            //liveVolBidLabel.Text = e.BidVol.ToString();
            //liveOfferLabel.Text = e.AskPrice.ToString();
            //liveVolOfferLabel.Text = e.AskVol.ToString();

        }
    }
}
