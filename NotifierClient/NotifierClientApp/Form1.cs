using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NotifierClientApp
{
    public partial class Form1 : Form
    {
        AlsiWebService.AlsiNotifyService service = new AlsiWebService.AlsiNotifyService();
        AlsiWebService.xlTradeOrder lastOrder;

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
            

           bool oldorder =  (lastOrder.Price == order.Price
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
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                updateBW.RunWorkerAsync();
            }
            catch(Exception ee)
            {};
        }

        private void updateBW_DoWork(object sender, DoWorkEventArgs e)
        {
            var o = service.getLastOrder();
           if(o!=null) updateFromWeb(o);
           Debug.WriteLine("Checked");
        }

        private void getAllOrderButton_Click(object sender, EventArgs e)
        {
            updateTimer.Stop();
            getAllorders();
            updateTimer.Start();
        }

        private void clearAllOrderButton_Click(object sender, EventArgs e)
        {
            service.clearLists();
            ordersListView.Items.Clear();
        }

       

        
       


    }
}
