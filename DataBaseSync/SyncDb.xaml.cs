using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AlsiTrade_Backend;
using AlsiUtils;
using AlsiTrade_Backend;
namespace DataBaseSync
{
	/// <summary>
	/// Interaction logic for SyncDb.xaml
	/// </summary>
	public partial class SyncDb : Window
	{


		private AlsiDBDataContext dc;
		private DateTime _StartOnlineData;
		private DateTime _EndOnlineData;
		private AlsiTrade_Backend.SyncDB sdb;

		private ObservableCollection<SyncDB.DailyPriceData> _DbPrices = new ObservableCollection<SyncDB.DailyPriceData>();
		public ObservableCollection<SyncDB.DailyPriceData> DbPrices
		{
			get { return _DbPrices; }
			set { _DbPrices = value; }
		}

		private ObservableCollection<SyncDB.DailyPriceData> _OnlinePrices = new ObservableCollection<SyncDB.DailyPriceData>();
		public ObservableCollection<SyncDB.DailyPriceData> OnlinePrices
		{
			get { return _OnlinePrices; }
			set { _OnlinePrices = value; }
		}



		public SyncDb()
		{
			InitializeComponent();


            string con = AlsiUtils.WebSettings.General.GetConnectionStringFromGeneral(@"Data Source=85.214.244.19;Initial Catalog=General;Persist Security Info=True;User ID=Tradebot;Password=boeboe");
			AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = con;
			dc = new AlsiDBDataContext();
			sdb = new SyncDB();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			WebSettings.GetSettings();
			OnlinePrices.Clear();
			foreach (var z in sdb.GetDailyOnlinePrices(out _StartOnlineData, out _EndOnlineData)) OnlinePrices.Add(z);
			// datagridOnline.ItemsSource = OnlinePrices;
			// Item source is now set in Xaml
			DbPrices.Clear();
			foreach (var z in sdb.GetDailyDatabasePrices(_StartOnlineData, _EndOnlineData)) DbPrices.Add(z);
			//datagridDatabase.ItemsSource = DbPrices;
			ComparePrices();

			datagridDatabase.SelectedIndex = 0;
			datagridOnline.SelectedIndex = 0;

			datagridDatabase.Columns[3].Visibility = Visibility.Hidden;
			datagridDatabase.Columns[4].Visibility = Visibility.Hidden;
			datagridDatabase.Columns[5].Visibility = Visibility.Hidden;
			datagridOnline.Columns[3].Visibility = Visibility.Hidden;
			datagridOnline.Columns[4].Visibility = Visibility.Hidden;
			datagridOnline.Columns[5].Visibility = Visibility.Hidden;

		}

		private void dbSync_Click(object sender, RoutedEventArgs e)
		{
			var w = new Waiting();
			w.ShowDialog();
			Window_Loaded(null, null);
		}

		private void CheckMasterMinute(IQueryable<MasterMinute> DayData)
		{

			var Data = dc.MasterMinutes.ToList();
			for (int x = 1; x < dc.MasterMinutes.Count(); x++)
			{
				if ((Data[x].Stamp - Data[x - 1].Stamp).Minutes > 1)
				{
					Debug.WriteLine(Data[x - 1].Instrument + "  " + Data[x - 1].Stamp + " " + Data[x - 1].C);
					Debug.WriteLine(Data[x].Instrument + "  " + Data[x].Stamp + " " + Data[x].C);
					Debug.WriteLine(Data[x + 1].Instrument + "  " + Data[x + 1].Stamp + " " + Data[x + 1].C);
				}
			}
		}


		private IQueryable<AlsiTrade_Backend.SyncDB.DailyPriceData> GetDailyDatabasePrices(DateTime Start, DateTime End)
		{

			return from q in dc.MasterMinutes.Where(z => z.Stamp >= Start && z.Stamp <= End).AsQueryable()
						 group q by new
						 {
							 Y = q.Stamp.Year,
							 M = q.Stamp.Month,
							 D = q.Stamp.Day,
						 }
							 into FGroup
							 orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.D
							 select new AlsiTrade_Backend.SyncDB.DailyPriceData
							 {
								 Open = FGroup.First().Stamp,
								 Close = FGroup.OrderByDescending(z => z.Stamp).First().Stamp,
								 Count = FGroup.Count(),

							 };


		}
        private int loadDaysBack = -15;
		private IQueryable<AlsiTrade_Backend.SyncDB.DailyPriceData> GetDailyOnlinePrices(out DateTime StartDate, out DateTime EndDate)
		{
			var data = AlsiTrade_Backend.HiSat.HistData.GetHistoricalMINUTE_FromWEB(DateTime.Now.AddDays(loadDaysBack), DateTime.Now, 1, AlsiUtils.WebSettings.General.HISAT_INST);
			DateTime start;
			start = data.First().TimeStamp;
			EndDate = data.Last().TimeStamp;
			StartDate = start;
			return from q in data.Where(z => z.TimeStamp >= start && z.TimeStamp <= DateTime.Now).AsQueryable()
						 group q by new
						 {
							 Y = q.TimeStamp.Year,
							 M = q.TimeStamp.Month,
							 D = q.TimeStamp.Day,
						 }
							 into FGroup
							 orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.D
							 select new AlsiTrade_Backend.SyncDB.DailyPriceData
							 {
								 Open = FGroup.First().TimeStamp,
								 Close = FGroup.OrderByDescending(z => z.TimeStamp).First().TimeStamp,
								 Count = FGroup.Count(),

							 };
		}

		

		private void ComparePrices()
		{
            try
            {


                foreach (var o in OnlinePrices) o.Fout = true;
                for (int x = 0; x < DbPrices.Count; x++)
                {
                    DbPrices[x].Fout = (!(OnlinePrices[x].Open == DbPrices[x].Open && OnlinePrices[x].Close == DbPrices[x].Close && OnlinePrices[x].Count == DbPrices[x].Count));
                    OnlinePrices[x].Fout = (!(OnlinePrices[x].Open.Date == DbPrices[x].Open.Date));
                }
            }
            catch (Exception ex)
            { }

			InfoLabelDb.Content = "Database records : " + DbPrices.Count();
			InfoLabelOnline.Content = "Online records : " + OnlinePrices.Count();
			InfoLabelUnMatched.Content = "Unmatched records " + DbPrices.Count(z => z.Fout == true);
		}
		private void datagridDatabase_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{

            try
            {
                if (datagridOnline.SelectedItem == null || datagridDatabase.SelectedItem == null) return;
                var selected = (AlsiTrade_Backend.SyncDB.DailyPriceData)datagridDatabase.SelectedItem;
                var index = OnlinePrices.IndexOf(OnlinePrices.Where(z => z.Open == selected.Open).First());
                datagridOnline.SelectedIndex = index;
                SynchSelectedRows(datagridDatabase, index);
            }
            catch
            {
            }

		
		}

		private void datagridOnline_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			try
			{
				if (datagridOnline.SelectedItem == null || datagridDatabase.SelectedItem == null) return;
				var selected = (SyncDB.DailyPriceData)datagridOnline.SelectedItem;
				var index = DbPrices.IndexOf(DbPrices.Where(z => z.Open == selected.Open).First());
				datagridDatabase.SelectedIndex = index;
				SynchSelectedRows(datagridOnline, index);
			}
			catch
			{

			}
		}
		private void SynchSelectedRows(DataGrid grid, int index)
		{
			if (grid == null || index == 0) return;
			DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
			row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
		}


	}
}

