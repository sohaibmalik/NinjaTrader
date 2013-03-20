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
    public partial class NewOrderInput : Form
    {
        public NewOrderInput()
        {
            InitializeComponent();
        }

        private void NewOrderInput_Load(object sender, EventArgs e)
        {
            PopulateBoxes();
        }


        private void PopulateBoxes()
        {
          

            reasonComboBox.DataSource = GetReasons();
        }


        private string GetDateTimeString()
        {
            return DateTime.UtcNow.AddHours(2).ToString();
        }

        private void ValidateDateBox(string DateTime)
        {
                     
          
        }


        private List<string> GetReasons()
        {
            var reason=new List<string>();

            reason.Add("OpenLong");
            reason.Add("OpenShort");
            reason.Add("CloseLong");
            reason.Add("CloseShort");

            return reason;
        }

        private void priceSubmitTextBox_TextChanged(object sender, EventArgs e)
        {
            priceMatchedTextBox.Text = priceSubmitTextBox.Text;
        }

       
    }
}
