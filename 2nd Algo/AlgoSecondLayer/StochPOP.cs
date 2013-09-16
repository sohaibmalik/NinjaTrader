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

        private int UPPER_75 = 75;
        private int LOWER_25 = 25;
        private int LIMIT_HIGH = 85;
        private int LIMIT_LOW = 15;
        private double OPEN_Long_50 = 50;
        private double OPEN_Short_50 = 50;
        private double TOTALPROFIT = 0;

        public void Start()
        {

            var Prices = GlobalObjects.Points;
            SSPOP = new List<SS_Price>();

            Dictionary<DateTime, SlowStoch> SS_DIC = new Dictionary<DateTime, SlowStoch>();
            Dictionary<DateTime, double> PROF_DIC = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> VOL_DIC = new Dictionary<DateTime, double>();

            //START LOOOP


            int fastK = 16;
            int slowK = 12;
            int slowD = 6;

            var SS = AlsiUtils.Factory_Indicator.createSlowStochastic(fastK, slowK, slowD, Prices);


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


            WriteResults();
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
                    if (SSPOP[x - 1].SS.D <= LOWER_25) SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_UP_Below_25;
                }
                else
                    //Crossed DOWN
                    if (SSPOP[x - 1].SS.K > SSPOP[x - 1].SS.D && SSPOP[x].SS.K < SSPOP[x].SS.D)
                    {
                        SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_DOWN_Between;
                        if (SSPOP[x - 1].SS.D >= UPPER_75) SSPOP[x].LastCross = StochPOP.SS_Price.CrossType.Crossed_DOWN_Above_75;
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
                if (SSPOP[x - 1].SS.K < LIMIT_HIGH && SSPOP[x].SS.K > LIMIT_HIGH)
                {
                    SSPOP[x].Limit = StochPOP.SS_Price.TouchedLimit.Touched_Upper;
                    SSPOP[x].TrailingLevel = SSPOP[x].SS.K;
                }
                else
                    //Touched LOW
                    if (SSPOP[x - 1].SS.K > LIMIT_LOW && SSPOP[x].SS.K < LIMIT_LOW)
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
                SSPOP[x].TradedPrice = SSPOP[x-1].TradedPrice;

                if (!SSPOP[x].Position) //Open Triggers
                {
                    //Open Long - 50
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Between)
                        if (SSPOP[x - 1].SS.K < OPEN_Long_50 && SSPOP[x].SS.K > OPEN_Long_50)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            goto Done;
                        }

                    //Open Long - 75
                    if (SSPOP[x - 1].SS.K < UPPER_75 && SSPOP[x].SS.K > UPPER_75)
                    {
                        SSPOP[x].Position = true;
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                        SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        goto Done;
                    }

                    //Open Long > 25
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_UP_Below_25)
                        if (SSPOP[x - 1].SS.K < LOWER_25 && SSPOP[x].SS.K > LOWER_25)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenLong;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Long;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            goto Done;
                        }

                    //Open Short - 50
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Between)
                        if (SSPOP[x - 1].SS.K > OPEN_Short_50 && SSPOP[x].SS.K < OPEN_Short_50)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            goto Done;
                        }

                    //Open Short - 25
                    if (SSPOP[x - 1].SS.K < UPPER_75 && SSPOP[x].SS.K > UPPER_75)
                    {
                        SSPOP[x].Position = true;
                        SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                        SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                        SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                        SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                        goto Done;
                    }

                    //Open Short < 75
                    if (SSPOP[x].LastCross == SS_Price.CrossType.Crossed_DOWN_Above_75)
                        if (SSPOP[x - 1].SS.K > UPPER_75 && SSPOP[x].SS.K < UPPER_75)
                        {
                            SSPOP[x].Position = true;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.OpenShort;
                            SSPOP[x].Direction = SS_Price.TradeDirection.Short;
                            SSPOP[x].TradedPrice = SSPOP[x].ClosePrice;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
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
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = TOTALPROFIT;
                            SSPOP[x].TradedPrice = 0;
                            goto Done;
                        }
                        //Close Long  - Trailing
                        if (SSPOP[x - 1].TrailingLevel > -1 && SSPOP[x].TrailingLevel == -1)
                        {
                            SSPOP[x].Position = false;
                            SSPOP[x].Direction = SS_Price.TradeDirection.None;
                            SSPOP[x].Trigger = SS_Price.TradeTrigger.CloseLong;
                            SSPOP[x].RunningProfit = SSPOP[x].TradedPrice - SSPOP[x].ClosePrice;
                            TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = TOTALPROFIT;
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
                            TOTALPROFIT += SSPOP[x].RunningProfit;
                            SSPOP[x].TotalRunningProfit = TOTALPROFIT;
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
                        TOTALPROFIT += SSPOP[x].RunningProfit;
                        SSPOP[x].TotalRunningProfit = TOTALPROFIT;
                        SSPOP[x].TradedPrice = 0;
                        goto Done;
                    }
                }

            Done:
                continue;
            }
        }

        private void WriteResults()
        {
            var sr = new StreamWriter(@"D:\POPtest.csv");
            sr.WriteLine("Date,ClosePrice,SS,Last Cross,Limit,Trailing,Trigger,Position,Direction,Traded Price,PL,TPL");
            foreach (var q in SSPOP)
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                    , q.Stamp, q.ClosePrice, q.SS.K, q.LastCross, q.Limit, q.TrailingLevel, q.Trigger, q.Position
                    , q.Direction,q.TradedPrice ,q.RunningProfit ,q.TotalRunningProfit 
                    );
            sr.Close();
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
