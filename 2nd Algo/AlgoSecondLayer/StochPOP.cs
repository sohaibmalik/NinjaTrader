using AlsiUtils;
using AlsiUtils.Data_Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class StochPOP
    {
        private List<SS_Price> SSPOP;
        private List<SS_Price> SSPOP_Raw_Trades_Only;
        private List<Trade> TradeList;
        private Seq _Seq;
        double Profit = 0;
        int Trades = 0;
        string _sequence = "";
        public DateTime StartDate;
        public DateTime EndDate;

        public List<string> Start(string Sequence, string simcontext, bool single)
        {
            // Console.WriteLine("Getting Sequence");

            if (!single)
            {
                _sequence = Utils.GetRandomSequence();

                if (_sequence == "None")
                {
                    Console.WriteLine("No elements found");
                    return null;
                }
            }
            else
            {
                _sequence = Sequence;
            }
            var par = _sequence.Split(',');

            // _Seq = new Seq();
            _Seq = new Seq(_sequence);
            // Debug.WriteLine("Sequence selected " + _sequence);


            List<Price> Prices= GlobalObjects.Points;
           

            SSPOP = new List<SS_Price>();
            SSPOP_Raw_Trades_Only = new List<SS_Price>();

            Dictionary<DateTime, SlowStoch> SS_DIC = new Dictionary<DateTime, SlowStoch>();
            Dictionary<DateTime, double> PROF_DIC = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> VOL_DIC = new Dictionary<DateTime, double>();

            //START LOOOP




            var SS = AlsiUtils.Factory_Indicator.createSlowStochastic(_Seq.Fast_K, _Seq.Slow_K, _Seq.Slow_D, Prices);


            //CREATE DICTIONARY          
            SS_DIC = SS.ToDictionary(x => x.TimeStamp, x => x);

            //POPULATE
            foreach (var p in Prices)
            {
                SS_Price rsp = new SS_Price()
                {
                    ClosePrice = p.Close,
                    Volume = p.Volume,
                    Stamp = p.TimeStamp,
                    SS = null,
                    LastCross = SS_Price.CrossType.None,
                    TrailingLevel = -1,
                };
                SlowStoch s;

                SS_DIC.TryGetValue(p.TimeStamp, out s);

                if (s != null)
                {
                    rsp.SS = s;
                    SSPOP.Add(rsp);
                }
            }

            //RUN CALCS
            SetTriggers_Crossed();
            SetTriggers_Limits();
            SetTriggers_TradeSignals();
            SetTriggers_StopLoss_TakeProfit();
            GetTrades();
            CalcBasicStats();

         //   WriteResults();
            if (Profit > 15000 || Profit < -15000 && !single) ;
            //   WriteResultsToDatabase(_sequence);
            else
                if (single)
                {
                    //  WriteResults();
                }
            var output = new List<string>()
            {
                _sequence,
                Profit.ToString(),
                Trades.ToString(),
            };
            return output;

            //END LOOP
            //}
        }


        //Step 1
        private void SetTriggers_Crossed()
        {
            var Count = SSPOP.Count;

            for (int x = 1; x < Count; x++)
            {
                //Crossed UP
                if (SSPOP[x - 1].SS.K < SSPOP[x - 1].SS.D && SSPOP[x].SS.K > SSPOP[x].SS.D)
                {
                    SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_UP_Between;
                    if (SSPOP[x - 1].SS.D <= _Seq.LOWER_25) SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_UP_Below_25;
                }
                else
                    //Crossed DOWN
                    if (SSPOP[x - 1].SS.K > SSPOP[x - 1].SS.D && SSPOP[x].SS.K < SSPOP[x].SS.D)
                    {
                        SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_DOWN_Between;
                        if (SSPOP[x - 1].SS.D >= _Seq.UPPER_75) SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_DOWN_Above_75;
                    }
                    else
                    {
                        SSPOP[x].LastCross = SSPOP[x - 1].LastCross;
                    }


            }
        }
        //Step 2
        private void SetTriggers_Limits()
        {
            var Count = SSPOP.Count;

            for (int x = 1; x < Count; x++)
            {
                //Touched HIGH
                if (SSPOP[x - 1].SS.K < _Seq.LIMIT_HIGH && SSPOP[x].SS.K > _Seq.LIMIT_HIGH)
                {
                    SSPOP[x].Limit = StochPOP.SS_Price.TouchedLimit.Touched_Upper;
                    SSPOP[x].TrailingLevel = SSPOP[x].SS.K;
                }
                else
                    //Touched LOW
                    if (SSPOP[x - 1].SS.K > _Seq.LIMIT_LOW && SSPOP[x].SS.K < _Seq.LIMIT_LOW)
                    {
                        SSPOP[x].Limit = StochPOP.SS_Price.TouchedLimit.Touched_Lower;
                        SSPOP[x].TrailingLevel = SSPOP[x].SS.K;
                    }
                    else
                    {
                        SSPOP[x].Limit = SSPOP[x - 1].Limit;

                        if (SSPOP[x].Limit == SS_Price.TouchedLimit.Touched_Upper)
                            if (SSPOP[x].SS.K > SSPOP[x - 1].TrailingLevel && SSPOP[x - 1].TrailingLevel != -1)
                                SSPOP[x].TrailingLevel = SSPOP[x].SS.K;
                            else
                                SSPOP[x].Limit = SS_Price.TouchedLimit.None;


                        if (SSPOP[x].Limit == SS_Price.TouchedLimit.Touched_Lower)
                            if (SSPOP[x].SS.K < SSPOP[x - 1].TrailingLevel && SSPOP[x - 1].TrailingLevel != -1)
                                SSPOP[x].TrailingLevel = SSPOP[x].SS.K;
                            else
                            {
                                SSPOP[x].TrailingLevel = -1;
                                SSPOP[x].Limit = SS_Price.TouchedLimit.None;
                            }
                    }

            }
        }
        //Step 3
        private void SetTriggers_TradeSignals()
        {
            var Count = SSPOP.Count;
            for (int x = 1; x < Count; x++)
            {
                //Carry over values
                SSPOP[x].Position = SSPOP[x - 1].Position;
                SSPOP[x].Direction = SSPOP[x - 1].Direction;
                SSPOP[x].TradedPrice = SSPOP[x - 1].TradedPrice;
                if (SSPOP[x].Direction != SS_Price.TradeDirection.None)
                {
                    SSPOP[x].StopLoss = SSPOP[x - 1].StopLoss;
                    SSPOP[x].TakeProfit = SSPOP[x - 1].TakeProfit;
                }




                if (!SSPOP[x].Position) //Open Triggers
                {
                    //Open Long - 50
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Between)
                        if (SSPOP[x - 1].SS.K < _Seq.OPEN_Long_50 && SSPOP[x].SS.K > _Seq.OPEN_Long_50)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            SSPOP[x].TakeProfit = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                            SSPOP[x].StopLoss = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                            goto Done;
                        }

                    //Open Long - 75
                    if (SSPOP[x - 1].SS.K < _Seq.UPPER_75 && SSPOP[x].SS.K > _Seq.UPPER_75)
                    {
                        SSPOP[x].Position = true;
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                        SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        SSPOP[x].TakeProfit = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                        SSPOP[x].StopLoss = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                        goto Done;
                    }

                    //Open Long > 25
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Below_25)
                        if (SSPOP[x - 1].SS.K < _Seq.LOWER_25 && SSPOP[x].SS.K > _Seq.LOWER_25)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            SSPOP[x].TakeProfit = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                            SSPOP[x].StopLoss = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                            goto Done;
                        }

                    //Open Short - 50
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Between)
                        if (SSPOP[x - 1].SS.K > _Seq.OPEN_Short_50 && SSPOP[x].SS.K < _Seq.OPEN_Short_50)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            SSPOP[x].TakeProfit = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                            SSPOP[x].StopLoss = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                            goto Done;
                        }

                    //Open Short - 25
                    if (SSPOP[x - 1].SS.K < _Seq.UPPER_75 && SSPOP[x].SS.K > _Seq.UPPER_75)
                    {
                        SSPOP[x].Position = true;
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                        SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        SSPOP[x].TakeProfit = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                        SSPOP[x].StopLoss = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                        goto Done;
                    }

                    //Open Short < 75
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Above_75)
                        if (SSPOP[x - 1].SS.K > _Seq.UPPER_75 && SSPOP[x].SS.K < _Seq.UPPER_75)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            SSPOP[x].TakeProfit = SSPOP[x].ClosePrice - _Seq.STOPLOSS;
                            SSPOP[x].StopLoss = SSPOP[x].ClosePrice + _Seq.TAKEPROFIT;
                            goto Done;
                        }

                }
                //Close Triggers
                else
                {
                    //Close Long  - Any Cross
                    if (SSPOP[x].Direction == SS_Price.TradeDirection.Long)
                    {
                        if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Above_75 || SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Between)
                        {
                            SSPOP[x].Position = false;
                            SSPOP[x].Direction = SS_Price.TradeDirection.None;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.CloseLong;
                            SSPOP[x].RunningProfit = SSPOP[x].ClosePrice - SSPOP[x].TradedPrice;
                            _Seq.TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = _Seq.TOTALPROFIT;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            goto Done;
                        }
                        //Close Long  - Trailing
                        if (SSPOP[x - 1].TrailingLevel > -1 && SSPOP[x].TrailingLevel == -1)
                        {
                            SSPOP[x].Position = false;
                            SSPOP[x].Direction = SS_Price.TradeDirection.None;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.CloseLong;
                            SSPOP[x].RunningProfit = SSPOP[x].ClosePrice - SSPOP[x].TradedPrice;
                            _Seq.TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = _Seq.TOTALPROFIT;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            goto Done;
                        }

                    }


                    //Close Short  - Any Cross
                    if (SSPOP[x].Direction == SS_Price.TradeDirection.Short)
                        if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Below_25 || SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Between)
                        {
                            SSPOP[x].Position = false;
                            SSPOP[x].Direction = SS_Price.TradeDirection.None;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.CloseShort;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            _Seq.TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = _Seq.TOTALPROFIT;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            goto Done;
                        }

                    //Close Short  - Trailing
                    if (SSPOP[x - 1].TrailingLevel > -1 && SSPOP[x].TrailingLevel == -1)
                    {
                        SSPOP[x].Position = false;
                        SSPOP[x].Direction = SS_Price.TradeDirection.None;
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.CloseShort;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        _Seq.TOTALPROFIT += SSPOP[x].RunningProfit;
                        SSPOP[x].TotalRunningProfit = _Seq.TOTALPROFIT;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                        goto Done;
                    }
                }

            Done:
                continue;
            }
        }
        //Step 4
        private void SetTriggers_StopLoss_TakeProfit()
        {
            var Count = SSPOP.Count;
            for (int x = 1; x < Count; x++)
            {
                //Long 
                if (SSPOP[x].Direction == SS_Price.TradeDirection.Long)
                {
                    if (SSPOP[x].ClosePrice >= SSPOP[x].TakeProfit)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.TakeProfit;
                        SSPOP[x].RunningProfit = SSPOP[x].ClosePrice - SSPOP[x].TradedPrice;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                    }
                    if (SSPOP[x].ClosePrice <= SSPOP[x].StopLoss)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.StopLoss;
                        SSPOP[x].RunningProfit = SSPOP[x].ClosePrice - SSPOP[x].TradedPrice;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                    }
                }

                //Short
                if (SSPOP[x].Direction == SS_Price.TradeDirection.Short)
                {
                    if (SSPOP[x].ClosePrice >= SSPOP[x].StopLoss)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.StopLoss;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                    }
                    if (SSPOP[x].ClosePrice <= SSPOP[x].TakeProfit)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.TakeProfit;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                    }
                }

                //End Of Day Close
                if (SSPOP[x].Stamp.Date != SSPOP[x - 1].Stamp.Date && _Seq.CLOSE_END_OF_DAY)
                {
                    SSPOP[x - 1].Trigger = SS_Price.TradeTrigger.CloseEndOfDay;
                    if (SSPOP[x - 1].Direction == SS_Price.TradeDirection.Long) SSPOP[x - 1].RunningProfit = SSPOP[x - 1].ClosePrice - SSPOP[x - 1].TradedPrice;
                    if (SSPOP[x - 1].Direction == SS_Price.TradeDirection.Short) SSPOP[x - 1].RunningProfit = SSPOP[x - 1].TradedPrice - SSPOP[x - 1].ClosePrice;
                    SSPOP[x - 1].TradedPrice = SSPOP[x - 1].ClosePrice;
                }
            }



            for (int x = 1; x < Count; x++)
            {
                if (SSPOP[x].Trigger == SS_Price.TradeTrigger.StopLoss)
                {
                    if (SSPOP[x].Direction == SS_Price.TradeDirection.Long) ;
                    if (SSPOP[x].Direction == SS_Price.TradeDirection.Short) ;
                }
                else
                    if (SSPOP[x].Trigger == SS_Price.TradeTrigger.TakeProfit)
                    {
                        if (SSPOP[x].Direction == SS_Price.TradeDirection.Long) ;
                        if (SSPOP[x].Direction == SS_Price.TradeDirection.Short) ;
                    }
            }
        }
        //Step 5
        private void GetTrades()
        {
            TradeList = new List<Trade>();
            var Count = SSPOP.Count;
            for (int x = 1; x < Count; x++)
            {
                if (SSPOP[x].Position && !SSPOP[x - 1].Position && SSPOP[x].Trigger != SS_Price.TradeTrigger.CloseEndOfDay)
                {

                    var Opentrade = SSPOP[x];

                    for (int q = x + 1; q < Count; q++)
                    {
                        if (SSPOP[q].Position && SSPOP[q].Trigger != SS_Price.TradeTrigger.None
                            || (!SSPOP[q].Position && SSPOP[q].Trigger == SS_Price.TradeTrigger.CloseLong || !SSPOP[q].Position && SSPOP[q].Trigger == SS_Price.TradeTrigger.CloseShort)
                            || (!SSPOP[q].Position && SSPOP[q].Trigger == SS_Price.TradeTrigger.CloseEndOfDay)
                            )
                        {
                            var Closetrade = SSPOP[q];
                            Trade t = new Trade()
                            {
                                OpenTrade = Opentrade,
                                CloseTrade = Closetrade,
                            };
                            TradeList.Add(t);
                            x = q;
                            break;
                        }



                    }
                }

            }


        }
        //Step 6 
        private void CalcBasicStats()
        {
            Profit = TradeList.Select(z => z.CloseTrade).Sum(x => x.RunningProfit);
            Trades = TradeList.Select(z => z.CloseTrade).Count();
            Console.WriteLine("Total Profit : {0}   Trades : {1} SEQ : {2}  ", Profit, Trades, _sequence);
        }

        private void WriteResults()
        {
            var sr = new StreamWriter(@"e:\POPtestRAW.csv");
            sr.WriteLine("Date,ClosePrice,SS,Last Cross,Limit,Trailing,Trigger,Position,Direction,Traded Price,PL,TPL,Take Profit,Stop Loss");
            foreach (var q in SSPOP)
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                    , q.Stamp, q.ClosePrice, q.SS.K, q.LastCross, q.Limit, q.TrailingLevel, q.Trigger, q.Position
                    , q.Direction, q.TradedPrice, q.RunningProfit, q.TotalRunningProfit, q.TakeProfit, q.StopLoss
                    );
            sr.Close();


            sr = new StreamWriter(@"e:\POPtestTradesOnly.csv");
            sr.WriteLine("Date,ClosePrice,SS,Last Cross,Limit,Trailing,Trigger,Position,Direction,Traded Price,PL,TPL,Take Profit,Stop Loss");
            foreach (var q in TradeList)
            {
                var o = q.OpenTrade;
                var c = q.CloseTrade;
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                    , o.Stamp, o.ClosePrice, o.SS.K, o.LastCross, o.Limit, o.TrailingLevel, o.Trigger, o.Position
                    , o.Direction, o.TradedPrice, o.RunningProfit, o.TotalRunningProfit, o.TakeProfit, o.StopLoss
                    );

                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                  , c.Stamp, c.ClosePrice, c.SS.K, c.LastCross, c.Limit, c.TrailingLevel, c.Trigger, c.Position
                  , c.Direction, c.TradedPrice, c.RunningProfit, c.TotalRunningProfit, c.TakeProfit, o.StopLoss
                  );
            }
            sr.Close();
        }

        private void WriteResultsToDatabase(string seq)
        {
            var r = new tblResults_5Min
            {
                Profit = Profit,
                Sequence = seq,
                Trades = Trades,

            };
            var dc = new AlgoSecondLayer.SPPOPRESULTDataContext();
            dc.tblResults_5Mins.InsertOnSubmit(r);
            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(seq + " already exists");
            }
        }

        class Seq
        {

            public int Fast_K { get; set; }
            public int Slow_K { get; set; }
            public int Slow_D { get; set; }

            public int UPPER_75 { get; set; }
            public int LOWER_25 { get; set; }
            public int LIMIT_HIGH { get; set; }
            public int LIMIT_LOW { get; set; }
            public double OPEN_Long_50 { get; set; }
            public double OPEN_Short_50 { get; set; }
            public double TOTALPROFIT { get; set; }
            public double STOPLOSS { get; set; }
            public double TAKEPROFIT { get; set; }
            public bool CLOSE_END_OF_DAY { get; set; }

            public Seq()
            {
                this.Fast_K = 11;
                this.Slow_K = 27;
                this.Slow_D = 24;
                this.UPPER_75 = 75;
                this.LOWER_25 = 25;
                this.LIMIT_HIGH = 85;
                this.LIMIT_LOW = 15;
                this.OPEN_Long_50 = 50;
                this.OPEN_Short_50 = 50;
                this.STOPLOSS = 5000;
                this.TAKEPROFIT = 5000;

                this.CLOSE_END_OF_DAY = false;
            }


            public Seq(string Sequence)
            {
                try
                {
                    var s = Sequence.Split(',');

                    this.Fast_K = int.Parse(s[0]);
                    this.Slow_K = int.Parse(s[1]);
                    this.Slow_D = int.Parse(s[2]);
                    this.UPPER_75 = int.Parse(s[3]);
                    this.LOWER_25 = int.Parse(s[4]);
                    this.LIMIT_HIGH = int.Parse(s[5]);
                    this.LIMIT_LOW = int.Parse(s[6]);
                    this.STOPLOSS = int.Parse(s[7]);
                    this.TAKEPROFIT = int.Parse(s[8]);
                    this.CLOSE_END_OF_DAY = true;
                }
                catch (Exception ex)
                {

                }
            }
        }

        public class Trade
        {
            public SS_Price OpenTrade { get; set; }
            public SS_Price CloseTrade { get; set; }
        }
        public class SS_Price
        {
            public double Volume { get; set; }
            public double ClosePrice { get; set; }
            public DateTime Stamp { get; set; }
            public SlowStoch SS { get; set; }
            public double TrailingLevel { get; set; }
            public double RunningProfit { get; set; }
            public double TotalRunningProfit { get; set; }
            public string Note1 { get; set; }
            public string Note2 { get; set; }
            public string Note3 { get; set; }
            public CrossType LastCross { get; set; }
            public TouchedLimit Limit { get; set; }
            public TradeTrigger Trigger { get; set; }
            public TradeDirection Direction { get; set; }
            public bool Position { get; set; }
            public double TradedPrice { get; set; }
            public double StopLoss { get; set; }
            public double TakeProfit { get; set; }

            public override string ToString()
            {
                var s = Stamp.ToString() + "  " + Trigger + " @ " + ClosePrice;
                return s;
            }

            public enum CrossType
            {
                Crossed_UP_Below_25,
                Crossed_DOWN_Above_75,
                Crossed_UP_Between,
                Crossed_DOWN_Between,
                None,
            }
            public enum TouchedLimit
            {
                Touched_Upper,
                Touched_Lower,
                None
            }

            public enum TradeTrigger
            {
                None,
                OpenLong,
                OpenShort,
                CloseLong,
                CloseShort,
                StopLoss,
                TakeProfit,
                CloseEndOfDay,
            }

            public enum TradeDirection
            {
                None,
                Long,
                Short
            }
        }


    }


}
