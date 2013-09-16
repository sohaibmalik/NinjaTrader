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
        public event EventHandler Done;
       

        private List<Price> Prices = new List<Price>();
        private List<RSI_SS_Price> RSICC = new List<RSI_SS_Price>();
        Dictionary<DateTime, double> RSI_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> SS_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> PROF_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, double> VOL_DIC = new Dictionary<DateTime, double>();
        Dictionary<DateTime, string> NOTE1_DIC = new Dictionary<DateTime, string>();
        Dictionary<DateTime, bool> TRADETRIGGER_DIC = new Dictionary<DateTime, bool>();

       
        private string SIMCONTEXT = "";
        private Seq _Seq;
      
        public void Start(string simcontext)
        {
            SIMCONTEXT = simcontext;
          
         
                var Prices = GlobalObjects.Points;
             
        

            //for (int rsi = 4; rsi < 30; rsi++)
            //    for (int fastK = 3; fastK < 30; fastK++)
            //        for (int slowK = 3; slowK < 30; slowK++)
            //            for (int slowD = 3; slowD < 30; slowD++)
            //            {
            var dc = new AlsiSimDataContext(simcontext);
            var m = dc.tblSequences.Where(x => !x.Started).Count();
            var _skip = Utils.RandomNumber(0, m - 1);

            var _sequence = dc.tblSequences.Where(x => !x.Started).Skip(_skip).First();
            var par = _sequence.Sequence.Split(',');
            //Might cuase duplicates, but chances are slim
           // _sequence.Started = true;
           // dc.SubmitChanges();

            _Seq = new Seq(_sequence.Sequence);
            var rsi = _Seq.RSI;
            var fastK = _Seq.Fast_K;
            var slowK = _Seq.Slow_K;
            var slowD = _Seq.Slow_D;

            //START LOOOP
            ClearDictionaries();

            var RSI = AlsiUtils.Factory_Indicator.createRSI(rsi, Prices);
            var SS = AlsiUtils.Factory_Indicator.createSlowStochastic(fastK, slowK, slowD, Prices);


            //CREATE DICTIONARY 
            RSI_DIC = RSI.ToDictionary(x => x.TimeStamp, x => Math.Round(x.RSI, 3));
            SS_DIC = SS.ToDictionary(x => x.TimeStamp, x => Math.Round(x.D , 3));

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
            int lookback = Utils.RandomNumber(5, 15);
            _Seq.Lookback = lookback;
            SetTriggers_A(lookback);
            var profit = AddTradeslayer();

            Console.WriteLine("{0}  {1} {2} {3} {4} LB:{5} H:{6} L:{7}", profit, rsi, fastK, slowK, slowD,_Seq.Lookback,_Seq.Upper,_Seq.Lower );
            WriteResults();
           // WriteResultsToDatabase(dc,_sequence, profit);

          //  Done(this, new EventArgs());
           
            //END LOOP
            //}
        }


        private void WriteResultsToDatabase(AlsiSimDataContext dc,tblSequence seq, double profit)
        {
         
            var r = new tblResult_5Min_D()
            {
                Profit=profit,
                Sequence=seq.Sequence+" l:"+_Seq.Lookback+" H:"+_Seq.Upper+" L:"+_Seq.Lower,
                Trades=0,
                Notes = " l:" + _Seq.Lookback + " H:" + _Seq.Upper + " L:" + _Seq.Lower,
            };
            dc.tblResult_5Min_Ds.InsertOnSubmit(r);
            dc.SubmitChanges();
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
                            if (fastK != slowK)
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
            int h = Utils.RandomNumber(55, 85);
            int l = Utils.RandomNumber(15, 45);
            _Seq.Lower = l;
            _Seq.Upper = h;

            for (int x = lookback; x < RSICC.Count; x++)
            {

                for (int lb = x - lookback; lb < x; lb++)
                {                   

                    if (RSICC[lb].RSI > h && RSICC[lb].SS > h) RSICC[x].Trigger_High = true;
                    if (RSICC[lb].RSI < l && RSICC[lb].SS < l) RSICC[x].Trigger_Low = true;
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
        private double AddTradeslayer()
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

                    if (q.Reason == Trade.Trigger.None && !tookprofit) volProfit = volume * q.RunningProfit;
                    else
                        volProfit = q.RunningProfit;


                    PROF_DIC.Add(q.TimeStamp, volProfit);
                    NOTE1_DIC.Add(q.TimeStamp, q.Reason.ToString());
                    VOL_DIC.Add(q.TimeStamp, volume);
                    if (tookprofit)
                    {
                        TOTALPROFIT += volProfit;
                        break;
                    }
                    else
                        if (q.Reason == Trade.Trigger.CloseLong || q.Reason == Trade.Trigger.CloseShort) TOTALPROFIT += PL.Last().RunningProfit;
                }

                var R = RSICC.Where(x => x.Stamp >= PL[0].TimeStamp && x.Stamp <= PL[PL.Count - 1].TimeStamp);
                foreach (var t in R)
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
            return TOTALPROFIT;



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

        class Seq
        {
            public int RSI { get; set; }
            public int Fast_K { get; set; }
            public int Slow_K { get; set; }
            public int Slow_D { get; set; }
            public int Lookback { get; set; }
            public int Upper { get; set; }
            public int Lower { get; set; }

            public Seq(string Sequence)
            {
                var s = Sequence.Split(',');
                this.RSI = int.Parse(s[0]);
                this.Fast_K = int.Parse(s[1]);
                this.Slow_K = int.Parse(s[2]);
                this.Slow_D = int.Parse(s[3]);

            }
        }
               
    }
}
