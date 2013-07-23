using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AlsiUtils;
using System.ComponentModel;


namespace DataBaseSync
{
	/// <summary>
	/// Interaction logic for Waiting.xaml
	/// </summary>
	public partial class Waiting : Window
	{

		private AlsiTrade_Backend.SyncDB sdb;

		public Waiting()
		{
			InitializeComponent();
			sdb = new AlsiTrade_Backend.SyncDB();
			sdb.OnUpdatedComplete += new AlsiTrade_Backend.SyncDB.Updated(sdb_OnUpdatedComplete);		
		}

		void sdb_OnUpdatedComplete(object sender, AlsiTrade_Backend.SyncDB.UpdatedEventArgs e)
		{
			Close();
		}
				
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			sdb.StartSync();
		}

	}
}
