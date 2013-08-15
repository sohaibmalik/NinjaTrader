using HISAT_API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AlsiUtils;
namespace ModernNotifier.Pages.Alsi
{
    /// <summary>
    /// Interaction logic for LiveDataPage.xaml
    /// </summary>
    public partial class LiveDataPage : UserControl
    {
        HISAT_API.DataFeed datafeed;
        private System.Timers.Timer t = new System.Timers.Timer();
        private string Instrument = "";
        public LiveDataPage()
        {
            InitializeComponent();
            datafeed = new DataFeed();

            datafeed.StatusUpdate += new DataFeed.StatusUpdateEventHandler(datafeed_StatusUpdate);
            datafeed.onTrade += new DataFeed.onTradeEventHandler(datafeed_onTrade);
            datafeed.onAsk += new DataFeed.onAskEventHandler(datafeed_onAsk);
            datafeed.onBid += new DataFeed.onBidEventHandler(datafeed_onBid);
            datafeed.onBidask += new DataFeed.onBidaskEventHandler(datafeed_onBidask);

            datafeed.LogOnToHisat("Johan", "Hisat");
            t.Elapsed += t_Elapsed;

            WebSettings.GetSettings();
            Instrument = WebSettings.General.HISAT_INST;
           // instrumentLabel.Text = Instrument;
        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
           
            datafeed.RequestInstrumentUpdate(Instrument);          
            t.Stop();
        }




        private void datafeed_StatusUpdate(object sender, DataFeed.HSEventArgs e)
        {
            Debug.WriteLine("STATUS UPDATE " + e.theString);
        }

        private void datafeed_onTrade(object sender, DataFeed.aTrade e)
        {
            Debug.WriteLine("TRADE" + e.TradePrice);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
           {
               Last.Text = e.TradePrice.ToString();
           }, null);
        }

        private void datafeed_onAsk(object sender, DataFeed.aAsk e)
        {
            Debug.WriteLine("ASK " + e.AskPrice);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
           {
               Offer.Text = e.AskPrice.ToString();
           }, null);
        }

        private void datafeed_onBid(object sender, DataFeed.aBid e)
        {
            Debug.WriteLine("BID " + e.BidPrice);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
           {
               Bid.Text = e.BidPrice.ToString();
           }, null);
        }

        private void datafeed_onBidask(object sender, DataFeed.aBidAsk e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            {
                Bid.Text = e.BidPrice.ToString();
                Offer.Text = e.AskPrice.ToString();
             
            }, null);
            Debug.WriteLine("BIDask " + " BID " + e.BidPrice + "  " + e.AskPrice + " ASK");
        }



        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            t.Interval = 2000;
            t.Start();
        }




    }
}
