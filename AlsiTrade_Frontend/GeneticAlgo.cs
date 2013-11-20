using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd
{
    public partial class GeneticAlgoForm : Form
    {
        public GeneticAlgoForm()
        {
            InitializeComponent();
        }

        public void WriteText(string Msg)
        {
            outputTextBox.AppendText(Msg);
        }

    }
}
