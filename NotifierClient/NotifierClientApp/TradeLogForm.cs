using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NotifierClientApp
{
    public partial class TradeLogForm : Form
    {
        private newLogSaved onUpdate;
        WebDbDataContext dc = new WebDbDataContext();
        TradeLog _selectedLog;
        int price;
        public TradeLogForm()
        {
            InitializeComponent();
            onUpdate = LoadListview;
        }

        private void TradeLogForm_Load(object sender, EventArgs e)
        {
            LoadListview();
        }

        private void LoadListview()
        {
            tradeListView.Items.Clear();
            var log = dc.TradeLogs.OrderByDescending(z => z.Time).Take(25);

            foreach (var t in log)
            {
                ListViewItem lvi = new ListViewItem(t.Time.ToString());
                lvi.Tag = t;
                lvi.SubItems.Add(t.BuySell);
                lvi.SubItems.Add(t.Price.ToString());
                lvi.SubItems.Add(t.Reason);
                lvi.SubItems.Add(t.Volume.ToString());
                lvi.SubItems.Add(t.Matched.ToString());
                lvi.SubItems.Add(t.PriceMatched.ToString());
                tradeListView.Items.Add(lvi);
            }
        }

     
       

    

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string matchedSt = "";
            if (_selectedLog.Matched) matchedSt = "Matched @ " + _selectedLog.PriceMatched;
            else
                matchedSt = "Not Macthed";
            string dial = _selectedLog.Time.ToString() + "  " + _selectedLog.BuySell + " " + _selectedLog.Volume + " @ " + _selectedLog.Price + " " +matchedSt;

            DialogResult dr = MessageBox.Show("Delete :\n" + dial, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                Cursor = Cursors.Default;
                return;
            }
            dc.TradeLogs.DeleteOnSubmit(_selectedLog);
            dc.SubmitChanges();
            LoadListview();
            Cursor = Cursors.Default;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            var n = new NewOrderInput(onUpdate);
            n.Show();
        }

        private void tradeListView_DoubleClick(object sender, EventArgs e)
        {
            var tl = (TradeLog)tradeListView.SelectedItems[0].Tag;
            var n = new NewOrderInput(onUpdate,tl);
            n.Show();
        }

        private void tradeListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (tradeListView.SelectedItems.Count == 0) return;
            _selectedLog = (TradeLog)tradeListView.SelectedItems[0].Tag;
            deleteButton.Enabled = true;
        }

    }
}
