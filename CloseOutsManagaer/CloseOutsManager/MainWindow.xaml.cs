using AlsiUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace CloseOutsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void getOnlineDataButton_Click_1(object sender, RoutedEventArgs e)
        {
            GetOnlineData();
            GetDatabaseData();
            onlinedataTextBox.Text = onlineString.ToString();
            mergeButton.IsEnabled = (onlineString.Length > 0 && dbData.Length > 0); 

        }

        private void mergeButton_Click_1(object sender, RoutedEventArgs e)
        {
            Merge();
        }

        private string GetConnectionString()
        {
            var path = new FileInfo(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var file = path + @"\ConnectionString.txt";
            var Sr = new StreamReader(file);
            var GeneralCS = Sr.ReadLine();
            Sr.Close();

            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString= AlsiUtils.WebSettings.General.GetConnectionStringFromGeneral(GeneralCS);
            return AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString;
        }

        private IQueryable<AlsiUtils.MasterMinute> DatabaseData;
        private AlsiDBDataContext dc;
        private string  instrument="";
        private StringBuilder onlineString;
        private StringBuilder dbData;

        private void GetDatabaseData()
        {
            dc = new AlsiUtils.AlsiDBDataContext(GetConnectionString());
            var date = newInstrumentDatePicker.SelectedDate.Value;
            DatabaseData = dc.MasterMinutes.Where(z => z.Stamp.Date >= date);
            dbData = new StringBuilder();
            foreach (var v in DatabaseData)
            {
                dbData.AppendLine(v.Stamp.ToString() + "," + v.O + "," + v.H + "," + v.L + "," + v.C + "," + v.V + "," + v.Instrument);
            }
            dbdataTextBox.Text = dbData.ToString();
        }

        private void GetOnlineData()
        {
            instrument = newInstrumentName.Text;
            var date = newInstrumentDatePicker.SelectedDate.Value;
            var data = AlsiTrade_Backend.HiSat.HistData.GetHistoricalMINUTE_FromWEB(date, DateTime.Now.Date, 1, instrument);

             onlineString = new StringBuilder();
            foreach (var v in data)
            {
                onlineString.AppendLine(v.TimeStamp.ToString() + "," + v.Open + "," + v.High + "," + v.Low + "," + v.Close + "," + v.Volume + "," + instrument);
            }
           
        }

        private void Merge()
        {
            //delete data
            dc.MasterMinutes.DeleteAllOnSubmit(DatabaseData);
            dc.SubmitChanges();

            //enter seed so that update works
            var seed = new MasterMinute()
            {
                Stamp=DateTime.Now.AddYears(1),
                Instrument=instrument,
                O=0,
                H=0,
                L=0,
                C=0,
                V=0,
                
            };
            dc.MasterMinutes.InsertOnSubmit(seed);
            dc.SubmitChanges();

            //Update -- seed is automatically deleted
            AlsiTrade_Backend.UpdateDB.FullHistoricUpdate_MasterMinute(instrument);
            
           
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            newInstrumentDatePicker.SelectedDate = DateTime.Now.Date;
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("Width : {0}   Height : {1}", Width, Height);
        }
    }
}
