using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using AlsiUtils;
using System.Windows.Media;

namespace AlsiTrade_Backend
{
	public class SyncDB
	{
		private BackgroundWorker bw = new BackgroundWorker();
		DateTime _StartOnlineData, _EndOnlineData;
		AlsiDBDataContext dc = new AlsiDBDataContext();


		public SyncDB()
		{
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
			bw.DoWork += new DoWorkEventHandler(bw_DoWork);
		}

	

		public void StartSync()
		{
			GetDailyOnlinePrices(out _StartOnlineData, out _EndOnlineData);
			bw.RunWorkerAsync();
		}

		void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			var del = dc.MasterMinutes.Where(z => z.Stamp >= _StartOnlineData);
			dc.MasterMinutes.DeleteAllOnSubmit(del);
			dc.SubmitChanges();
			AlsiTrade_Backend.UpdateDB.FullHistoricUpdate_MasterMinute(WebSettings.General.HISAT_INST);
		}

		void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			UpdatedEventArgs ee = new UpdatedEventArgs();
			OnUpdatedComplete(this, ee);
		}

		public IQueryable<DailyPriceData> GetDailyOnlinePrices(out DateTime StartDate, out DateTime EndDate)
		{
			AlsiUtils.WebSettings.GetSettings();
			
			var data = AlsiTrade_Backend.HiSat.HistData.GetHistoricalMINUTE_FromWEB(DateTime.Now.AddMonths(-3), DateTime.Now, 1, AlsiUtils.WebSettings.General.HISAT_INST);
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
							 select new DailyPriceData
							 {
								 Open = FGroup.First().TimeStamp,
								 Close = FGroup.OrderByDescending(z => z.TimeStamp).First().TimeStamp,
								 Count = FGroup.Count(),

							 };
		}

		public IQueryable<DailyPriceData> GetDailyDatabasePrices(DateTime Start, DateTime End)
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
							 select new DailyPriceData
							 {
								 Open = FGroup.First().Stamp,
								 Close = FGroup.OrderByDescending(z => z.Stamp).First().Stamp,
								 Count = FGroup.Count(),

							 };


		}



		public class DailyPriceData : INotifyPropertyChanged
		{
			public DateTime Open { get; set; }
			public DateTime Close { get; set; }
			public int Count { get; set; }
			public bool Fout { get; set; }
			public Brush BC { get { return Brushes.Red; } }
			public Brush AC { get { return Brushes.Yellow; } }


			public event PropertyChangedEventHandler PropertyChanged;

			event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
			{
				add { }
				remove { }
			}
		}


		public event Updated OnUpdatedComplete;
		public delegate void Updated(object sender, UpdatedEventArgs e);
		public class UpdatedEventArgs : EventArgs
		{

		}
	}
}
