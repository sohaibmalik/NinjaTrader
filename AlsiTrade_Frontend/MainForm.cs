using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AlsiTrade_Backend;
using AlsiUtils;
using AlsiUtils.Data_Objects;
using AlsiUtils.Strategies;
using BrightIdeasSoftware;

namespace FrontEnd
{
   
    public partial class MainForm : Form
    {
        private PrepareForTrade p;
        private UpdateTimer U5;
        MarketOrder marketOrder;
        GlobalObjects.TimeInterval _Interval;

        private OLVColumn ColStamp;
        private OLVColumn ColReason;
        private OLVColumn ColBuySell;
        private OLVColumn ColNotes;
        private OLVColumn ColPosition;
        private OLVColumn ColRunningProf;
        private OLVColumn ColCurrentDirection;
        private OLVColumn ColCurrentPrice;
        private OLVColumn ColTotalProf;
        private OLVColumn ColInstrument;
       

        DateTime masterStart, masterEnd, allhistoStart, allhistoEnd;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
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
            BuildListViewColumns();
          
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
            p.GetPricesFromWeb(_Interval, Properties.Settings.Default.HISAT_INST);
        }

        void p_onPriceSync(object sender, PrepareForTrade.PricesSyncedEvent e)
        {
            Debug.WriteLine("Prices Synced : " + e.ReadyForTradeCalcs);
            if (e.ReadyForTradeCalcs)
            {

                var trades = RunCalcs.RunEMAScalpLiveTrade(GetParameters(), _Interval);
                var lt = trades.Last();
                Debug.WriteLine(lt.Notes);
                lt.TradeVolume = 2;
                marketOrder.SendOrderToMarket(lt);
                UpdateTradeLog(SetTradeLogColor(lt));
            }
            else
            {
                return;
                System.Threading.Thread.Sleep(3000);
                // GlobalObjects.Prices.Reverse();
                p.GetPricesFromWeb(_Interval, Properties.Settings.Default.HISAT_INST);
            }

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
            runningMinuteTooltripLabel.Text = _Interval.ToString().Replace("_", " ");
            endDateTimePicker.Value = DateTime.Now;
            startDateTimePicker.Value = DateTime.Now.AddDays(-50);
            endDateTimePicker.MaxDate = DateTime.Now;
        }

        private void BuildListViewColumns()
        {
            
            ColStamp = new OLVColumn
            {
                Name = "cStamp",
                AspectName = "TimeStamp",
                Text = "Time",
                Groupable = false,
                IsVisible = true,
                Width = 150
            };
            histListview.AllColumns.Add(ColStamp);

            ColReason = new OLVColumn
            {
                Name = "cReason",
                AspectName = "Reason",
                Text = "Reason",
                IsVisible = true,
                Width = 70
            };
            histListview.AllColumns.Add(ColReason);

            ColBuySell = new OLVColumn
            {
                Name = "cBuySell",
                AspectName = "BuyorSell",
                Text = "Buy or Sell",
                IsVisible = true,
                Width = 70
            };
            histListview.AllColumns.Add(ColBuySell);

           

            ColCurrentDirection = new OLVColumn
             {
                 Name = "cDirection",
                 AspectName = "CurrentDirection",
                 Text = "Direction",
                 IsVisible = true,
                 Width = 60
             };
            histListview.AllColumns.Add(ColCurrentDirection);

            ColCurrentPrice = new OLVColumn
            {
                Name = "cPrice",
                AspectName = "CurrentPrice",
                Text = "Price",
                IsVisible = true,
                Width = 50
            };
            histListview.AllColumns.Add(ColCurrentPrice);

            ColPosition = new OLVColumn
            {
                Name = "cPosition",
                ShowTextInHeader = false,
                AspectName = "Position",
                Text = "Position",
                Sortable = false,
                Groupable = false,
                Width = 60
            };
            histListview.AllColumns.Add(ColPosition);

            ColRunningProf = new OLVColumn
            {
                Name = "cRunningProfit",
                AspectName = "RunningProfit",
                Text = "Running Profit",
                IsVisible = true,
                Width = 50
            };
            histListview.AllColumns.Add(ColRunningProf);

            ColTotalProf = new OLVColumn
            {
                Name = "cTotalProf",
                AspectName = "TotalPL",
                Text = "Total Profit",
                IsVisible = true,
                Width = 50
            };
            histListview.AllColumns.Add(ColTotalProf);

            ColInstrument = new OLVColumn
            {
                Name = "cInstrument",
                AspectName = "InstrumentName",
                Text = "Instrument",
                IsVisible = true,
                Width = 80
            };
            histListview.AllColumns.Add(ColInstrument);


            ColPosition.ImageGetter = rowObject => ((Trade)rowObject).Position ? "add" : "add";

        }       
                
