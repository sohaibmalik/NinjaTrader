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


        private string laptop = @"Data Source=ALSI-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
        private string pc = @"Data Source=PIETER-PC\;Initial Catalog=AlsiTrade;Integrated Security=True";
        private string custom = "";
        private void StartupForm_Load(object sender, EventArgs e)
        {
         
            pcStringRadioButton.Text = pc;
            laptopStringRadioButton.Text = laptop;
            pcStringRadioButton.BackColor = Color.Yellow;
            laptopStringRadioButton.BackColor = Color.Yellow;
        
            if (AlsiUtils.DataBase.TestSqlConnection(Properties.Settings.Default.ConnectionString))
            {
                this.Shown += new EventHandler(StartupForm_Shown);
                main.Shown += new EventHandler(main_Shown);
                main.onStartupLoad += new MainForm.LoadingStartup(main_onStartupLoad);
                AlsiUtils.DataBase.SetConnectionString(Properties.Settings.Default.ConnectionString);
            }
            else
            {
                this.Size = new Size(612, 167);
            }
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

        private void pcStringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (pcStringRadioButton.Checked)
                if (AlsiUtils.DataBase.TestSqlConnection(pc))
                {
                    pcStringRadioButton.BackColor = Color.Green;
                    Properties.Settings.Default.ConnectionString = pc;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Sql connection made.\nPlease Restart App");
                    Environment.Exit(0);
                }
                else
                    pcStringRadioButton.BackColor = Color.Red;
        }

        private void laptopStringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (laptopStringRadioButton.Checked)
                if (AlsiUtils.DataBase.TestSqlConnection(laptop))
                {
                    laptopStringRadioButton.BackColor = Color.Green;
                    Properties.Settings.Default.ConnectionString = laptop;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Sql connection made.\nPlease Restart App");
                    Environment.Exit(0);
                }
                else
                    laptopStringRadioButton.BackColor = Color.Red;
        }
    }


}
