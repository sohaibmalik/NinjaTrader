using BTL.generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GATest
{
    public class EvolvingTradingSystemTest
    {
        private List<TradePeriod> _Periods = new List<TradePeriod>();
       

        public void Start()
        {
            Console.WriteLine("Starting Evolving Aglo");
            GetAllData();
            CreateDateGroups();
            RunEvolve();
            WriteTradesToFile();
        }

        private void GetAllData()
        {

            string localdata = @"Data Source=PITER-PC;Initial Catalog=AlsiTrade;Integrated Security=True";
            string remotedata = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";

            Console.WindowWidth = 100;
            Console.WriteLine("Getting Prices...");
            AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = remotedata;

            AlsiUtils.Data_Objects.GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
            Console.WriteLine("Done.");
        }

        private void CreateDateGroups()
        {


            var weekly = from q in AlsiUtils.Data_Objects.GlobalObjects.Points
                         group q by new
                         {
                             Y = q.TimeStamp.Year,
                             M = q.TimeStamp.Month,
                             W = Math.Floor((decimal)q.TimeStamp.DayOfYear / 7) + 1,
                             //D=(DateTime)q.TimeStamp
                         }
                             into FGroup
                             orderby FGroup.Key.Y, FGroup.Key.M, FGroup.Key.W
                             select new
                             {
                                 Year = FGroup.Key.Y,
                                 Month = FGroup.Key.M,
                                 Week = FGroup.Key.W,

                                 FirstTradeDate = FGroup.First().TimeStamp,
                                 LastTradeDate = FGroup.Last().TimeStamp,
                                 //AvPrice = (double)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                 // .Average(t => t.RunningProfit),
                                 //SumPrice = (int)FGroup.Where(z => z.Reason == Trade.Trigger.CloseShort || z.Reason == Trade.Trigger.CloseLong)
                                 //.Sum(t => t.RunningProfit),
                                 //marketmovement = (FGroup.Last().CurrentPrice) - (FGroup.First().CurrentPrice),
                                 //Prices = FGroup.Select(z => z.CurrentPrice),

                             };

            foreach (var v in weekly)
            {
                Console.WriteLine("Year {0} Month {1} Week {2}  Start {3}  End {4}", v.Year, v.Month, v.Week, v.FirstTradeDate, v.LastTradeDate);
                var tp = new TradePeriod()
                {
                    Start = v.FirstTradeDate,
                    End = v.LastTradeDate,

                };
                _Periods.Add(tp);

            }

        }

        private void RunEvolve()
        {
            var firstSeq = RunAlgo().GetSequence();
            _Periods[0].Seq = firstSeq;
            for (int x = 1; x < _Periods.Count; x++)
            {
                
                _Periods[x].Seq_prev  = _Periods[x - 1].Seq;
                _Periods[x].Seq = RunAlgo().GetSequence();

                var s = new AlgoSecondLayer.StochPOP();
                s.StartDate = _Periods[x].Start;
                s.EndDate = _Periods[x].End;
                var o = s.Start(_Periods[x].Seq_prev, null, true);
              
               _Periods[x].Profit= double.Parse(o[1]);
               _Periods[x].TradeCount = int.Parse(o[2]);
                
            }
            Console.WriteLine("Stop");
            Console.ReadLine();
        }

        private void WriteTradesToFile()
        {
            var sr = new StreamWriter(@"d:\Evolving.csv");
        
            sr.WriteLine("StartDate,EndDate,Profit,Trades,Seq");
            foreach (var v in _Periods)
            {
                sr.WriteLine("{0},{1},{2},{3},{4}", v.Start, v.End, v.Profit, v.TradeCount, v.Seq_prev);
            }
            sr.Close();
        }

        public double theActualFunction(double[] values)
        {
            if (values.GetLength(0) != 9)
                throw new ArgumentOutOfRangeException("should only have 9 args");


            var bb = @" this.Fast_K = int.Parse(s[0]);
                    this.Slow_K = int.Parse(s[1]);
                    this.Slow_D = int.Parse(s[2]);
                    this.UPPER_75 = int.Parse(s[3]);
                    this.LOWER_25 = int.Parse(s[4]);
                    this.LIMIT_HIGH = int.Parse(s[5]);
                    this.LIMIT_LOW = int.Parse(s[6]);
                    this.STOPLOSS = int.Parse(s[7]);
                    this.TAKEPROFIT = int.Parse(s[8]);
                    this.CLOSE_END_OF_DAY = false;";



            double a = values[0];
            double b = values[1];
            double c = values[2];
            double d = values[3];
            double e = values[4];
            double f = values[5];
            double g = values[6];
            double h = values[7];
            double i = values[8];
            // double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(n * Math.PI * x) * Math.Sin(n * Math.PI * y), 2);
            //return f1;

            var seq = ((int)a).ToString() + "," + ((int)b).ToString() + "," + ((int)c).ToString() +
                "," + ((int)d).ToString() + "," + ((int)e).ToString() + "," + ((int)f).ToString() +
                 "," + ((int)g).ToString() + "," + ((int)h).ToString() + "," + ((int)i).ToString();

            var s = new AlgoSecondLayer.StochPOP();

            var o = s.Start(seq, null, true);


            //AddOutput("=================", true);
            //AddOutput("Sequence " + o[0], true);
            //AddOutput("Profit " + o[1], true);
            //AddOutput("Trades " + o[2], true);

            double profit = double.Parse(o[1]);
            double trades = double.Parse(o[2]);
            double avg = profit / (trades + 1);

            return profit;

        }


        private AlgoOutput RunAlgo()
        {
            //  Crossover		= 80%
            //  Mutation		=  5%
            //  Population size = 100
            //  Generations		= 2000
            //  Genome size		= 2
            GA ga = new GA(0.8, 0.05, 1000, 10, 9, MaxMin.Minimize);

            ga.FitnessFunction = new GAFunction(theActualFunction);

            //ga.FitnessFile = @"E:\fitness.csv";
            ga.Elitism = true;
            ga.Go();

            double[] values_Best;
            double fitness_Best;
            double[] values_Worst;
            double fitness_Worst;
            ga.GetBest(out values_Best, out fitness_Best);
            Console.WriteLine("Best ({0}):", fitness_Best);
            for (int i = 0; i < values_Best.Length; i++)
                Console.WriteLine("{0} ", values_Best[i]);

            ga.GetWorst(out values_Worst, out fitness_Worst);
            Console.WriteLine("\nWorst ({0}):", fitness_Worst);
            for (int i = 0; i < values_Worst.Length; i++)
                Console.WriteLine("{0} ", values_Worst[i]);


            var ar = new AlgoOutput()
            {
                Best=fitness_Best,
                Worst=fitness_Worst,
                Values_Best=values_Best,
                Values_Worst=values_Worst,
               
            };
         
            return ar;
            
        }

    }

    public class TradePeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Profit { get; set; }
        public int TradeCount { get; set; }
        public string Seq { get; set; }
        public string Seq_prev { get; set; }
    }
    public class AlgoOutput
    {
        public double Best { get; set; }
        public double Worst { get; set; }
        public double [] Values_Best { get; set; }
        public double[] Values_Worst { get; set; }

        public string GetSequence()
        {
            StringBuilder seq = new StringBuilder();
            int count = Values_Best.Count()-1;
            int c=0;
            foreach (var v in Values_Best)
            {
                if (c < count)
                {
                    seq.AppendLine(v.ToString() + ",");
                    c++;
                }
                else
                seq.AppendLine(v.ToString());
            }
            return seq.ToString();

            
        }
    }
}
