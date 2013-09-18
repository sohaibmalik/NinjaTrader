using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace NinjaTest
{
    public partial class Form4 : Form
    {
        private LuanchScalp l = new LuanchScalp();
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            l.RunSingleNEW();
            Close();
        }

        public void UpdatePos(string msg)
        {
            Debug.WriteLine(msg);
        }

        public void UpdateDisplay(string msg)
        {
            Debug.WriteLine(msg);
        }
    }
}
