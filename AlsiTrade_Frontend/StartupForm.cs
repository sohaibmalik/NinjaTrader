using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
namespace FrontEnd
{
    public partial class StartupForm : Form
    {
        private MainForm main = new MainForm();
      

        public StartupForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;            
        }

     

        private void StartupForm_Load(object sender, EventArgs e)
        {
                        this.Shown += new EventHandler(StartupForm_Shown);
            main.Shown += new EventHandler(main_Shown);
            main.onStartupLoad += new MainForm.LoadingStartup(main_onStartupLoad);           
        }


        void main_onStartupLoad(object sender, MainForm.LoadingStartupEvent e)
        {
            progressBar1.Value = e.Progress;
            progressBar1.Refresh();

        }



        void StartupForm_Shown(object sender, EventArgs e)
        {
            main.Show();
        }

        void main_Shown(object sender, EventArgs e)
        {
            Visible = false;

        }
    }

  
}
