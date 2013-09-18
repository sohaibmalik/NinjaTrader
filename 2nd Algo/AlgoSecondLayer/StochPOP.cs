using AlsiUtils;
using AlsiUtils.Data_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class StochPOP
    {
        private List<SS_Price> SSPOP;
        private Seq _Seq;

        public void Start(string simcontext)
        {
            //var dc = new AlsiSimDataContext(simcontext);
            //var m = dc.tblSequences.Where(x => !x.Started).Count();
            //var _skip = Utils.RandomNumber(0, m - 1);

            //var _sequence = dc.tblSequences.Where(x => !x.Started).Skip(_skip).First();
            //var par = _sequence.Sequence.Split(',');
            ////Might cuase duplicates, but chances are slim
            //_sequence.Started = true;
            //dc.SubmitChanges();

            _Seq = new Seq();
            //  _Seq = new Seq(_sequence.Sequence);  


            var Prices = GlobalObjects.Points;
            SSPOP = new List<SS_Price>();

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

            WriteResults();
            // WriteResultsToDatabase(dc,_sequence , TOTALPROFIT);
            Console.WriteLine("{0}  {1} {2} {3} ", _Seq.TOTALPROFIT, _Seq.Fast_K, _Seq.Slow_K, _Seq.Slow_D);
            //END LOOP
            //}
        }



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
                            SSPOP[x].TradedPrice = 0;
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
                            SSPOP[x].TradedPrice = 0;
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
                            SSPOP[x].TradedPrice = 0;
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
                        SSPOP[x].TradedPrice = 0;
                        goto Done;
                    }
                }

            Done:
                continue;
            }
        }


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
                    }
                    if (SSPOP[x].ClosePrice <= SSPOP[x].StopLoss)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.StopLoss;
                        SSPOP[x].RunningProfit = SSPOP[x].ClosePrice - SSPOP[x].TradedPrice;
                    }
                }

                //Short
                if (SSPOP[x].Direction == SS_Price.TradeDirection.Short)
                {
                    if (SSPOP[x].ClosePrice >= SSPOP[x].StopLoss)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.StopLoss;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                    }
                    if (SSPOP[x].ClosePrice <= SSPOP[x].TakeProfit)
                    {
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.TakeProfit;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                    }
                }

                //End Of Day Close
                if (SSPOP[x].Stamp.Date != SSPOP[x - 1].Stamp.Date && _Seq.CLOSE_END_OF_DAY)
                {
                    SSPOP[x - 1].Trigger = SS_Price.TradeTrigger.CloseEndOfDay;
                    if (SSPOP[x-1].Direction == SS_Price.TradeDirection.Long) SSPOP[x-1].RunningProfit = SSPOP[x-1].ClosePrice - SSPOP[x-1].TradedPrice;
                    if (SSPOP[x-1].Direction == SS_Price.TradeDirection.Short) SSPOP[x-1].RunningProfit = SSPOP[x-1].TradedPrice - SSPOP[x-1].ClosePrice;
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

        private void WriteResults()
        {
            var sr = new StreamWriter(@"D:\POPtest.csv");
            sr.WriteLine("Date,ClosePrice,SS,Last Cross,Limit,Trailing,Trigger,Position,Direction,Traded Price,PL,TPL,Take Profit,Stop Loss");
            foreach (var q in SSPOP)
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}"
                    , q.Stamp, q.ClosePrice, q.SS.K, q.LastCross, q.Limit, q.TrailingLevel, q.Trigger, q.Position
                    , q.Direction, q.TradedPrice, q.RunningProfit, q.TotalRunningProfit, q.TakeProfit, q.StopLoss
                    );
            sr.Close();
        }

        private void WriteResultsToDatabase(AlsiSimDataContext dc, tblSequence seq, double profit)
        {
            var r = new tblResult_5Min_SSPOP()
            {
                Profit = profit,
                Sequence = seq.Sequence,
                Trades = 0,
            };
            dc.tblResult_5Min_SSPOPs.InsertOnSubmit(r);
            dc.SubmitChanges();
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
                this.STOPLOSS = 50;
                this.TAKEPROFIT = 50;

                this.CLOSE_END_OF_DAY = true;
            }


            public Seq(string Sequence)
            {
                var s = Sequence.Split(',');

                this.Fast_K = int.Parse(s[0]);
                this.Slow_K = int.Parse(s[1]);
                this.Slow_D = int.Parse(s[2]);

            }
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
