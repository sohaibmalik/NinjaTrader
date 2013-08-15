using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AlsiTrade_Backend;
using System.Reflection;
using System.IO;
using System.Diagnostics;
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



        private string configfile = "";
        private void StartupForm_Load(object sender, EventArgs e)
        {

            if (!Communicator.Internet.CheckConnection())
            {
                MessageBox.Show("No Internet Connection", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);

            }


            var path = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var file = path + @"\ConnectionString.txt";
            configfile = file;
            var Sr = new StreamReader(file);
            var GeneralCS = Sr.ReadLine();
            Sr.Close();


            Properties.Settings.Default.ConnectionString = AlsiUtils.WebSettings.General.GetConnectionStringFromGeneral(GeneralCS);



            if (AlsiUtils.DataBase.TestSqlConnection(Properties.Settings.Default.ConnectionString))
            {
                this.Shown += new EventHandler(StartupForm_Shown);
                main.Shown += new EventHandler(main_Shown);
                main.onStartupLoad += new MainForm.LoadingStartup(main_onStartupLoad);
                AlsiUtils.DataBase.SetConnectionString(Properties.Settings.Default.ConnectionString);
            }
            else
            {
                this.Size = new Size(612, 181);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(configfile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }



    }


}
