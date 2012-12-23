using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
using AlsiUtils.Data_Objects;
using AlsiUtils.Strategies;
using AlsiUtils;
using System.Diagnostics;


namespace FrontEnd
{
    public partial class MainForm : Form
    {
        private PrepareForTrade p;
        private UpdateTimer U5;
        MarketOrder marketOrder;
        GlobalObjects.TimeInterval _Interval;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _Interval = GlobalObjects.TimeInterval.Minute_5;
            AlsiUtils.DataBase.SetConnectionString();
            PopulateControls();
            U5 = new UpdateTimer(_Interval);
            p = new PrepareForTrade();
            marketOrder = new MarketOrder();
            p.onPriceSync += new PrepareForTrade.PricesSynced(p_onPriceSync);
            U5.onStartUpdate += new UpdateTimer.StartUpdate(U5_onStartUpdate);
            marketOrder.onOrderSend += new MarketOrder.OrderSend(marketOrder_onOrderSend);
            marketOrder.onOrderMatch += new MarketOrder.OrderMatch(marketOrder_onOrderMatch);
        }

        void marketOrder_onOrderMatch(object sender, MarketOrder.OrderMatchEvent e)
        {
            Debug.WriteLine("MATCHED : " + e.Success);
            Debug.WriteLine("Trade : " + e.Trade.TimeStamp + "  " + e.Trade.BuyorSell + "  " + e.Trade.CurrentDirection + "  " + e.Trade.TradeVolume);
        }

        void marketOrder_onOrderSend(object sender, MarketOrder.OrderSendEvent e)
        {
            Debug.WriteLine("Success : " + e.Success);
            Debug.WriteLine("Trade : " + e.Trade.TimeStamp + "  " + e.Trade.BuyorSell + "  " + e.Trade.CurrentDirection + "  " + e.Trade.TradeVolume);
        }

        void U5_onStartUpdate(object sender, UpdateTimer.StartUpDateEvent e)
        {
            Debug.WriteLine(e.Message + "  " + e.Interval);
            Debug.WriteLine("Getting priced");
            p.GetPricesFromWeb(_Interval);
        }

        void p_onPriceSync(object sender, PrepareForTrade.PricesSyncedEvent e)
        {
            Debug.WriteLine("Prices Synced : " + e.ReadyForTradeCalcs);
            var trades = RunCalcs.RunEMAScalpLiveTrade(GetParameters(), _Interval, false);
            var lt = trades.Last();
            Debug.WriteLine("====================");
            Debug.WriteLine("===Current Trade");
            Debug.WriteLine(lt.TimeStamp + "  " + lt.CurrentPrice + "  " + lt.CurrentDirection + "  " + lt.BuyorSell);
            marketOrder.SendOrderToMarket(lt, 2, "ALSI Contract");
        }

        private Parameter_EMA_Scalp GetParameters()
        {
            AlsiUtils.Strategies.Parameter_EMA_Scalp E = new AlsiUtils.Strategies.Parameter_EMA_Scalp()
            {
                A_EMA1 = Properties.Settings.Default.A1,
                A_EMA2 = Properties.Settings.Default.A2,
                B_EMA1 = Properties.Settings.Default.B1,
                B_EMA2 = Properties.Settings.Default.B2,
                C_EMA = Properties.Settings.Default.C,
                TakeProfit = Properties.Settings.Default.PROF,
                StopLoss = Properties.Settings.Default.STOP,
                CloseEndofDay = false,
                //Period = GlobalObjects.Prices.Count,
            };

            return E;
        }

        private void PopulateControls()
        {
            emaA1TextBox.Text = Properties.Settings.Default.A1.ToString();
            emaA2TextBox.Text = Properties.Settings.Default.A2.ToString();
            emaB1TextBox.Text = Properties.Settings.Default.B1.ToString();
            emaB2TextBox.Text = Properties.Settings.Default.B2.ToString();
            emaCTextBox.Text = Properties.Settings.Default.C.ToString();
            stopLossTextBox.Text = Properties.Settings.Default.STOP.ToString();
            takeProfTextBox.Text = Properties.Settings.Default.PROF.ToString();
            tradeVolTextBox.Text = Properties.Settings.Default.VOL.ToString();
            hiSatInstrumentTextBox.Text = Properties.Settings.Default.HISAT_INST;
            otsInstrumentTextBox.Text = Properties.Settings.Default.OTS_INST;

            endDateTimePicker.Value = DateTime.Now;
            startDateTimePicker.Value = DateTime.Now.AddDays(-500);
            endDateTimePicker.MaxDate = DateTime.Now;
        }

      

        private List<Trade> _tempTradeList = new List<Trade>();
        private void runHistCalcButton_Click(object sender, EventArgs e)
        {
            GlobalObjects.TimeInterval t = GlobalObjects.TimeInterval.Minute_1;
            if (_2minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_2;
            if (_5minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_5;
            if (_10minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_10;

            DataBase.dataTable dt;
            if(fullTradesRadioButton.Checked)dt=DataBase.dataTable.MasterMinute;
            else dt=DataBase.dataTable.AllHistory;
                     
             _tempTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(GetParameters(), t, onlyTradesRadioButton.Checked, startDateTimePicker.Value, endDateTimePicker.Value, dt);
                      

        }

     
    }
}
