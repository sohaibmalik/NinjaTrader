using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NotifierClientApp
{
    public partial class Form1 : Form
    {
        AlsiWebService.AlsiNotifyService service = new AlsiWebService.AlsiNotifyService();
        AlsiWebService.xlTradeOrder lastOrder;
        private bool _updateApp1;
        private bool _updateApp2;
        private DateTime _app1LastUpdate = DateTime.Now.AddDays(-1);
        private DateTime _app2LastUpdate = DateTime.Now.AddDays(-1);

        public Form1()
        {
            InitializeComponent();
        }

        private void updateFromWeb(AlsiWebService.xlTradeOrder order)
        {
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
        private void ColorStatus()
        {
            foreach (ListViewItem i in ordersListView.Items)
            {
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Ready) i.BackColor = Color.DarkOrange;
                if (((AlsiWebService.xlTradeOrder)i.Tag).Status == AlsiWebService.orderStatus.Completed) i.BackColor = Color.LightGreen;
            }

        }

        private void getAllorders()
        {
            var allOrders = service.getAllOrders();
            ordersListView.Items.Clear();
            if (allOrders.Count() > 0)
                foreach (var o in allOrders) updateFromWeb(o);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            updateTimer.Enabled = true;
            updateTimer.Interval = 5000;
            updateTimer.Start();

            app1Statustimer.Enabled = true;
            app1Statustimer.Interval = 60000;
            app1Statustimer.Start();
            statusLabel1.Text = "AlsiTrade";
            statusLabel2.Text = "DataManager";


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
            var o = service.getLastOrder();
            if (o != null) updateFromWeb(o);
            Debug.WriteLine("Checked");
        }





        private void getAllOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateTimer.Stop();
            getAllorders();
            updateTimer.Start();
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            service.clearLists();
            ordersListView.Items.Clear();
        }



        private void app1Statustimer_Tick(object sender, EventArgs e)
        {
            appStatusBW.RunWorkerAsync();

            AppUpdateColors();
        }

        private void AppUpdateColors()
        {
            if (!_updateApp1) statusLabel1.BackColor = Color.Red;
            else
                statusLabel1.BackColor = Color.LightGreen;

            if (!_updateApp2) statusLabel2.BackColor = Color.Red;
            else
                statusLabel2.BackColor = Color.LightGreen;
        }



        private void appStatusBW_DoWork(object sender, DoWorkEventArgs e)
        {
            getAppUpdate();

        }

        private void getAppUpdate()
        {
            var n = DateTime.Now;

            var b = service.getLastMessage();
            if (b.Message == "AlsiTrade") _app1LastUpdate = b.TimeStamp;
            if (b.Message == "DataManager") _app2LastUpdate = b.TimeStamp;

            Debug.WriteLine("Appupdate" + b.TimeStamp + "  " + b.Message);

            var check1 = _app1LastUpdate.AddSeconds(120);
            var check2 = _app2LastUpdate.AddSeconds(120);

            if (check1 > n) _updateApp1 = true;
            else
                _updateApp1 = false;

            if (check2 > n) _updateApp2 = true;
            else
                _updateApp2 = false;
        }










    }
}
