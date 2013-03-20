using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NotifierClientApp
{
    public delegate void newLogSaved();

    public partial class NewOrderInput : Form
    {
        private newLogSaved _onSaved;
        private TradeLog tl;
        TradeLog EX;
        public NewOrderInput(newLogSaved Updatedeleagte)
        {
            InitializeComponent();
            _onSaved = Updatedeleagte;

        }

        private bool loadExisting;
        public NewOrderInput(newLogSaved Updatedeleagte, TradeLog Log)
        {
            InitializeComponent();
            _onSaved = Updatedeleagte;
            loadExisting = true;
            EX = Log;

        }

        private void PopuilateFromExisting(TradeLog ex)
        {

            dateTimePicker.Value = ex.Time.Date;
            timeTimePicker.Value = ex.Time;
            reasonComboBox.SelectedIndex = GetComboboxSelectedItemIndex(ex.Reason);
            volNum.Value = ex.Volume;
            matchedCheckBox.Checked = ex.Matched;
            priceSubmitTextBox.Text = ex.Price.ToString();
            priceMatchedTextBox.Text = ex.PriceMatched.ToString();

            saveButton.Enabled = false;
            loadExisting = false;
        }

        private int GetComboboxSelectedItemIndex(string value)
        {
            int index = 0;
            foreach (var v in reasonComboBox.Items)
            {
                if (v.ToString() == value) return index;
                index++;
            }
            return index;
        }

        private void NewOrderInput_Load(object sender, EventArgs e)
        {
            PopulateBoxes();
            if (loadExisting) PopuilateFromExisting(EX);
        }


        private void PopulateBoxes()
        {
            reasonComboBox.DataSource = GetReasons();
        }


        private string GetDateTimeString()
        {
            return DateTime.UtcNow.AddHours(2).ToString();
        }

        private bool ValidateData(out TradeLog Log)
        {
            Log = new TradeLog();
            if (loadExisting) return false;
            int price, priceMatched;
            int.TryParse(priceSubmitTextBox.Text, out price);
            int.TryParse(priceMatchedTextBox.Text, out priceMatched);
            bool matched = IsMatched();
            matchedCheckBox.Checked = matched;
            string reason = reasonComboBox.SelectedValue.ToString();
            string BS = (reason == "OpenLong" || reason == "CloseShort") ? "Buy" : "Sell";
            var Time = new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day, timeTimePicker.Value.Hour, timeTimePicker.Value.Minute, 0, 0);
            if (price < 10000) return false;

            Log = new TradeLog
            {
                Time = Time,
                Reason = reason,
                BuySell = BS,
                Price = price,
                PriceMatched = matched ? priceMatched : 0,
                Volume = (int)volNum.Value,
                Matched = matched,
            };





            return true;
        }

        private bool IsMatched()
        {
            int p;
            int.TryParse(priceMatchedTextBox.Text, out p);
            return (p > 10000);
        }

        private List<string> GetReasons()
        {
            var reason = new List<string>();

            reason.Add("OpenLong");
            reason.Add("OpenShort");
            reason.Add("CloseLong");
            reason.Add("CloseShort");

            return reason;
        }

        private void priceSubmitTextBox_TextChanged(object sender, EventArgs e)
        {
            priceMatchedTextBox.Text = priceSubmitTextBox.Text;

            saveButton.Enabled = ValidateData(out tl);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var dc = new WebDbDataContext();
            if (EX == null) dc.TradeLogs.InsertOnSubmit(tl);
            else
            {
                EX.BuySell = tl.BuySell;
                EX.Matched = tl.Matched;
                EX.Price = tl.Price;
                EX.PriceMatched = tl.PriceMatched;
                EX.Price = tl.Price;
                EX.Reason = tl.Reason;
                EX.Time = tl.Time;
                EX.Volume = tl.Volume;
                
            }
            dc.SubmitChanges();
            _onSaved();
            saveButton.Enabled = false;
        }




        private void priceMatchedTextBox_TextChanged(object sender, EventArgs e)
        {

            saveButton.Enabled = ValidateData(out tl);
        }

        private void reasonComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (loadExisting) return;
            saveButton.Enabled = ValidateData(out tl);
        }

        private void volNum_ValueChanged(object sender, EventArgs e)
        {
            ValidateData(out tl);
        }



    }
}