        private void UpdateTradeLog(Trade t)
        {


            ListViewItem lvi = new ListViewItem(DateTime.UtcNow.AddHours(2).ToString());
            lvi.SubItems.Add(t.BuyorSell.ToString());
            lvi.SubItems.Add(t.Reason.ToString());
            lvi.SubItems.Add(t.TradedPrice.ToString());
            lvi.SubItems.Add(t.TradeVolume.ToString());
            lvi.ForeColor = t.ForeColor;
            lvi.BackColor = t.BackColor;
            liveTradeListView.Items.Add(lvi);


            DataBase.InsertTradeLog(t);




        }

        private Trade SetTradeLogColor(Trade t)
        {

            switch (t.Reason)
            {
                case Trade.Trigger.None:
                    t.ForeColor = Color.DarkGray;
                    t.BackColor = Color.White;
                    break;

                case Trade.Trigger.OpenLong:
                    t.ForeColor = Color.Green;
                    t.BackColor = Color.White;
                    break;

                case Trade.Trigger.CloseLong:
                    t.ForeColor = Color.Green;
                    t.BackColor = Color.WhiteSmoke;
                    break;

                case Trade.Trigger.OpenShort:
                    t.ForeColor = Color.Red;
                    t.BackColor = Color.White;
                    break;


                case Trade.Trigger.CloseShort:
                    ForeColor = Color.Red;
                    t.BackColor = Color.WhiteSmoke;
                    break;


            }
            return t;
        }

        private void LoadTradeLog(bool todayOnly, bool onlyTrades, int periods)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            liveTradeListView.Items.Clear();

            IQueryable<AlsiUtils.TradeLog> tl = null;
            int count = dc.TradeLogs.Count();
            if (periods > count) return;

            if (todayOnly)
            {
                if (onlyTrades)
                {
                    tl = from x in dc.TradeLogs
                         where x.Time >= DateTime.UtcNow.AddHours(3)
                         where x.BuySell != "None"
                         select x;
                }
                else
                {
                    tl = from x in dc.TradeLogs
                         where x.Time >= DateTime.UtcNow.AddHours(3)
                         select x;
                }
            }
            else
            {
                if (onlyTrades)
                {
                    tl = (from x in dc.TradeLogs
                          select x).Skip(count - periods);

                    tl = tl.Where(z => z.BuySell != "None");
                }
                else
                {
                    tl = (from x in dc.TradeLogs
                          select x).Skip(count - periods);
                }
            }




