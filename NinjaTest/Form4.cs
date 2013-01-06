using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;

namespace NinjaTest
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            AlsiUtils.DataBase.SetConnectionString();
            DoStuff.GetDataFromTick.DoYourThing("MAR13ALSI");
        }
    }
}
