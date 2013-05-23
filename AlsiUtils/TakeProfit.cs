using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace AlsiUtils
{


	public class TakeProfit
	{
		private List<TakeProfit.TakeProfitTrade> _FullTradeList = new List<TakeProfit.TakeProfitTrade>();
		private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
		private double NewTotalRunningPL;
		private List<MAMA> mama;
		private List<VariableIndicator> VarList = new List<VariableIndicator>();
		private bool CurrentAllowTrade;
		private double _m, _f, _cutoff;
		private int _reg1, _reg2, _reg3;
		public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades, double mama, double fama)
		{
			_CompletedTrades = CompletedTrades;
			_m = mama;
			_f = fama;



			AdjustVolume(FullTradeList);

			foreach (var a in FullTradeList)
			{
				var b = new TakeProfitTrade((Trade)a.Clone());
				_FullTradeList.Add(b);
			}

		}

		public void Calculate()
		{
			CalculateA();
			CalculateB();

			var pl = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).Last();
			var plnew = pl.RunningTotalProfit_New;
			var plold = pl.TotalPL;
			var diff = (-plold + plnew);

			//	if(diff>0)
			//Debug.WriteLine("mama " + _m + "  fama " + _f + "    "+ "     " + diff);
			SetOHLC();
			Print();
		}

		private void CalculateA()
		{

			NewTotalRunningPL = 0;
			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
								 select x;

				var tpl = pl.ToList();

				SetCenterLine(tpl);
			}
			SetMAMA();
		}

		private void CalculateB()
		{

			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp && x.Mama != null
								 select x;

				var tpl = pl.ToList();
				if (tpl.Count > 0)
				{
					SetCloseTriggers(tpl);
					SetOpenTriggers(tpl);
					SetOpenCloseRawTriggers(tpl);
					SetOpenCloseTriggers(tpl);
					CalcProfLoss(tpl);
				}

			}
		}

		private void SetOHLC()
		{
			var tpl = new List<TakeProfit.TakeProfitTrade>();
			StreamWriter sr = new StreamWriter(@"d:\ohlcPL.txt");
			int C = _CompletedTrades.Count;
			for (int i = 1; i < C; i++)
			{
				var pl = from x in _FullTradeList
								 where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp 
								 select x;

                tpl = pl.ToList();
				if (tpl.Count > 1)
				{
                    var o = tpl[1].CurrentPrice -tpl[0].TradedPrice;
					var h = tpl.Max(x => x.CurrentPrice) - tpl[0].TradedPrice;
					var l = tpl.Min(x => x.CurrentPrice) - tpl[0].TradedPrice;
					var c = tpl.Last().CurrentPrice - tpl[0].TradedPrice;


					 //Debug.WriteLine(i + "  O:" + (o + tpl[0].RunningTotalProfit_New) + "  H:" + (h + tpl[0].RunningTotalProfit_New) + "  L:" + (l + tpl[0].RunningTotalProfit_New) + "  C:" + (c + tpl[0].RunningTotalProfit_New));
					sr.WriteLine(i + "," + (o+tpl[0].RunningTotalProfit_New) + "," + (h + tpl[0].RunningTotalProfit_New) + "," + (l + tpl[0].RunningTotalProfit_New) + "," + (c + tpl[0].RunningTotalProfit_New));
				}
			}
			sr.Close();
		}

		private void Print()
		{


			var F = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0);
			StreamWriter sr = new StreamWriter(@"d:\tt.txt");
			foreach (var d in F.Skip(100))
			{

                var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.TotalRunningProfit
                                     + "," + d.Mama.Mama + "," + d.Mama.Fama + "," + d.Trigger_Open + "," + d.Trigger_Close
                                     + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.InPosition + "," + d.Reg1_Slope + "," + (d.Reg2_Slope) + "," + (d.Reg3_Slope)
                                     + "," + _cutoff + "," + d.RunningProfit_New + "," + (d.RunningTotalProfit_New);

                                    

				sr.WriteLine(data);
			}
			sr.Close();
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


				//Debug.WriteLine(trade[x].TimeStamp + "  " + trade[x].Reason + "   " + trade[x].RunningProfit + "  " + trade[x].CurrentDirection
				//   + "  " + trade[x].CurrentPrice + "  " + trade[x].TradedPrice + "   " + trade[x].TotalPL +"   " + trade[x].TotalRunningProfit );

			}





		}

		private void SetCenterLine(List<TakeProfitTrade> tpt)
		{
			var r = tpt.Last().TotalPL;
			var count = tpt.Count();
			for (int q = 0; q < count; q++)
			{
				tpt[q].StopLoss_CenterLine = r;


				//set Variable List
				var v = new VariableIndicator()
				{
					TimeStamp = tpt[q].TimeStamp,
					Value = tpt[q].TotalRunningProfit
				};
				VarList.Add(v);

			}

		}

		private void SetMAMA()
		{   //cannot use linq, too slow
			//0.01,0.01;
			mama = Factory_Indicator.createAdaptiveMA_MAMA(_f, _m, VarList);

			var FTL = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).ToList();
			var Ftl_Count = FTL.Count;
			var mam_Count = mama.Count;

			//find first mama.time=TRL.time
			var Ftl_Time_Equal_Mama_Time = FTL.Where(f => f.TimeStamp == mama[0].TimeStamp).First().TimeStamp;
			int ftl_Start_Index = 0;

			foreach (var Ft in FTL)
			{
				if (Ft.TimeStamp == Ftl_Time_Equal_Mama_Time) break;
				ftl_Start_Index++;
			}

			for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Mama = mama[x];


		}

		private void SetRegressionSlopes()
		{   //cannot use linq, too slow
			var FTL = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).ToList();
			var Ftl_Count = FTL.Count;


			#region Regression Calcs-Regression of MAMA

			var varList1 = new List<VariableIndicator>();
			foreach (var x in FTL.Where(z => z.Mama != null))
			{
				var v = new VariableIndicator()
				{
					TimeStamp = x.TimeStamp,
					Value = x.Mama.Mama,
				};
				varList1.Add(v);
			}

			var reg1 = Factory_Indicator.createRegression(_reg1, varList1);
			var varList2 = new List<VariableIndicator>();

			foreach (var rr in reg1)
			{
				var q = new VariableIndicator()
				{
					TimeStamp = rr.TimeStamp,
					Value = rr.Slope,
				};
				varList2.Add(q);
			}

			var reg2 = Factory_Indicator.createRegression(_reg2, varList2);

			var varList3 = new List<VariableIndicator>();

			foreach (var rr in reg2)
			{
				var qq = new VariableIndicator()
				{
					TimeStamp = rr.TimeStamp,
					Value = rr.Slope,
				};
				varList3.Add(qq);
			}

			var reg3 = Factory_Indicator.createRegression(_reg3, varList3);
			#endregion

			#region Populate TakeProfitTrade object

			int ftl_Start_Index;

			//REG_1_SLOPE
			ftl_Start_Index = 0;
			//find start for reg1
			var reg1_Count = reg1.Count;
			//find first reg1.Time=FTL.Time
			var Ftl_Time_Equal_Reg1_Time = FTL.Where(f => f.TimeStamp == reg1[0].TimeStamp).First().TimeStamp;
			foreach (var Ft in FTL)
			{
				if (Ft.TimeStamp == Ftl_Time_Equal_Reg1_Time) break;
				ftl_Start_Index++;
			}
			for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg1_Slope = reg1[x].Slope;

			//REG_2_SLOPE
			ftl_Start_Index = 0;
			//find start for reg2
			var reg2_Count = reg2.Count;
			//find first reg1.Time=FTL.Time
			var Ftl_Time_Equal_Reg2_Time = FTL.Where(f => f.TimeStamp == reg2[0].TimeStamp).First().TimeStamp;
			foreach (var Ft in FTL)
			{
				if (Ft.TimeStamp == Ftl_Time_Equal_Reg2_Time) break;
				ftl_Start_Index++;
			}
			for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg2_Slope = reg2[x].Slope * 10;


			//REG_3_SLOPE
			ftl_Start_Index = 0;
			//find start for reg3
			var reg3_Count = reg3.Count;
			//find first reg1.Time=FTL.Time
			var Ftl_Time_Equal_Reg3_Time = FTL.Where(f => f.TimeStamp == reg3[0].TimeStamp).First().TimeStamp;
			foreach (var Ft in FTL)
			{
				if (Ft.TimeStamp == Ftl_Time_Equal_Reg3_Time) break;
				ftl_Start_Index++;
			}
			for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg3_Slope = reg3[x].Slope * 100;

			#endregion

		}


		private void SetCloseTriggers(List<TakeProfitTrade> tpt)
		{

			int i = tpt.Count;
			for (int x = 1; x < i; x++)
			{
				if (tpt[x].Mama.Fama > tpt[x].Mama.Mama && tpt[x].TotalRunningProfit < tpt[x].Mama.Mama)
				{
					tpt[x].Trigger_Close = true;
					break;
				}
				
			}
		}
		private void SetOpenTriggers(List<TakeProfitTrade> tpt)
		{
			int i = tpt.Count;
			for (int x = 1; x < i; x++)
			{
				if (tpt[x].Mama.Fama < tpt[x].Mama.Mama)
				{
					tpt[x].Trigger_Open = true;
					break;
				}

				if (tpt[x].TotalRunningProfit < tpt[x].Mama.Fama - 150)
				{
					tpt[x].Trigger_Open = true;
					break;
				}
				
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
			public double StopLoss_CenterLine { get; set; }
			public TradeActions TradeAction_Raw { get; set; }
			public TradeActions TradeAction { get; set; }

			public double RunningProfit_New { get; set; }
			public double RunningTotalProfit_New { get; set; }


			public bool Trigger_Close { get; set; }
			public bool Trigger_Open { get; set; }


			public MAMA Mama { get; set; }
			public double Reg1_Slope { get; set; }
			public double Reg2_Slope { get; set; }
			public double Reg3_Slope { get; set; }

			public double TEST { get; set; }

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



	}
	/////REGRESSION MAMA
	//public class TakeProfit
	//{
	//  private List<TakeProfit.TakeProfitTrade> _FullTradeList = new List<TakeProfit.TakeProfitTrade>();
	//  private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
	//  private double NewTotalRunningPL;
	//  private List<MAMA> mama;
	//  private List<VariableIndicator> VarList = new List<VariableIndicator>();
	//  private bool CurrentAllowTrade;
	//  private double _m, _f, _cutoff;
	//  private int _reg1, _reg2, _reg3;
	//  public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades, double mama, double fama,double cutoff,int reg1,int reg2,int reg3)
	//  {
	//    _CompletedTrades = CompletedTrades;
	//    _m = mama;
	//    _f = fama ;
	//    _cutoff = cutoff;
	//    _reg1 = reg1;
	//    _reg2 = reg2;
	//    _reg3 = reg3;

	//    AdjustVolume(FullTradeList);

	//    foreach (var a in FullTradeList)
	//    {
	//      var b = new TakeProfitTrade((Trade)a.Clone());
	//      _FullTradeList.Add(b);
	//    }

	//  }

	//  public void Calculate()
	//  {
	//    CalculateA();
	//    CalculateB();

	//    var pl = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).Last();
	//    var plnew = pl.RunningTotalProfit_New;
	//    var plold = pl.TotalPL;
	//    var diff = (-plold + plnew);

	//  //	if(diff>0)
	//      Debug.WriteLine("mama " + _m + "  fama " + _f + "    "  + " cutoff " + _cutoff+ "   " 
	//        + " reg1 "+ " " +  _reg1
	//        + " reg2 " + " " + _reg2
	//        + " reg3 " + " " + _reg3 
	//        + "     " +diff);
	//    // Print();
	//  }

	//  private void CalculateA()
	//  {

	//    NewTotalRunningPL = 0;
	//    int C = _CompletedTrades.Count;
	//    for (int i = 1; i < C; i++)
	//    {
	//      var pl = from x in _FullTradeList
	//               where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
	//               select x;

	//      var tpl = pl.ToList();

	//      SetCenterLine(tpl);
	//    }
	//    SetMAMA();
	//    SetRegressionSlopes();
	//  }

	//  private void CalculateB()
	//  {

	//    int C = _CompletedTrades.Count;
	//    for (int i = 1; i < C; i++)
	//    {
	//      var pl = from x in _FullTradeList
	//               where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp && x.Mama != null
	//               select x;

	//      var tpl = pl.ToList();
	//      if (tpl.Count > 0)
	//      {
	//        SetCloseTriggers(tpl);
	//        SetOpenTriggers(tpl);
	//        SetOpenCloseRawTriggers(tpl);
	//        SetOpenCloseTriggers(tpl);
	//        CalcProfLoss(tpl);
	//      }

	//    }
	//  }



	//  private void Print()
	//  {


	//    var F = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0);
	//    StreamWriter sr = new StreamWriter(@"d:\tt.txt");
	//    foreach (var d in F.Skip(100).Take(9999999))
	//    {

	//      var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.TotalRunningProfit
	//                 + "," + d.Mama.Mama + "," + d.Mama.Fama + "," + d.Trigger_Open + "," + d.Trigger_Close
	//                 + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.InPosition + "," + d.Reg1_Slope + "," + (d.Reg2_Slope) + "," + (d.Reg3_Slope)
	//                 + "," + _cutoff + "," + d.RunningProfit_New + "," + d.RunningTotalProfit_New;

	//      sr.WriteLine(data);
	//    }
	//    sr.Close();
	//  }


	//  private void AdjustVolume(List<Trade> trade)
	//  {
	//    int count = trade.Count;
	//    for (int x = 1; x < count; x++)
	//    {   //set traded price
	//      if (trade[x].Reason == Trade.Trigger.OpenLong || trade[x].Reason == Trade.Trigger.OpenShort)
	//        trade[x].TradedPrice = trade[x].CurrentPrice;
	//      else
	//        trade[x].TradedPrice = trade[x - 1].TradedPrice;
	//    }


	//    //clear vol to 1
	//    double tp = 0;
	//    for (int x = 1; x < count; x++)
	//    {

	//      trade[x].TradeVolume = 1;
	//      trade[x].TotalPL = 0;
	//      trade[x].RunningProfit = 0;

	//      if (trade[x].Position && trade[x].CurrentDirection == Trade.Direction.Long)
	//        trade[x].RunningProfit = trade[x].CurrentPrice - trade[x].TradedPrice;

	//      if (trade[x].Position & trade[x].CurrentDirection == Trade.Direction.Short)
	//        trade[x].RunningProfit = trade[x].TradedPrice - trade[x].CurrentPrice;


	//      if (trade[x - 1].Position && !trade[x].Position)
	//      {
	//        if (trade[x - 1].CurrentDirection == Trade.Direction.Long)
	//          trade[x].RunningProfit = trade[x].CurrentPrice - trade[x - 1].TradedPrice;

	//        if (trade[x - 1].CurrentDirection == Trade.Direction.Short)
	//          trade[x].RunningProfit = trade[x - 1].TradedPrice - trade[x].CurrentPrice;
	//      }

	//      if (trade[x].Reason == Trade.Trigger.CloseLong || trade[x].Reason == Trade.Trigger.CloseShort)
	//      {
	//        tp += trade[x].RunningProfit;
	//        trade[x].TotalPL = tp;
	//        trade[x].TotalRunningProfit = tp;
	//      }
	//      else
	//        trade[x].TotalRunningProfit = tp + trade[x].RunningProfit;


	//      //Debug.WriteLine(trade[x].TimeStamp + "  " + trade[x].Reason + "   " + trade[x].RunningProfit + "  " + trade[x].CurrentDirection
	//      //   + "  " + trade[x].CurrentPrice + "  " + trade[x].TradedPrice + "   " + trade[x].TotalPL +"   " + trade[x].TotalRunningProfit );

	//    }





	//  }

	//  private void SetCenterLine(List<TakeProfitTrade> tpt)
	//  {
	//    var r = tpt.Last().TotalPL;
	//    var count = tpt.Count();
	//    for (int q = 0; q < count; q++)
	//    {
	//      tpt[q].StopLoss_CenterLine = r;


	//      //set Variable List
	//      var v = new VariableIndicator()
	//      {
	//        TimeStamp = tpt[q].TimeStamp,
	//        Value = tpt[q].TotalRunningProfit
	//      };
	//      VarList.Add(v);

	//    }

	//  }

	//  private void SetMAMA()
	//  {   //cannot use linq, too slow
	//    //0.01,0.01;
	//    mama = Factory_Indicator.createAdaptiveMA_MAMA(_f, _m, VarList);

	//    var FTL = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).ToList();
	//    var Ftl_Count = FTL.Count;
	//    var mam_Count = mama.Count;

	//    //find first mama.time=TRL.time
	//    var Ftl_Time_Equal_Mama_Time = FTL.Where(f => f.TimeStamp == mama[0].TimeStamp).First().TimeStamp;
	//    int ftl_Start_Index = 0;

	//    foreach (var Ft in FTL)
	//    {
	//      if (Ft.TimeStamp == Ftl_Time_Equal_Mama_Time) break;
	//      ftl_Start_Index++;
	//    }

	//    for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Mama = mama[x];


	//  }

	//  private void SetRegressionSlopes()
	//  {   //cannot use linq, too slow
	//    var FTL = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).ToList();
	//    var Ftl_Count = FTL.Count;


	//    #region Regression Calcs-Regression of MAMA

	//    var varList1 = new List<VariableIndicator>();
	//    foreach (var x in FTL.Where(z => z.Mama != null))
	//    {
	//      var v = new VariableIndicator()
	//      {
	//        TimeStamp = x.TimeStamp,
	//        Value = x.Mama.Mama,
	//      };
	//      varList1.Add(v);
	//    }

	//    var reg1 = Factory_Indicator.createRegression( _reg1,varList1);
	//    var varList2 = new List<VariableIndicator>();

	//    foreach (var rr in reg1)
	//    {
	//      var q = new VariableIndicator()
	//      {
	//        TimeStamp = rr.TimeStamp,
	//        Value = rr.Slope,
	//      };
	//      varList2.Add(q);
	//    }

	//    var reg2 = Factory_Indicator.createRegression(_reg2, varList2);

	//    var varList3 = new List<VariableIndicator>();

	//    foreach (var rr in reg2)
	//    {
	//      var qq = new VariableIndicator()
	//      {
	//        TimeStamp = rr.TimeStamp,
	//        Value = rr.Slope,
	//      };
	//      varList3.Add(qq);
	//    }

	//    var reg3 = Factory_Indicator.createRegression(_reg3, varList3);
	//    #endregion

	//    #region Populate TakeProfitTrade object

	//    int ftl_Start_Index;

	//    //REG_1_SLOPE
	//    ftl_Start_Index = 0;
	//    //find start for reg1
	//    var reg1_Count = reg1.Count;
	//    //find first reg1.Time=FTL.Time
	//    var Ftl_Time_Equal_Reg1_Time = FTL.Where(f => f.TimeStamp == reg1[0].TimeStamp).First().TimeStamp;
	//    foreach (var Ft in FTL)
	//    {
	//      if (Ft.TimeStamp == Ftl_Time_Equal_Reg1_Time) break;
	//      ftl_Start_Index++;
	//    }
	//    for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg1_Slope = reg1[x].Slope;

	//    //REG_2_SLOPE
	//    ftl_Start_Index = 0;
	//    //find start for reg2
	//    var reg2_Count = reg2.Count;
	//    //find first reg1.Time=FTL.Time
	//    var Ftl_Time_Equal_Reg2_Time = FTL.Where(f => f.TimeStamp == reg2[0].TimeStamp).First().TimeStamp;
	//    foreach (var Ft in FTL)
	//    {
	//      if (Ft.TimeStamp == Ftl_Time_Equal_Reg2_Time) break;
	//      ftl_Start_Index++;
	//    }
	//    for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg2_Slope = reg2[x].Slope * 10;


	//    //REG_3_SLOPE
	//    ftl_Start_Index = 0;
	//    //find start for reg3
	//    var reg3_Count = reg3.Count;
	//    //find first reg1.Time=FTL.Time
	//    var Ftl_Time_Equal_Reg3_Time = FTL.Where(f => f.TimeStamp == reg3[0].TimeStamp).First().TimeStamp;
	//    foreach (var Ft in FTL)
	//    {
	//      if (Ft.TimeStamp == Ftl_Time_Equal_Reg3_Time) break;
	//      ftl_Start_Index++;
	//    }
	//    for (int x = 0; x < Ftl_Count - ftl_Start_Index; x++) FTL[ftl_Start_Index + x].Reg3_Slope = reg3[x].Slope * 100;

	//    #endregion

	//  }


	//  private void SetCloseTriggers(List<TakeProfitTrade> tpt)
	//  {
	//    int i = tpt.Count;

	//    for (int x = 1; x < i; x++)
	//    {
	//      if (tpt[x].Reg2_Slope < tpt[x - 1].Reg2_Slope
	//          && tpt[x].Reason == Trade.Trigger.None
	//          && tpt[x].RunningProfit > 1
	//          && tpt[x - 1].Reg3_Slope > _cutoff)
	//      {
	//        tpt[x].Trigger_Close = true;
	//        break;
	//      }

	//    }
	//  }
	//  private void SetOpenTriggers(List<TakeProfitTrade> tpt)
	//  {

	//  }
	//  private void SetOpenCloseRawTriggers(List<TakeProfitTrade> tpt)
	//  {

	//    tpt[0].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
	//    int i = tpt.Count;

	//    for (int x = 1; x < i; x++)
	//    {
	//      tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
	//      if (tpt[x].Trigger_Close) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.CloseTrade;
	//      if (tpt[x].Trigger_Open) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.OpenTrade;

	//    }
	//  }
	//  private void SetOpenCloseTriggers(List<TakeProfitTrade> tpt)
	//  {

	//    tpt[0].TradeAction = TakeProfitTrade.TradeActions.None;
	//    tpt[0].InPosition = true;
	//    //tpt[0].AllowTrade = CurrentAllowTrade;
	//    int i = tpt.Count - 0;

	//    for (int x = 1; x < i; x++)
	//    {
	//      //set start
	//      tpt[x].InPosition = tpt[x - 1].InPosition;
	//      //close trade
	//      if (tpt[x].InPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.CloseTrade)
	//      {
	//        tpt[x].InPosition = false;
	//        //  CurrentAllowTrade = false;
	//        tpt[x].TradeAction = tpt[x].TradeAction_Raw;
	//      }

	//      //open trade
	//      if (!tpt[x].InPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.OpenTrade)
	//      {
	//        tpt[x].InPosition = true;
	//        // CurrentAllowTrade = true;
	//        tpt[x].TradeAction = tpt[x].TradeAction_Raw;
	//      }
	//    }
	//  }
	//  private void CalcProfLoss(List<TakeProfitTrade> tpt)
	//  {
	//    int i = tpt.Count;
	//    double pl = 0;

	//    tpt[0].RunningTotalProfit_New = NewTotalRunningPL;
	//    for (int x = 1; x < i; x++)
	//    {
	//      double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;

	//      if (tpt[x - 1].InPosition)
	//      {
	//        pl += tickpl;
	//        tpt[x].RunningProfit_New = pl;
	//      }
	//      tpt[x].RunningTotalProfit_New = NewTotalRunningPL + pl;
	//    }
	//    NewTotalRunningPL += pl;

	//  }



	//  public class TakeProfitTrade : Trade
	//  {
	//    public bool InPosition { get; set; }
	//    public double StopLoss_CenterLine { get; set; }
	//    public TradeActions TradeAction_Raw { get; set; }
	//    public TradeActions TradeAction { get; set; }

	//    public double RunningProfit_New { get; set; }
	//    public double RunningTotalProfit_New { get; set; }


	//    public bool Trigger_Close { get; set; }
	//    public bool Trigger_Open { get; set; }


	//    public MAMA Mama { get; set; }
	//    public double Reg1_Slope { get; set; }
	//    public double Reg2_Slope { get; set; }
	//    public double Reg3_Slope { get; set; }

	//    public double TEST { get; set; }

	//    public TakeProfitTrade(Trade trade)
	//    {
	//      this.BuyorSell = trade.BuyorSell;
	//      this.CurrentDirection = trade.CurrentDirection;
	//      this.CurrentPrice = trade.CurrentPrice;
	//      this.Extention = trade.Extention;
	//      this.IndicatorNotes = trade.IndicatorNotes;
	//      this.InstrumentName = trade.InstrumentName;
	//      this.Notes = trade.Notes;
	//      this.OHLC = trade.OHLC;
	//      this.Position = trade.Position;
	//      this.Reason = trade.Reason;
	//      this.RunningProfit = trade.RunningProfit;
	//      this.TimeStamp = trade.TimeStamp;
	//      this.TotalPL = trade.TotalPL;
	//      this.TotalRunningProfit = trade.TotalRunningProfit;
	//      this.TradedPrice = trade.TradedPrice;
	//      this.TradeVolume = trade.TradeVolume;

	//    }

	//    public enum TradeActions
	//    {
	//      None,
	//      CloseTrade,
	//      OpenTrade,

	//    }


	//  }



	//}

}