            foreach (var v in tl)
            {
                Trade t = new Trade()
                {
                    TimeStamp = (DateTime)v.Time.Value,
                    TradeVolume = (int)v.Volume,
                    BuyorSell = Trade.BuySellFromString(v.BuySell),
                    CurrentPrice = (double)v.Price,
                    ForeColor = Color.FromName(v.ForeColor),
                    BackColor = Color.FromName(v.BackColor),

                };
                UpdateTradeLog(t);
            }
        }



        private List<Trade> _tempTradeList = new List<Trade>();
        private void runHistCalcButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            exportToTextButton.Enabled = false;
            GlobalObjects.TimeInterval t = GlobalObjects.TimeInterval.Minute_1;
            if (_2minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_2;
            if (_5minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_5;
            if (_10minRadioButton.Checked) t = GlobalObjects.TimeInterval.Minute_10;

            DataBase.dataTable dt;
            if (recentRadioButton.Checked)
            {
                dt = DataBase.dataTable.MasterMinute;
            }
            else
            { dt = DataBase.dataTable.AllHistory; }

            _tempTradeList = AlsiTrade_Backend.RunCalcs.RunEMAScalp(GetParameters(), t, onlyTradesRadioButton.Checked, startDateTimePicker.Value, endDateTimePicker.Value, dt);
            _tempTradeList.Reverse();
            histListview.Items.Clear();
            histListview.RebuildColumns();
            histListview.SetObjects(_tempTradeList);
            Cursor = Cursors.Default;
            exportToTextButton.Enabled = true;
        }

        private void loadTradeLogButton_Click(object sender, EventArgs e)
        {
            LoadTradeLog(loadTodayRadioButton.Checked, tradelogOnlyTradesCheckBox.Checked, (int)loadTradeLogNumericUpDown.Value);
        }

        private void loadNumberRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (loadNumberRadioButton.Checked) loadTradeLogNumericUpDown.Enabled = true;
            else loadTradeLogNumericUpDown.Enabled = false;
        }

        private void emaA1TextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(emaA1TextBox.Text, out em))
            {
                Properties.Settings.Default.A1 = em;
                Properties.Settings.Default.Save();
            }

        }

        private void emaA2TextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(emaA2TextBox.Text, out em))
            {
                Properties.Settings.Default.A2 = em;
                Properties.Settings.Default.Save();
            }
        }

        private void emaB1TextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(emaB1TextBox.Text, out em))
            {
                Properties.Settings.Default.B1 = em;
                Properties.Settings.Default.Save();
            }
        }

        private void emaB2TextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(emaB2TextBox.Text, out em))
            {
                Properties.Settings.Default.B2 = em;
                Properties.Settings.Default.Save();
            }
        }

        private void emaCTextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(emaCTextBox.Text, out em))
            {
                Properties.Settings.Default.C = em;
                Properties.Settings.Default.Save();
            }
        }

        private void fullHistoUpdateButton_Click(object sender, EventArgs e)
        {
            masterStart = new DateTime();
            Cursor = Cursors.WaitCursor;
            AlsiTrade_Backend.UpdateDB.FullHistoricUpdate_MasterMinute(Properties.Settings.Default.HISAT_INST);
            Cursor = Cursors.Default;
        }

        private void dataTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 2) settingsTabControl.SelectedIndex = 2;
        }

        private void LoadDBPrices_Click(object sender, EventArgs e)
        {

            Cursor = Cursors.WaitCursor;
            DataBase.dataTable tb = DataBase.dataTable.Temp;
            if (TableMastrMinRadioButton.Checked) tb = DataBase.dataTable.MasterMinute;
            if (TableAllHistMinRadioButton.Checked) tb = DataBase.dataTable.AllHistory;
            ViewDBPrices((int)VieDBNumericUpDown.Value, tb);
            Cursor = Cursors.Default;

        }

        private void ViewDBPrices(int N, DataBase.dataTable Table)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();

            if (Table == DataBase.dataTable.MasterMinute)
            {
                int count = dc.MasterMinutes.Count();
                DBPricesGridView.DataSource = dc.MasterMinutes.Skip(count - N).OrderByDescending(z => z.Stamp);
            }

            if (Table == DataBase.dataTable.AllHistory)
            {
                int count = dc.AllHistoricMinutes.Count();
                DBPricesGridView.DataSource = dc.AllHistoricMinutes.Skip(count - N).OrderByDescending(z => z.Stamp);
            }

        }

        private void TableAllHistMinRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            int x = dc.AllHistoricMinutes.Count();
            VieDBNumericUpDown.Maximum = x;
            dbPriceCountLabel.Text = "Datapoints : " + x.ToString();
        }

        private void TableMastrMinRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            AlsiDBDataContext dc = new AlsiDBDataContext();
            int x = dc.MasterMinutes.Count();
            VieDBNumericUpDown.Maximum = x;
            dbPriceCountLabel.Text = "Datapoints : " + x.ToString();
        }

        private void settingsTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 2) dataTabControl.SelectedIndex = 2;
        }

        private void UpdateAllHistoPrices_Click(object sender, EventArgs e)
        {
            masterStart = new DateTime();
            allhistoStart = new DateTime();
            Cursor = Cursors.WaitCursor;
            AlsiDBDataContext dc = new AlsiDBDataContext();
            dc.UpadteAllHistory();
            Cursor = Cursors.Default;
        }

        private void tradeVolTextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(tradeVolTextBox.Text, out em))
            {
                Properties.Settings.Default.VOL = em;
                Properties.Settings.Default.Save();
            }
        }

        private void takeProfTextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(takeProfTextBox.Text, out em))
            {
                Properties.Settings.Default.PROF = em;
                Properties.Settings.Default.Save();
            }
        }

        private void stopLossTextBox_TextChanged(object sender, EventArgs e)
        {
            int em = 0;
            if (int.TryParse(stopLossTextBox.Text, out em))
            {
                Properties.Settings.Default.STOP = em;
                Properties.Settings.Default.Save();
            }
        }

        private void hiSatInstrumentTextBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.HISAT_INST = hiSatInstrumentTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void otsInstrumentTextBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.OTS_INST = otsInstrumentTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void recentRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (masterStart.Year == 1)
            {
                AlsiDBDataContext dc = new AlsiDBDataContext();
                masterStart = dc.MasterMinutes.AsEnumerable().First().Stamp;
                masterEnd = dc.MasterMinutes.AsEnumerable().Last().Stamp;
                startDateTimePicker.MinDate = masterStart;
                endDateTimePicker.MaxDate = masterEnd;
                startDateTimePicker.Refresh();
                endDateTimePicker.Refresh();
            }
            if (recentRadioButton.Checked)
            {
                startDateTimePicker.MinDate = masterStart;
                endDateTimePicker.MaxDate = masterEnd;
                if (startDateTimePicker.Value < masterStart) startDateTimePicker.Value = masterStart;

                startDateTimePicker.Refresh();
                endDateTimePicker.Refresh();
            }
            Cursor = Cursors.Default;
        }

        private void AllHistoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (allhistoStart.Year == 1)
            {
                AlsiDBDataContext dc = new AlsiDBDataContext();
                allhistoStart = dc.AllHistoricMinutes.AsEnumerable().First().Stamp;
                allhistoEnd = dc.AllHistoricMinutes.AsEnumerable().Last().Stamp;
                startDateTimePicker.MinDate = allhistoStart;
                endDateTimePicker.MaxDate = allhistoEnd;
                startDateTimePicker.Refresh();
                endDateTimePicker.Refresh();
            }
            if (AllHistoRadioButton.Checked)
            {
                startDateTimePicker.MinDate = allhistoStart;
                endDateTimePicker.MaxDate = allhistoEnd;
                startDateTimePicker.Refresh();
                endDateTimePicker.Refresh();
            }

            Cursor = Cursors.Default;
        }

        private void histListview_CellClick(object sender, CellClickEventArgs e)
        {
            var z = (Trade)histListview.GetSelectedObject();
            Debug.WriteLine(z.Position);
        }

        private void exportToTextButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DoStuff.ExportToText(_tempTradeList);
            Cursor = Cursors.Default;
        }

      

    }
}
