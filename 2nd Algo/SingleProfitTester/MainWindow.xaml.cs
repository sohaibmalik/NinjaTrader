using AlgoSecondLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SingleProfitTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StringBuilder outText = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
           
          
        }

        private void GetData()
        {
            string localdata = @"Data Source=PITER-PC;Initial Catalog=AlsiTrade;Integrated Security=True";
            string remotedata = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";

            AddOutput("Getting Data", false );
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = remotedata;
            AlsiUtils.Data_Objects.GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
            AddOutput("...Done", true );

        }

        private void AddOutput(string output,bool newline)
        {
            if (newline) outText.AppendLine(output);
            else
                outText.Append(output);
            outputBox.Text = outText.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new StochPOP();

            var o = s.Start(input.Text, null, true);
            AddOutput("=================",true );
            AddOutput("Sequence " + o[0],true);
            AddOutput("Profit " + o[1], true);
            AddOutput("Trades " + o[2], true);
           
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            GetData();
            input.ToolTip = @" this.Fast_K = int.Parse(s[0]);
                    this.Slow_K = int.Parse(s[1]);
                    this.Slow_D = int.Parse(s[2]);
                    this.UPPER_75 = int.Parse(s[3]);
                    this.LOWER_25 = int.Parse(s[4]);
                    this.LIMIT_HIGH = int.Parse(s[5]);
                    this.LIMIT_LOW = int.Parse(s[6]);
                    this.STOPLOSS = int.Parse(s[7]);
                    this.TAKEPROFIT = int.Parse(s[8]);
                    this.CLOSE_END_OF_DAY = false;";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            outText = new StringBuilder();
            outputBox.Text = outText.ToString();
        }
    }
}
