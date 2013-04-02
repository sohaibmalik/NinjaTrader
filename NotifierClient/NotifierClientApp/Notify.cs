using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NotifierClientApp
{
    public partial class Notify : Form
    {
        AlsiWebService.AlsiNotifyService service = new AlsiWebService.AlsiNotifyService();
        AlsiWebService.xlTradeOrder lastOrder;
        private bool _updateApp1;
        private DateTime _app1LastUpdate = DateTime.Now.AddDays(-1);
        private int _OrderUpdate, _StatusUpdate, _StatusDelay;
        private delegate void StatusFail(AlsiWebService.Boodskap boodskap);
        private StatusFail onStatusFail;
        private NotifyIcon ni = new NotifyIcon();
        private Admin admin = new Admin();
        public Notify()
        {
            InitializeComponent();
            ni.BalloonTipClicked += new EventHandler(ni_BalloonTipClicked);
        }



        private void updateFromWeb(AlsiWebService.xlTradeOrder order)
        {
            Debug.WriteLine("-------------------------------------------------------");
            Debug.WriteLine(order.Timestamp + "  " + order.Price + "  " + order.Status);
            Debug.WriteLine("-------------------------------------------------------");
            ListViewItem lvi = new ListViewItem(order.Timestamp.ToLongTimeString());
            lvi.Tag = order;
            lvi.SubItems.Add(order.Contract);
            lvi.SubItems.Add(order.BS.ToString());
            lvi.SubItems.Add(order.Volume.ToString());
            lvi.SubItems.Add(order.Price.ToString());
            lvi.SubItems.Add(order.Status.ToString());
            updateListView(lvi, order);
            ColorStatus();
        }

        private void updateListView(ListViewItem lvi, AlsiWebService.xlTradeOrder order)
        {
            var ind = ordersListView.Items.Count;
            if (ind == 0)
            {
                ordersListView.Items.Add(lvi);
                lastOrder = (AlsiWebService.xlTradeOrder)lvi.Tag;
                return;
            }

            //Status Update
            if (lastOrder.Price == order.Price
                && lastOrder.Volume == order.Volume
                && lastOrder.BS == order.BS
                && lastOrder.Status != order.Status)
            {
                lastOrder = order;
                ordersListView.Items[ind - 1].Tag = order;
                ordersListView.Items[ind - 1].SubItems[5].Text = order.Status.ToString();
                return;
            }


            bool oldorder = (lastOrder.Price == order.Price
                 && lastOrder.Volume == order.Volume
                 && lastOrder.BS == order.BS
                 && lastOrder.Status == order.Status);

            //New Order
            if (!oldorder)
            {
                lastOrder = order;
                lvi.Tag = order;
                ordersListView.Items.Add(lvi);
            }



        }

        
        private DateTime _alertAcknowledged;
        private void ColorStatus()
        {
            foreach (ListViewItem i in ordersListView.Items)
            {
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Ready)
                {
                    var ordertime = ((AlsiWebService.xlTradeOrder)i.Tag).Timestamp;
                    i.BackColor = Color.DarkOrange;
                    if (_alertAcknowledged <= ordertime) balloonNotify(App.AlsiTrade, "New Order!");

                }
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Completed && admin.IsAdmin )
                {
                    var ordertime = ((AlsiWebService.xlTradeOrder)i.Tag).Timestamp;
                    i.BackColor = Color.LightGreen;
                    if (_alertAcknowledged <= ordertime) balloonNotify(App.AlsiTrade, "Order Matched!");

                }
            }

        }

        private void getAllorders()
        {
            try
            {
                var allOrders = service.getAllOrders();
                ordersListView.Items.Clear();
                foreach (var v in allOrders)
                    if (allOrders.Count() > 0)
                        foreach (var o in allOrders) updateFromWeb(o);

            }
            catch (Exception ex)
            {

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _StatusUpdate = Properties.Settings.Default.StatusUpdateInt;
            _OrderUpdate = Properties.Settings.Default.OrderUpdateInt;
            _StatusDelay = Properties.Settings.Default.StatusDelayInt;

            OrderUpdateToolStripTextBox.Text = _OrderUpdate.ToString();
            StatusUpdateToolStripTextBox.Text = _StatusUpdate.ToString();
            DelayUpdateToolStripTextBox.Text = _StatusDelay.ToString();

            CheckForIllegalCrossThreadCalls = false;
            OrderUpdateTimer.Enabled = true;
            OrderUpdateTimer.Interval = _OrderUpdate;
            OrderUpdateTimer.Start();

            StatusUpdateTimer.Enabled = true;
            StatusUpdateTimer.Interval = _StatusUpdate;
            StatusUpdateTimer.Start();
            statusLabel1.Text = "Trade Bot Online";

            SetAdminButtons(admin.IsAdmin);
            admin.ReportLiveStatus(true);
            AlsiUtils.WebSettings.GetSettings();

        }

        private void SetAdminButtons(bool vis)
        {
            adminToolStripMenuItem.Visible = vis;
            clearHistoryToolStripMenuItem.Enabled = vis;

        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateBW.RunWorkerAsync();
            }
            catch (Exception ee)
            { };
        }

        private void updateBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var o = service.getLastOrder();
                if (o != null) updateFromWeb(o);

            }
            catch (Exception ex)
            {

            }
        }

        private int alsiTradeFailedCount = 0;

        void statusFailed(App app, string Mesg)
        {

            if (app == App.AlsiTrade)
            {
                alsiTradeFailedCount++;
                Debug.WriteLine(app + " Failed  " + alsiTradeFailedCount);
            }

            if (alsiTradeFailedCount > 10 && admin.IsAdmin ) balloonNotify(App.AlsiTrade, "Failed to update !");


        }



        private void balloonNotify(App app, string Msg)
        {
            string Title = "";
            if (app == App.AlsiTrade)
            {
                Title = "Alsi Trade";
            }

            // (new SoundPlayer(Properties.Resources.alert3)).Play();
            ni.Visible = true;
            ni.Icon = Properties.Resources.alert;
            ni.ShowBalloonTip(1000, Title, Msg, ToolTipIcon.Info);

        }

        void ni_BalloonTipClicked(object sender, EventArgs e)
        {
            _alertAcknowledged = DateTime.UtcNow.AddHours(2);
            alsiTradeFailedCount = 0;
            ni.Visible = false;

        }

        private void getAllOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderUpdateTimer.Stop();
            getAllorders();
            OrderUpdateTimer.Start();
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            service.clearLists();
            ordersListView.Items.Clear();
        }

        enum App
        {
            AlsiTrade = 1,

        }

        private void app1Statustimer_Tick(object sender, EventArgs e)
        {
            try
            {
                appStatusBW.RunWorkerAsync();
                AppUpdateColors();
            }
            catch (Exception ex)
            {

            }
        }

        private void AppUpdateColors()
        {
            if (!_updateApp1)
            {
                statusFailed(App.AlsiTrade, "Failed");
                statusLabel1.BackColor = Color.Red;
            }
            else
            {
                statusLabel1.BackColor = Color.LightGreen;
                alsiTradeFailedCount = 0;
            }

        }



        private void appStatusBW_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                getAppUpdate();
                ordersListView.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                ordersListView.BackColor = Color.Red;
                _updateApp1 = false;
                service = new AlsiWebService.AlsiNotifyService();
            }
        }

        private void getAppUpdate()
        {
            var n = DateTime.UtcNow.AddHours(2);
            var b = service.getLastMessage();
            if (b.Message == AlsiWebService.Messages.isAlive) _app1LastUpdate = b.TimeStamp;
            Debug.WriteLine("Appupdate " + b.TimeStamp + "  " + b.Message);
            var check1 = _app1LastUpdate.AddSeconds(Properties.Settings.Default.StatusDelayInt);

            if (check1 > n)
                _updateApp1 = true;
            else
                _updateApp1 = false;

        }

        private void OrderUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(OrderUpdateToolStripTextBox.Text, out _OrderUpdate);
            Properties.Settings.Default.OrderUpdateInt = _OrderUpdate;
            OrderUpdateTimer.Interval = _OrderUpdate;
            Properties.Settings.Default.Save();
        }

        private void StatusUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(StatusUpdateToolStripTextBox.Text, out _StatusUpdate);
            Properties.Settings.Default.StatusUpdateInt = _StatusUpdate;
            StatusUpdateTimer.Interval = _StatusUpdate;
            Properties.Settings.Default.Save();
        }

        private void DelayUpdateToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(DelayUpdateToolStripTextBox.Text, out _StatusDelay);
            Properties.Settings.Default.StatusDelayInt = _StatusDelay;
            Properties.Settings.Default.Save();

        }

        private void tradeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!Communicator.Internet.CheckConnection())
            {
                MessageBox.Show("No Internet Connection", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TradeLogForm ts = new TradeLogForm(admin);
            ts.Show();
            Cursor = Cursors.Default;
        }

        private void getPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (!Communicator.Internet.CheckConnection())
            {
                MessageBox.Show("No Internet Connection", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var alsi = AlsiTrade_Backend.HiSat.HistData.GetHistoricalTICK_FromWEB(DateTime.Now, DateTime.Now, AlsiUtils.WebSettings.General.HISAT_INST);
            if (alsi.Count == 0)
            {
                pricesStatusLabel.Text = "Market Closed";
                return;
            }
            pricesStatusLabel.Text = alsi.Last().Close.ToString();
            Cursor = Cursors.Default;
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var a = new AdminForm(admin);
            a.Show();
        }

        private void Notify_FormClosing(object sender, FormClosingEventArgs e)
        {
            admin.ReportLiveStatus(false);
        }

       













    }
}
