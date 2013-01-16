using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NinjaTest
{
    public partial class Form4 : Form
    {
        private Luanch l = new Luanch();
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            l.RunSingleNEW();
            Close();
        }
    }
}
