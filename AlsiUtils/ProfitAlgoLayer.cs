using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlsiUtils
{
	public class ProfitAlgoLayer
	{
		private List<ProfitAlgoLayer.TakeProfitTrade> _FullTradeList = new List<ProfitAlgoLayer.TakeProfitTrade>();
		private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
		private double NewTotalRunningPL;

		public ProfitAlgoLayer(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
		{
			_CompletedTrades = CompletedTrades;
			AdjustVolume(FullTradeList);

			foreach (var a in FullTradeList)
			{
				var b = new TakeProfitTrade((Trade)a.Clone());
				_FullTradeList.Add(b);
			}

		}

		#region Prepare OHLC and BoilBands

		private List<Price> OHLC_LIST = new List<Price>();
		private List<BollingerBand> BOIL = new List<BollingerBand>();

		public void Calculate()
		{
			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
								 select x;

				var tpl = pl.ToList();
				if (tpl.Count > 0)
				{
					SetOpenCloseRawTriggers(tpl);
					SetOpenCloseTriggers(tpl);
					CalcProfLoss(tpl);
				}
			}

			SetOHLC();

			BOIL = AlsiUtils.Factory_Indicator.createBollingerBand(20, 2, OHLC_LIST, TicTacTec.TA.Library.Core.MAType.Ema);

			StreamWriter sw = new StreamWriter(@"D:\indicator.txt");
			foreach (var r in BOIL) sw.WriteLine(r.TimeStamp + "," + r.Price_Close + "," + r.Upper + "," + r.Lower);
			sw.Close();

		}
		private void SetOHLC()
		{
			var tpl = new List<ProfitAlgoLayer.TakeProfitTrade>();
			//	StreamWriter sr = new StreamWriter(@"d:\ohlcPL.txt");
			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
								 select x;

				tpl = pl.ToList();
				if (tpl.Count > 1)
				{
					var o = tpl[1].CurrentPrice - tpl[0].TradedPrice;
					var h = tpl.Max(x => x.CurrentPrice) - tpl[0].TradedPrice;
					var l = tpl.Min(x => x.CurrentPrice) - tpl[0].TradedPrice;
					var c = tpl.Last().CurrentPrice - tpl[0].TradedPrice;

					var P = new Price();
					P.TimeStamp = tpl.Last().TimeStamp;
					P.Open = o + tpl[0].RunningTotalProfit_New;
					P.High = h + tpl[0].RunningTotalProfit_New;
					P.Low = l + tpl[0].RunningTotalProfit_New;
					P.Close = c + tpl[0].RunningTotalProfit_New;
					OHLC_LIST.Add(P);
					//	Debug.WriteLine(i + "," + (o + tpl[0].RunningTotalProfit_New) + "," + (h + tpl[0].RunningTotalProfit_New) + "," + (l + tpl[0].RunningTotalProfit_New) + "," + (c + tpl[0].RunningTotalProfit_New));
					//	Debug.WriteLine(i + "," + P.Open + "," + "," + P.High + "," + P.Low + "," + P.Close);
				}
			}
			//sr.Close();
		}
		private void AdjustVolume(List<Trade> trade)
		{
			int count = trade.Count;
			for (int x = 1; x < count; x++)
			{   //set traded price
				if (trade[x].Reason == Trade.Trigger.OpenLong || trade[x].Reason == Trade.Trigger.OpenShort)
					trade[x].TradedPrice = trade[x].CurrentPrice;
				else
					trade[x].TradedPrice = trade[x - 1].TradedPrice;
			}


			//clear vol to 1
			double tp = 0;
			for (int x = 1; x < count; x++)
			{

				trade[x].TradeVolume = 1;
				trade[x].TotalPL = 0;
				trade[x].RunningProfit = 0;

				if (trade[x].Position && trade[x].CurrentDirection == Trade.Direction.Long)
					trade[x].RunningProfit = trade[x].CurrentPrice - trade[x].TradedPrice;

				if (trade[x].Position & trade[x].CurrentDirection == Trade.Direction.Short)
					trade[x].RunningProfit = trade[x].TradedPrice - trade[x].CurrentPrice;


				if (trade[x - 1].Position && !trade[x].Position)
				{
					if (trade[x - 1].CurrentDirection == Trade.Direction.Long)
						trade[x].RunningProfit = trade[x].CurrentPrice - trade[x - 1].TradedPrice;

					if (trade[x - 1].CurrentDirection == Trade.Direction.Short)
						trade[x].RunningProfit = trade[x - 1].TradedPrice - trade[x].CurrentPrice;
				}

				if (trade[x].Reason == Trade.Trigger.CloseLong || trade[x].Reason == Trade.Trigger.CloseShort)
				{
					tp += trade[x].RunningProfit;
					trade[x].TotalPL = tp;
					trade[x].TotalRunningProfit = tp;
				}
				else
					trade[x].TotalRunningProfit = tp + trade[x].RunningProfit;


			}





		}
		private void SetOpenCloseRawTriggers(List<TakeProfitTrade> tpt)
		{

			tpt[0].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
			int i = tpt.Count;

			for (int x = 1; x < i; x++)
			{
				tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
				if (tpt[x].Trigger_Close) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.CloseTrade;
				if (tpt[x].Trigger_Open) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.OpenTrade;

			}
		}
		private void SetOpenCloseTriggers(List<TakeProfitTrade> tpt)
		{

			tpt[0].TradeAction = TakeProfitTrade.TradeActions.None;
			tpt[0].InPosition = true;
			//tpt[0].AllowTrade = CurrentAllowTrade;
			int i = tpt.Count - 0;

			for (int x = 1; x < i; x++)
			{
				//set start
				tpt[x].InPosition = tpt[x - 1].InPosition;
				//close trade
				if (tpt[x].InPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.CloseTrade)
				{
					tpt[x].InPosition = false;
					//  CurrentAllowTrade = false;
					tpt[x].TradeAction = tpt[x].TradeAction_Raw;
				}

				//open trade
				if (!tpt[x].InPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.OpenTrade)
				{
					tpt[x].InPosition = true;
					// CurrentAllowTrade = true;
					tpt[x].TradeAction = tpt[x].TradeAction_Raw;
				}
			}
		}
		private void CalcProfLoss(List<TakeProfitTrade> tpt)
		{
			int i = tpt.Count;
			double pl = 0;

			tpt[0].RunningTotalProfit_New = NewTotalRunningPL;
			for (int x = 1; x < i; x++)
			{
				double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;

				if (tpt[x - 1].InPosition)
				{
					pl += tickpl;
					tpt[x].RunningProfit_New = pl;
				}
				tpt[x].RunningTotalProfit_New = NewTotalRunningPL + pl;
			}
			NewTotalRunningPL += pl;

		}
		public class TakeProfitTrade : Trade
		{
			public bool InPosition { get; set; }
			public TradeActions TradeAction_Raw { get; set; }
			public TradeActions TradeAction { get; set; }
			public double RunningProfit_New { get; set; }
			public double RunningTotalProfit_New { get; set; }
			public bool Trigger_Close { get; set; }
			public bool Trigger_Open { get; set; }
			public bool ReverseTrade { get; set; }

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
				this.TotalRunningProfit = trade.TotalRunningProfit;
				this.TradedPrice = trade.TradedPrice;
				this.TradeVolume = trade.TradeVolume;

			}
			public enum TradeActions
			{
				None,
				CloseTrade,
				OpenTrade,
			}

		}

		#endregion

		#region Boil Triggers

		private void CalcBoilTriggers()
		{
			foreach (var b in BOIL)
			{

			}
		}

		#endregion
	}
}
