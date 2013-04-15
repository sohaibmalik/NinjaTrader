using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlsiUtils
{

	public class TakeProfit
	{
		private List<TakeProfit.TakeProfitTrade> _FullTradeList = new List<TakeProfit.TakeProfitTrade>();
		private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();

		public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
		{
			_CompletedTrades = CompletedTrades;
			foreach (var a in FullTradeList)
			{

				var b = new TakeProfitTrade((Trade)a.Clone());
				b.StopLoss_UpperLevel_1 = 150;
				b.StopLoss_UpperLevel_2 = 300;
				b.StopLoss_LoweLevel_1 = -150;
				_FullTradeList.Add(b);
			}

		}

		public void Calculate()
		{

			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
								 select x;

				var tpl = pl.ToList();
				SetCenterLine(tpl);
				SetCloseTriggers(tpl);
				SetOpenTriggers(tpl);
			}



			Print();

		}

		private void Print()
		{
			StreamWriter sr = new StreamWriter(@"d:\tt.txt");
			foreach (var d in _FullTradeList.Skip(1000).Take(5000).Where(z => z.StopLoss_CenterLine != 0))
			{
				var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.StopLoss_RunningTotalProfit + "," + d.StopLoss_CenterLine + "," + d.StopLoss_TakeProfitLevel_1
						+ "," + d.StopLoss_TakeProfitLevel_2 + "," + d.StopLoss_StopLossLevel_1 + "," + d.TakeProfit_Trigger1_Close + "," + d.TakeProfit_Trigger2_Close + "," + d.StopLoss_Triggered1_Close
						+ "," + d.TakeProfit_Trigger1_ReEnter + "," + d.TakeProfit_Trigger2_ReEnter + "," + d.StopLoss_Triggered1_ReEnter;
				//	Debug.WriteLine(data);
				//	AlsiUtils.Utilities.PrintAllProperties(d);
				sr.WriteLine(data);
			}
			sr.Close();
		}
		private void SetCenterLine(List<TakeProfitTrade> tpt)
		{
			var r = tpt.First().TotalPL;
			var count = tpt.Count();
			for (int q = 0; q < count; q++)
			{
				tpt[q].StopLoss_CenterLine = r;
				tpt[q].StopLoss_StopLossLevel_1 = r + tpt[q].StopLoss_LoweLevel_1;
				tpt[q].StopLoss_TakeProfitLevel_1 = r + tpt[q].StopLoss_UpperLevel_1;
				tpt[q].StopLoss_TakeProfitLevel_2 = r + tpt[q].StopLoss_UpperLevel_2;
				tpt[q].StopLoss_RunningTotalProfit = r + tpt[q].RunningProfit;
			}

		}
		private void SetCloseTriggers(List<TakeProfitTrade> tpt)
		{
			int i = tpt.Count;
			var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
			var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
			var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
			for (int x = 1; x < i; x++)
			{

				//UpperLevel1-Close Trade
				if (tpt[x].StopLoss_RunningTotalProfit < upper1 && tpt[x - 1].StopLoss_RunningTotalProfit > upper1) tpt[x].TakeProfit_Trigger1_Close = true;


				//UpperLevel2-Close Trade
				if (tpt[x].StopLoss_RunningTotalProfit < upper2 && tpt[x - 1].StopLoss_RunningTotalProfit > upper2) tpt[x].TakeProfit_Trigger2_Close = true;

				//LowerLevel1-Close Trade
				if (tpt[x].StopLoss_RunningTotalProfit < lower1 && tpt[x - 1].StopLoss_RunningTotalProfit > lower1) tpt[x].StopLoss_Triggered1_Close = true;

			}
		}
		private void SetOpenTriggers(List<TakeProfitTrade> tpt)
		{
			int i = tpt.Count;
			var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
			var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
			var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
			for (int x = 1; x < i; x++)
			{

				//UpperLevel1-ReEnter Trade
				if (tpt[x].StopLoss_RunningTotalProfit > upper1 && tpt[x - 1].StopLoss_RunningTotalProfit < upper1) tpt[x].TakeProfit_Trigger1_ReEnter = true;

				//UpperLevel2-ReEnter Trade
				if (tpt[x].StopLoss_RunningTotalProfit > upper2 && tpt[x - 1].StopLoss_RunningTotalProfit < upper2) tpt[x].TakeProfit_Trigger2_ReEnter = true;

				//LowerLevel1-AddPosition Trade
				if (tpt[x].StopLoss_RunningTotalProfit < lower1 && tpt[x - 1].StopLoss_RunningTotalProfit > lower1) tpt[x].StopLoss_Triggered1_ReEnter = true;



			}
		}

		public class TakeProfitTrade : Trade
		{

			public double StopLoss_UpperLevel_1;
			public double StopLoss_UpperLevel_2;
			public double StopLoss_LoweLevel_1;

			public double StopLoss_RunningTotalProfit { get; set; }
			public double StopLoss_TakeProfitLevel_1 { get; set; }
			public double StopLoss_TakeProfitLevel_2 { get; set; }
			public double StopLoss_StopLossLevel_1 { get; set; }
			public double StopLoss_CenterLine { get; set; }

			public bool TakeProfit_Trigger1_Close;
			public bool TakeProfit_Trigger2_Close;
			public bool StopLoss_Triggered1_Close;

			public bool TakeProfit_Trigger1_ReEnter;
			public bool TakeProfit_Trigger2_ReEnter;
			public bool StopLoss_Triggered1_ReEnter;


			public TakeProfitTrade(Trade trade)
			{
				this.BuyorSell = trade.BuyorSell;
				this.CurrentDirection = trade.CurrentDirection;
				this.CurrentPrice = trade.CurrentPrice;
				this.Extention = trade.Extention;
				this.IndicatorNotes = trade.IndicatorNotes;
				this.InstrumentName = trade.InstrumentName;
				this.Notes = trade.Notes;
				this.OHLC = trade.OHLC;
				this.Position = trade.Position;
				this.Reason = trade.Reason;
				this.RunningProfit = trade.RunningProfit;
				this.TimeStamp = trade.TimeStamp;
				this.TotalPL = trade.TotalPL;
				this.TradedPrice = trade.TradedPrice;
				this.TradeVolume = trade.TradeVolume;

			}



		}

	}
}
