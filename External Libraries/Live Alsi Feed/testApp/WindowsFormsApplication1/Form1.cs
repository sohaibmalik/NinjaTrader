using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HISAT_API;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
	public partial class Form1 : Form
	{
		HISAT_API.DataFeed datafeed = new DataFeed();


		public Form1()
		{
			InitializeComponent();
			datafeed.StatusUpdate += new DataFeed.StatusUpdateEventHandler(datafeed_StatusUpdate);		
			datafeed.onTrade+=new DataFeed.onTradeEventHandler(datafeed_onTrade);
			datafeed.onAsk+=new DataFeed.onAskEventHandler(datafeed_onAsk);
			datafeed.onBid+=new DataFeed.onBidEventHandler(datafeed_onBid);
			datafeed.onBidask+=new DataFeed.onBidaskEventHandler(datafeed_onBidask);
			datafeed.onError+=new DataFeed.onErrorEventHandler(datafeed_onError);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			datafeed.LogOnToHisat("Johan", "Hisat");
			
		}


		private void datafeed_StatusUpdate(object sender, DataFeed.HSEventArgs e)
		{
			Debug.WriteLine("STATUS UPDATE " + e.theString);		
		}

		private void datafeed_onTrade(object sender, DataFeed.aTrade e)
		{
			Debug.WriteLine("TRADE" + e.TradePrice);

		}

		private void datafeed_onAsk(object sender, DataFeed.aAsk e)
		{
			Debug.WriteLine("ASK " + e.AskPrice);

		}

		private void datafeed_onBid(object sender, DataFeed.aBid e)
		{

			Debug.WriteLine("BID " + e.BidPrice);
		}

		private void datafeed_onBidask(object sender, DataFeed.aBidAsk e)
		{

			Debug.WriteLine("BIDask " + " BID " + e.BidPrice + "  " + e.AskPrice + " ASK");
		}

		private void button2_Click(object sender, EventArgs e)
		{			
				datafeed.RequestInstrumentUpdate("DEC12ALSI");			
		}

		private void datafeed_onError(object sender, DataFeed.HSEventArgs e)
		{
			Debug.WriteLine(e.theString);
		}

	}
}
