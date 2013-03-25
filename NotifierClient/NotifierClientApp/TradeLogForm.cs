using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            dc = new WebDbDataContext();
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
            string dial = _selectedLog.Time.ToString() + "  " + _selectedLog.BuySell + " " + _selectedLog.Volume + " @ " + _selectedLog.Price + " " + matchedSt;

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
            var n = new NewOrderInput(onUpdate, dc);
            n.Show();
        }

        private void tradeListView_DoubleClick(object sender, EventArgs e)
        {
            var tl = (TradeLog)tradeListView.SelectedItems[0].Tag;
            var n = new NewOrderInput(onUpdate, tl, dc);
            n.Show();
        }

        private void tradeListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (tradeListView.SelectedItems.Count == 0) return;
            _selectedLog = (TradeLog)tradeListView.SelectedItems[0].Tag;
            deleteButton.Enabled = true;
        }

        private void chartButton_Click(object sender, EventArgs e)
        {
            AlsiCharts.MultiAxis_2 c = new AlsiCharts.MultiAxis_2();

            var diff = new List<double>();
            var totDiff = new List<double>();
            double sum = 0;
            var DC=dc.TradeLogs.Where(z => z.Matched == true && z.Price != 0 && z.PriceMatched != 0);
            foreach (var l in DC)
            {
                if (l.Reason == "OpenLong" || l.Reason == "CloseShort")
                {
                    var d = l.Price - l.PriceMatched;
                    diff.Add(d);
                    sum += d;
                    totDiff.Add(sum);
                }
                if (l.Reason == "CloseLong" || l.Reason == "OpenShort")
                {
                    var d =  l.PriceMatched-l.Price;
                    diff.Add(d);
                    sum += d;
                    totDiff.Add(sum);
                }
            }


            c.SharedTootltip = true;
            c.Title = "Difference between theoritical and actual trades";
            c.Subtitle = "Higher values are good";
            c.XaxisLabels = DC.Select(z => z.Time.ToString()).ToList();
            c.Height = chartsize;

            c.Series_A.Data = diff;
            c.Series_B.Data = totDiff;

            c.Series_A.YaxisNumber = 0;
            c.Series_B.YaxisNumber = 1;

            c.Series_A.YaxisUnitColor = Color.SteelBlue;
            c.Series_A.YaxisTitleColor = Color.SteelBlue;

            c.Series_B.YaxisUnitColor = Color.MediumVioletRed;
            c.Series_B.YaxisTitleColor = Color.MediumVioletRed;

            c.Series_B.AxisOppositeSide = true;

            c.Series_A.YaxixLabel = "Difference";
            c.Series_B.YaxixLabel = "Accumilating Difference";
          
            c.Series_A.Unit = "pts";
            c.Series_B.Unit = "pts";

           


            c.PopulateScript();
            c.OutputFileName = "TradeDifference";
            c.ShowChartInBrowser();
            c.UploadFile();
        }
        private int chartsize = 750;
        private void chartSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(chartSizeTextBox.Text, out chartsize);
        }


    }
}
