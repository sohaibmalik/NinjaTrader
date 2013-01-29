using System.Diagnostics;
using HISAT_API;
using AlsiUtils;
using System;
namespace AlsiTrade_Backend.HiSat
{

    public class LiveFeed
    {
     
        DataFeed datafeed;       
        string _Instrument;
      
        public LiveFeed(string InstrumentName)
        {
            datafeed = new DataFeed();
            _Instrument = InstrumentName;
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
            if (e.theString == "Logged on to server")
            {              
                StartFeed(_Instrument);
            }
        }

        private void datafeed_onTrade(object sender, DataFeed.aTrade e)
        {
            // DataBase.insertTicks(e.TradeTime, Convert.ToInt32(e.TradePrice));  
            LivePrice.Last = (double)e.TradePrice;
            
        }

        private void datafeed_onAsk(object sender, DataFeed.aAsk e)
        {
            LivePrice.Offer = (double)e.AskPrice;          
        }

        private void datafeed_onBid(object sender, DataFeed.aBid e)
        {
            LivePrice.Bid = (double)e.BidPrice;          
        }

        private void datafeed_onBidask(object sender, DataFeed.aBidAsk e)
        {
            LivePrice.Bid = (double)e.BidPrice;
            LivePrice.Offer = (double)e.AskPrice;            
        }
    }
}
