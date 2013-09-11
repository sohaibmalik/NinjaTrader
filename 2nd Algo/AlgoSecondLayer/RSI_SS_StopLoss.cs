using AlsiUtils;
using AlsiUtils.Data_Objects;
using AlsiUtils.Strategies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class RSI_SS_StopLoss
    {

        private List<Price> Prices = new List<Price>();
        private List<RSI_SS_Price> RSICC = new List<RSI_SS_Price>();
        Dictionary<DateTime, double> RSI_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> SS_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> PROF_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> VOL_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, string> NOTE1_DIC = new Dictionary<DateTime, string>();
        Dictionary<DateTime, bool> TRADETRIGGER_DIC = new Dictionary<DateTime, bool>();

        public void Start()
        {
            Prices = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
            GlobalObjects.Points = Prices;

            for (int rsi = 4; rsi < 30; rsi++)
                for (int fastK = 3; fastK < 30; fastK++)
                    for (int slowK = 3; slowK < 30; slowK++)
                        for (int slowD = 3; slowD < 30; slowD++)
                        {
                            //START LOOOP
                            ClearDictionaries();

                            var RSI = AlsiUtils.Factory_Indicator.createRSI(rsi, Prices);
                            var SS = AlsiUtils.Factory_Indicator.createSlowStochastic(fastK, slowK, slowD, Prices);


                            //CREATE DICTIONARY 
                            RSI_DIC = RSI.ToDictionary(x => x.TimeStamp, x => Math.Round(x.RSI, 3));
                            SS_DIC = SS.ToDictionary(x => x.TimeStamp, x => Math.Round(x.K, 3));

                            //POPULATE
                            foreach (var p in Prices)
                            {
                                RSI_SS_Price rsp = new RSI_SS_Price()
                                {
                                    ClosePrice = p.Close,
                                    Volume = p.Volume,
                                    Stamp = p.TimeStamp,
                                    RSI = 0,
                                    SS = 0,
                                };
                                double r, s;
                                RSI_DIC.TryGetValue(p.TimeStamp, out r);
                                SS_DIC.TryGetValue(p.TimeStamp, out s);

                                if (r != 0 && s != 0)
                                {
                                    rsp.RSI = r;
                                    rsp.SS = s;
                                    RSICC.Add(rsp);
                                }
                            }

                            //RUN CALCS
                            SetTriggers_A(10);
                            AddTradeslayer();
                            //  WriteResults();
                          


                            //END LOOP
                        }
        }

        public void CountPermutations()
        {
            List<string> Seq = new List<string>();
           
            for (int rsi = 4; rsi < 30; rsi++)
                for (int fastK = 3; fastK < 30; fastK++)
                    for (int slowK = 3; slowK < 30; slowK++)
                        for (int slowD = 3; slowD < 30; slowD++)
                        {
                            StringBuilder s = new StringBuilder();
                            s.Append(rsi + "," + fastK + "," + slowK + "," + slowD);
                            if(fastK !=slowK )
                            Seq.Add(s.ToString());                   
                        }

            string SIMcontext = @"Data Source=85.214.244.19;Initial Catalog=ALSI_SIM;User ID=SimLogin;Password=boeboe;MultipleActiveResultSets=True";
            var dc = new AlsiSimDataContext(SIMcontext);

            DataTable MinData = new DataTable("tblSequence");
            MinData.Columns.Add("Sequence", typeof(string));
            MinData.Columns.Add("Started", typeof(bool));
            MinData.Columns.Add("Completed", typeof(bool));
            foreach (var t in Seq)
            {
              
                MinData.Rows.Add(t, false, false);
                Debug.WriteLine("Adding {0}", t);
            }

            DataSet DataSet = new DataSet("Dataset");
            DataSet.Tables.Add(MinData);
            SqlConnection myConnection = new SqlConnection(SIMcontext);
            myConnection.Open();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(myConnection);
            bulkcopy.BulkCopyTimeout = 500000;
            bulkcopy.DestinationTableName = "tblSequence";
            bulkcopy.WriteToServer(MinData);
            MinData.Dispose();
            myConnection.Close();
        }

        private void SetTriggers_A(int lookback)
        {
            bool temptriggerLOW = false;
            bool temptriggerHIGH = false;

            for (int x = lookback; x < RSICC.Count; x++)
            {
                
                for (int lb = x - lookback; lb < x; lb++)
                {
                    if (RSICC[lb].RSI > 70 && RSICC[lb].SS > 70) RSICC[x].Trigger_High = true;
                    if (RSICC[lb].RSI < 30 && RSICC[lb].SS < 30) RSICC[x].Trigger_Low = true;
                }
                if (RSICC[x - 1].RSI > RSICC[x - 1].SS && RSICC[x].RSI < RSICC[x].SS) RSICC[x].Trigger_Crossed_Low = true;
                if (RSICC[x - 1].RSI < RSICC[x - 1].SS && RSICC[x].RSI > RSICC[x].SS) RSICC[x].Trigger_Crossed_High = true;

                if (RSICC[x].Trigger_High) temptriggerHIGH = true;
                if (RSICC[x].Trigger_Low) temptriggerLOW = true;

                if (!RSICC[x].Trigger_High && temptriggerHIGH && RSICC[x].Trigger_Crossed_High)
                {
                    RSICC[x].TradeTrigger = true;
                    TRADETRIGGER_DIC.Add(RSICC[x].Stamp, true);
                    temptriggerLOW = false;
                    temptriggerHIGH = false;
                }
                if (!RSICC[x].Trigger_Low && temptriggerLOW && RSICC[x].Trigger_Crossed_Low)
                {
                    RSICC[x].TradeTrigger = true;
                    TRADETRIGGER_DIC.Add(RSICC[x].Stamp, true);
                    temptriggerLOW = false;
                    temptriggerHIGH = false;
                }



            }


        }

        private void ClearDictionaries()
        {
            RSI_DIC.Clear();
            SS_DIC.Clear();
            PROF_DIC.Clear();
            VOL_DIC.Clear(); ;
            NOTE1_DIC.Clear();
            TRADETRIGGER_DIC.Clear();
            RSICC.Clear();
        }


        private Statistics _Stats = new Statistics();
        private List<Trade> _FullTradeList;
        private List<CompletedTrade> NewTrades;
        public List<Trade> _TradeOnlyList;
        private double TOTALPROFIT = 0;
        private void AddTradeslayer()
        {
            var PM = GetParametersMAMA();
            _FullTradeList = AlsiUtils.Strategies.MAMA_Scalp.MAMAScalp(PM, GlobalObjects.Points, false);
            _FullTradeList = _Stats.CalcBasicTradeStats_old(_FullTradeList);
            NewTrades = TradeStrategy.Expansion.ApplyRegressionFilter(11, _FullTradeList);
            NewTrades = _Stats.CalcExpandedTradeStats(NewTrades);
            _TradeOnlyList = CompletedTrade.CreateList(NewTrades);

       

           
           int C = NewTrades.Count;
            for (int i = 1; i < C; i++)
            {
                var pl = from x in _FullTradeList
                         where x.TimeStamp >= NewTrades[i].OpenTrade.TimeStamp && x.TimeStamp <= NewTrades[i].CloseTrade.TimeStamp
                         select x;

                double volume = 0;
                double volProfit = 0;
                var PL = pl.ToList();
                
                foreach (var q in PL)
                {
                    bool tookprofit = false;
                    volume = PL[0].TradeVolume;
                    TRADETRIGGER_DIC.TryGetValue(q.TimeStamp, out tookprofit);

                    if (q.Reason == Trade.Trigger.None && ! tookprofit ) volProfit = volume * q.RunningProfit;
                    else
                        volProfit = q.RunningProfit; 
                                      

                    PROF_DIC.Add(q.TimeStamp, volProfit);
                    NOTE1_DIC.Add(q.TimeStamp, q.Reason.ToString());
                    VOL_DIC.Add(q.TimeStamp,volume );
                    if (tookprofit)
                    {
                        TOTALPROFIT += volProfit;
                        break;
                    }
                    else
                        if (q.Reason==Trade.Trigger.CloseLong || q.Reason ==Trade.Trigger.CloseShort) TOTALPROFIT += PL.Last().RunningProfit;
                }

               
                foreach (var t in RSICC.Where(x=>x.Stamp>=PL[0].TimeStamp && x.Stamp <=PL[PL.Count].TimeStamp))
                {
                    double r = 0;
                    double v = 0;
                    string note = "";
                    PROF_DIC.TryGetValue(t.Stamp, out r);
                    NOTE1_DIC.TryGetValue(t.Stamp, out note);
                    VOL_DIC.TryGetValue(t.Stamp, out v);
                    t.Runningprofit = r;
                    t.Note1 = note;
                    t.Volume = v;
                }

              
                
            
            

            //End loop
            }
            Console.WriteLine(TOTALPROFIT);

          
            
        }

        private void WriteResults()
        {

            var sr = new StreamWriter(@"D:\test.csv");
            foreach (var q in RSICC)
                sr.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}"
                    , q.Stamp, q.ClosePrice, q.RSI, q.SS, q.Trigger_High,
                    q.Trigger_Low, q.Trigger_Crossed_High, q.Trigger_Crossed_Low, q.TradeTrigger,
                    q.Runningprofit, q.Note1, q.Volume
                    );
            sr.Close();
        }

        private Parameter_MAMA GetParametersMAMA()
        {


            AlsiUtils.Strategies.Parameter_MAMA E = new AlsiUtils.Strategies.Parameter_MAMA()
            {

                Fast = 0.5,//0.1
                Slow = 0.05,//0.01
                A_EMA1 = 16,
                A_EMA2 = 17,
                B_EMA1 = 43,
                B_EMA2 = 45,
                C_EMA = 52,
                TakeProfit = 55550,
                StopLoss = -250,
                //TakeProfitFactor = TPF,
                //StoplossFactor = SLF,
                CloseEndofDay = false,
            };

            return E;
        }
        class RSI_SS_Price
        {
            public double Volume { get; set; }
            public double ClosePrice { get; set; }
            public DateTime Stamp { get; set; }
            public double RSI { get; set; }
            public double SS { get; set; }
            public bool Trigger_High { get; set; }
            public bool Trigger_Low { get; set; }
            public bool Trigger_Crossed_High { get; set; }
            public bool Trigger_Crossed_Low { get; set; }
            public bool TradeTrigger { get; set; }
            public double Runningprofit { get; set; }
            public string Note1 { get; set; }
            public string Note2 { get; set; }
            public string Note3 { get; set; }
        }

    }
}
