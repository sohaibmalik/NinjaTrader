using AlsiUtils;
using RuleModelCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;



namespace FrontEnd
{
    public class GeneticBackend
    {
        #region ALSI
        private static GeneticBackend GB;
        private MainForm _mf;
        private RuleModelCalc.objCalcData _objCalcData;
        private FitnessFunction _FitnessFunction;
        public MainForm Mf
        {
            get { return _mf; }
            set
            {
                _mf = value;
                _mf.OnGeneticNotified += _mf_OnGeneticNotified;
            }
        }

        void _mf_OnGeneticNotified(object sender, MainForm.NotifyOnPriceSynch e)
        {
            Debug.WriteLine("Starting Algo Claculations");
            SetGenes();
            ConvertData();
            RunAlgo();
            GetLastEvent();
        }

        public GeneticBackend()
        {
            _objCalcData = new objCalcData();
            _FitnessFunction = new FitnessFunction(_objCalcData);
        }


        public static GeneticBackend GetSingletonGB()
        {
            if (GB == null) GB = new GeneticBackend();
            return GB;
        }

        #endregion




        public void SetGenes()
        {

            //Hardcode template

            _objCalcData.TEMPLATES_Long_Entry.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Long_Entry.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Long_Entry.Add(new EMA_Price_Cross());

            _objCalcData.TEMPLATES_Long_Exit.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Long_Exit.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Long_Exit.Add(new EMA_Price_Cross());

            _objCalcData.TEMPLATES_Short_Entry.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Short_Entry.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Short_Entry.Add(new EMA_Price_Cross());

            _objCalcData.TEMPLATES_Short_Exit.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Short_Exit.Add(new EMA_Price_Cross());
            _objCalcData.TEMPLATES_Short_Exit.Add(new EMA_Price_Cross());


            StandardExitStrategy.Long_Stoploss_Points = 275;
            StandardExitStrategy.Long_TakeProfit_Points = 706;
            StandardExitStrategy.Long_TimeExit_Bars = 132;
            StandardExitStrategy.Short_Stoploss_Points = 384;
            StandardExitStrategy.Short_TakeProfit_Points = 682;
            StandardExitStrategy.Short_TimeExit_Bars = 226;

            AlgoSettings.General.RunSingleResult = true;




        }

        public void ConvertData()
        {
            var lp = AlsiUtils.Data_Objects.GlobalObjects.Points.Last();

            _objCalcData.Prices = new List<Indicators.Price>();
            foreach (var p in AlsiUtils.Data_Objects.GlobalObjects.Points)
            {
                var newprice = new Indicators.Price()
                {
                    TimeStamp = p.TimeStamp,
                    Open = p.Open,
                    High = p.High,
                    Low = p.Low,
                    Close = p.Close,
                    Volume = p.Volume,
                };
                _objCalcData.Prices.Add(newprice);

            }
        }

        public void RunAlgo()
        {
            double[] chromosome = new double[]
            {   //Long entry
                0,40,0,
                1,43,0,
                2,41,0,
                //short entry
                0,25,0,
                1,8,1,
                2,35,0,
                //Long exit
                0,40,0,
                1,58,1,
                2,17,1,
                //Short exit
                0,32,0,
                1,40,0,
                2,56,1,
            };

            _objCalcData.genomeSize = chromosome.Count();

            var pl = _FitnessFunction.AlgoFitnessFunction(chromosome);
        }

        public void GetLastEvent()
        {
            var BT = _objCalcData.BoolTriggers_LONG_SHORT;
            var algotime = AlsiTrade_Backend.DoStuff.GetAlgoTime();
            var currentOrder = BT.Where(z => z.Trade.UnderlayingPrice.TimeStamp.Hour >= algotime.Hour && z.Trade.UnderlayingPrice.TimeStamp.Minute >= algotime.Minute && z.Trade.UnderlayingPrice.TimeStamp.Date == algotime.Date).First();
         
            var E = new GeneticUpdateEventsArgs();
            E.Msg = "=====================GENETIC ALGO UPDATE....===============";
            E.FinalTrigger = currentOrder;
            E.oTrade = BuildTrade(currentOrder);
            onUpdate(this, E);
        }

        private AlsiUtils.Trade BuildTrade(RuleModelCalc.objCalcData.FinalTrigger t)
        {
            ////Test
            //t.Trade.TradeTrigger_Open = objTrade.TriggerOpen.ReverseLong;
            //t.Trade.TradeTrigger_Close = objTrade.TriggerClose.CloseShort;

            var vol = 0;
            var T = new AlsiUtils.Trade();
            T.BackColor = Color.Black;
            T.InstrumentName = WebSettings.General.OTS_INST;
            T.BuyorSell = GetBS(t.Trade, out vol);
            T.TradeVolume = vol;
            return T;
        }

        private AlsiUtils.Trade.BuySell GetBS(objTrade t, out int Vol)
        {
            int x = 1;
            Trade.BuySell bs = Trade.BuySell.None;

            if (t.TradeTrigger_Open == objTrade.TriggerOpen.OpenLong && t.Exit_Reason == objTrade.ExitReason.ReverseSignal)
            {
                x = 2;
                bs = Trade.BuySell.Buy;
                Vol = WebSettings.General.VOL * x;
                return bs;
            }

            if (t.TradeTrigger_Open == objTrade.TriggerOpen.OpenLong && t.Exit_Reason == objTrade.ExitReason.ReverseSignal)
            {
                x = 2;
                bs = Trade.BuySell.Sell;
                Vol = WebSettings.General.VOL * x;
                return bs;
            }

            
            if (t.TradeTrigger_Open == objTrade.TriggerOpen.OpenLong) bs = Trade.BuySell.Buy;
            if (t.TradeTrigger_Open == objTrade.TriggerOpen.OpenShort) bs = Trade.BuySell.Sell;

            if (t.TradeTrigger_Close == objTrade.TriggerClose.CloseLong) bs = Trade.BuySell.Sell;
            if (t.TradeTrigger_Close == objTrade.TriggerClose.CloseShort) bs = Trade.BuySell.Buy;

          


            Vol = WebSettings.General.VOL * x;

            return bs;
        }

        public event GeneticTradeUpdate onUpdate;
        public delegate void GeneticTradeUpdate(object sender, GeneticUpdateEventsArgs e);
        public class GeneticUpdateEventsArgs : EventArgs
        {
            public string Msg { get; set; }
            public AlsiUtils.Trade oTrade { get; set; }
            public RuleModelCalc.objCalcData.FinalTrigger FinalTrigger { get; set; }
        }


    }
}
