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
        WebDbDataContext dc = new WebDbDataContext();
        TradeLog _selectedLog;
        int price;
        public TradeLogForm()
        {
            InitializeComponent();
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

        private void tradeListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
           _selectedLog= (TradeLog)e.Item.Tag;
           matchedTextBox.Text = _selectedLog.PriceMatched.ToString();
        }

        private void matchedTextBox_TextChanged(object sender, EventArgs e)
        {
           
            if (int.TryParse(matchedTextBox.Text, out price))
            {
                saveButton.Enabled = true;
            }
            else
            {
                saveButton.Enabled = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            saveButton.Enabled = false;
            _selectedLog.PriceMatched = price;
            dc.SubmitChanges();
            LoadListview();
            Cursor = Cursors.Default;
        }
    }
}
