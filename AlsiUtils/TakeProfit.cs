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
		public TakeProfit(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades, double mama, double fama,double cutoff)
		{
			_CompletedTrades = CompletedTrades;
			_m = mama;
			_f = fama ;
			_cutoff = cutoff;

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

			if(diff>0)Debug.WriteLine("mama " + _m + "  fama " + _f + "    "  + " cutoff " + _cutoff+ "   " + diff);
			 //Print();
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
			SetRegressionSlopes();
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



		private void Print()
		{


			var F = _FullTradeList.Where(z => z.StopLoss_CenterLine != 0);
			StreamWriter sr = new StreamWriter(@"d:\tt.txt");
			foreach (var d in F.Skip(100).Take(9999999))
			{

				var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.TotalRunningProfit
									 + "," + d.Mama.Mama + "," + d.Mama.Fama + "," + d.Trigger_Open + "," + d.Trigger_Close
									 + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.InPosition + "," + d.Reg1_Slope + "," + (d.Reg2_Slope) + "," + (d.Reg3_Slope)
									 + "," + _cutoff + "," + d.RunningProfit_New + "," + d.RunningTotalProfit_New;

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

			var reg1 = Factory_Indicator.createRegression(18, varList1);
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

			var reg2 = Factory_Indicator.createRegression(9, varList2);

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

			var reg3 = Factory_Indicator.createRegression(5, varList3);
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
				if (tpt[x].Reg2_Slope < tpt[x - 1].Reg2_Slope
						&& tpt[x].Reason == Trade.Trigger.None
						&& tpt[x].RunningProfit > 1
						&& tpt[x - 1].Reg3_Slope > _cutoff)
				{
					tpt[x].Trigger_Close = true;
					break;
				}

			}
		}
		private void SetOpenTriggers(List<TakeProfitTrade> tpt)
		{

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




	//public class TakeProfit_Range
	//{
	//    private List<TakeProfit_Range.TakeProfitTrade> _FullTradeList = new List<TakeProfit_Range.TakeProfitTrade>();
	//    private List<CompletedTrade> _CompletedTrades = new List<CompletedTrade>();
	//    private double NewTotalRunningPL;
	//    private List<MAMA> mama;
	//    private List<VariableIndicator> VarList = new List<VariableIndicator>();

	//    public TakeProfit_Range(List<Trade> FullTradeList, List<CompletedTrade> CompletedTrades)
	//    {
	//        _CompletedTrades = CompletedTrades;


	//        foreach (var a in FullTradeList)
	//        {

	//            var b = new TakeProfitTrade((Trade)a.Clone());
	//            b.StopLoss_UpperLevel_1 = 150;
	//            b.StopLoss_UpperLevel_2 = 300;
	//            b.StopLoss_LoweLevel_1 = -150;
	//            _FullTradeList.Add(b);
	//        }

	//    }

	//    public void Calculate()
	//    {


	//        NewTotalRunningPL = 0;
	//        int C = _CompletedTrades.Count;
	//        for (int i = 1; i < C; i++)
	//        {
	//            var pl = from x in _FullTradeList
	//                     where x.TimeStamp >= _CompletedTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= _CompletedTrades[i].CloseTrade.TimeStamp
	//                     select x;

	//            var tpl = pl.ToList();
	//            AdjustVolume(tpl);
	//            SetCenterLine(tpl);
	//            SetCloseTriggers(tpl);
	//            SetOpenTriggers(tpl);
	//            SetOpenCloseRawTriggers(tpl);
	//            SetOpenCloseTriggers(tpl);
	//            CalcProfLoss(tpl);
	//        }
	//        Print();
	//    }





	//    private void Print()
	//    {
	//        StreamWriter sr = new StreamWriter(@"d:\tt.txt");
	//        foreach (var d in _FullTradeList.Where(z => z.StopLoss_CenterLine != 0).Skip(1000))
	//        {
	//            var data = d.TimeStamp + "," + d.Reason + "," + d.RunningProfit + "," + d.StopLoss_RunningTotalProfit + "," + d.StopLoss_CenterLine + "," + d.StopLoss_TakeProfitLevel_1
	//                            + "," + d.StopLoss_TakeProfitLevel_2 + "," + d.StopLoss_StopLossLevel_1
	//                            + "," + d.Mama.Mama + "," + d.Mama.Fama;
	//            //+ "," + d.TakeProfit_Trigger1_Close + "," + d.TakeProfit_Trigger2_Close + "," + d.StopLoss_Triggered1_Close
	//            // + "," + d.TakeProfit_Trigger1_ReEnter + "," + d.TakeProfit_Trigger2_ReEnter + "," + d.StopLoss_Triggered1_ReEnter
	//            // + "," + d.TradeAction_Raw + "," + d.TradeAction + "," + d.IntraPosition + "," + d.NewRunningProf + "," + d.NewTotalRunningProf +","+d.TradeVolume;
	//            //	Debug.WriteLine(data);
	//            //	AlsiUtils.Utilities.PrintAllProperties(d);
	//            sr.WriteLine(data);
	//        }
	//        sr.Close();
	//    }
	//    private void SetCenterLine(List<TakeProfitTrade> tpt)
	//    {
	//        var r = tpt.First().TotalPL;
	//        var count = tpt.Count();
	//        for (int q = 0; q < count; q++)
	//        {
	//            tpt[q].StopLoss_CenterLine = r;
	//            tpt[q].StopLoss_StopLossLevel_1 = r + tpt[q].StopLoss_LoweLevel_1;
	//            tpt[q].StopLoss_TakeProfitLevel_1 = r + tpt[q].StopLoss_UpperLevel_1;
	//            tpt[q].StopLoss_TakeProfitLevel_2 = r + tpt[q].StopLoss_UpperLevel_2;
	//            tpt[q].StopLoss_RunningTotalProfit = r + tpt[q].RunningProfit;

	//            //set Variable List
	//            var v = new VariableIndicator()
	//            {
	//                TimeStamp = tpt[q].TimeStamp,
	//                Value = tpt[q].StopLoss_RunningTotalProfit,
	//            };
	//            VarList.Add(v);

	//        }

	//    }




	//    private void AdjustVolume(List<TakeProfitTrade> tpt)
	//    {
	//        var r = tpt.First().TotalPL;
	//        var count = tpt.Count();
	//        var vol = 1;// tpt[0].TradeVolume;
	//        for (int q = 0; q < count; q++)
	//        {
	//            tpt[q].TradeVolume = vol;
	//            tpt[q].RunningProfit = tpt[q].RunningProfit / vol;
	//        }
	//    }
	//    private void SetCloseTriggers(List<TakeProfitTrade> tpt)
	//    {
	//        int i = tpt.Count;
	//        var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
	//        var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
	//        var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
	//        for (int x = 1; x < i; x++)
	//        {

	//            //UpperLevel1-Close Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit < upper1 && tpt[x - 1].StopLoss_RunningTotalProfit > upper1) tpt[x].TakeProfit_Trigger1_Close = true;

	//            //UpperLevel2-Close Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit < upper2 && tpt[x - 1].StopLoss_RunningTotalProfit > upper2) tpt[x].TakeProfit_Trigger2_Close = true;

	//            //LowerLevel1-Close Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit < lower1 && tpt[x - 1].StopLoss_RunningTotalProfit > lower1) tpt[x].StopLoss_Triggered1_Close = true;

	//        }
	//    }
	//    private void SetOpenTriggers(List<TakeProfitTrade> tpt)
	//    {
	//        int i = tpt.Count;
	//        var upper1 = tpt[0].StopLoss_UpperLevel_1 + tpt[0].StopLoss_CenterLine;
	//        var upper2 = tpt[0].StopLoss_UpperLevel_2 + tpt[0].StopLoss_CenterLine;
	//        var lower1 = tpt[0].StopLoss_LoweLevel_1 + tpt[0].StopLoss_CenterLine;
	//        for (int x = 1; x < i; x++)
	//        {

	//            //UpperLevel1-ReEnter Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit > upper1 && tpt[x - 1].StopLoss_RunningTotalProfit < upper1) tpt[x].TakeProfit_Trigger1_ReEnter = true;

	//            //UpperLevel2-ReEnter Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit > upper2 && tpt[x - 1].StopLoss_RunningTotalProfit < upper2) tpt[x].TakeProfit_Trigger2_ReEnter = true;

	//            //LowerLevel1-AddPosition Trade
	//            if (tpt[x].StopLoss_RunningTotalProfit > lower1 && tpt[x - 1].StopLoss_RunningTotalProfit < lower1) tpt[x].StopLoss_Triggered1_ReEnter = true;


	//        }
	//    }
	//    private void SetOpenCloseRawTriggers(List<TakeProfitTrade> tpt)
	//    {

	//        tpt[0].TradeAction_Raw = TakeProfitTrade.TradeActions.None;

	//        int i = tpt.Count;

	//        for (int x = 1; x < i; x++)
	//        {
	//            tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.None;
	//            if (tpt[x].TakeProfit_Trigger1_Close || tpt[x].TakeProfit_Trigger2_Close) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.CloseTrade;
	//            if (tpt[x].TakeProfit_Trigger1_ReEnter) tpt[x].TradeAction_Raw = TakeProfitTrade.TradeActions.OpenTrade;

	//        }
	//    }
	//    private void SetOpenCloseTriggers(List<TakeProfitTrade> tpt)
	//    {

	//        tpt[0].TradeAction = TakeProfitTrade.TradeActions.None;
	//        tpt[0].IntraPosition = true;
	//        int i = tpt.Count - 1;

	//        for (int x = 1; x < i; x++)
	//        {
	//            //set start
	//            tpt[x].IntraPosition = tpt[x - 1].IntraPosition;
	//            //close trade
	//            if (tpt[x].IntraPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.CloseTrade)
	//            {
	//                tpt[x].IntraPosition = false;
	//                tpt[x].TradeAction = tpt[x].TradeAction_Raw;
	//            }

	//            //open trade
	//            if (!tpt[x].IntraPosition && tpt[x].TradeAction_Raw == TakeProfitTrade.TradeActions.OpenTrade)
	//            {
	//                tpt[x].IntraPosition = true;
	//                tpt[x].TradeAction = tpt[x].TradeAction_Raw;
	//            }
	//        }
	//    }
	//    private void CalcProfLoss(List<TakeProfitTrade> tpt)
	//    {
	//        int i = tpt.Count;
	//        double pl = 0;
	//        var vol = tpt[0].TradeVolume;
	//        tpt[0].NewTotalRunningProf = NewTotalRunningPL;
	//        for (int x = 1; x < i; x++)
	//        {
	//            double tickpl = tpt[x].RunningProfit - tpt[x - 1].RunningProfit;

	//            if (tpt[x - 1].IntraPosition) pl += (tickpl * vol);
	//            tpt[x].NewRunningProf += (pl * vol);
	//            if (tpt[x - 1].IntraPosition) tpt[x].NewTotalRunningProf = NewTotalRunningPL += (tickpl * vol);
	//            else
	//                tpt[x].NewTotalRunningProf = tpt[x - 1].NewTotalRunningProf;
	//        }


	//    }



	//    public class TakeProfitTrade : Trade
	//    {
	//        public bool IntraPosition { get; set; }
	//        public TradeActions TradeAction_Raw { get; set; }
	//        public TradeActions TradeAction { get; set; }

	//        public double NewRunningProf { get; set; }
	//        public double NewTotalRunningProf { get; set; }

	//        public double StopLoss_UpperLevel_1;
	//        public double StopLoss_UpperLevel_2;
	//        public double StopLoss_LoweLevel_1;

	//        public double StopLoss_RunningTotalProfit { get; set; }
	//        public double StopLoss_TakeProfitLevel_1 { get; set; }
	//        public double StopLoss_TakeProfitLevel_2 { get; set; }
	//        public double StopLoss_StopLossLevel_1 { get; set; }
	//        public double StopLoss_CenterLine { get; set; }

	//        public bool TakeProfit_Trigger1_Close;
	//        public bool TakeProfit_Trigger2_Close;
	//        public bool StopLoss_Triggered1_Close;

	//        public bool TakeProfit_Trigger1_ReEnter;
	//        public bool TakeProfit_Trigger2_ReEnter;
	//        public bool StopLoss_Triggered1_ReEnter;

	//        public double TEST { get; set; }

	//        public MAMA Mama { get; set; }


	//        public TakeProfitTrade(Trade trade)
	//        {
	//            this.BuyorSell = trade.BuyorSell;
	//            this.CurrentDirection = trade.CurrentDirection;
	//            this.CurrentPrice = trade.CurrentPrice;
	//            this.Extention = trade.Extention;
	//            this.IndicatorNotes = trade.IndicatorNotes;
	//            this.InstrumentName = trade.InstrumentName;
	//            this.Notes = trade.Notes;
	//            this.OHLC = trade.OHLC;
	//            this.Position = trade.Position;
	//            this.Reason = trade.Reason;
	//            this.RunningProfit = trade.RunningProfit;
	//            this.TimeStamp = trade.TimeStamp;
	//            this.TotalPL = trade.TotalPL;
	//            this.TradedPrice = trade.TradedPrice;
	//            this.TradeVolume = trade.TradeVolume;

	//        }

	//        public enum TradeActions
	//        {
	//            None,
	//            CloseTrade,
	//            OpenTrade,
	//            AddPosition,
	//        }

	//    }



	//}
}
