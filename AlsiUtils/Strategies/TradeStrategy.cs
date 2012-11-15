
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlsiUtils.Strategies
{
    public class TradeStrategy : Indicator
    {
        private static List<TradeStrategy> _ST = new List<TradeStrategy>();
        private static Parameter_General _p = new Parameter_General();

        public delegate void Triggers_Delegate(List<TradeStrategy> TradeStraterty, int Index);
        public static Triggers_Delegate _Trigger;

        public TradeStrategy()
        {

        }
        public TradeStrategy(List<Price> Prices, Parameter_General parameter, DateTime startPeriod, Triggers_Delegate Triggers)
        {
            _p.StopLoss = parameter.StopLoss;
            _p.TakeProfit = parameter.TakeProfit;
            _Trigger = Triggers;

            foreach (Price s in Prices)
            {
                if (s.TimeStamp >= startPeriod)
                {
                    TradeStrategy st = new TradeStrategy
                    {
                        Timestamp = s.TimeStamp,
                        Price_Open = s.Open,
                        Price_Close = s.Close,
                        Price_High = s.High,
                        Price_Low = s.Low,
                        InstrumentName = s.InstrumentName
                    };
                    _ST.Add(st);
                }
            }

        }

        public List<TradeStrategy> getStrategyList()
        {
            return _ST;
        }

        public void Calculate()
        {
            Prepare();
            Triggers();

            PositionAndDirection();
            TradeSignals();


            CalcProfitLoss();
            AddStopLossTriggers();

            Mark();
            ClearProf();

            Triggers();
            PositionAndDirection();
            TradeSignals();
            CalcProfitLoss();


            AddStopLossTriggers();
            Mark();
            ClearProf();
            Triggers();
            PositionAndDirection();
            TradeSignals();
            CalcProfitLoss();




            Stats();
            // */

        }

        public void ClearList()
        {
            _ST.Clear();
        }

        private void Prepare()
        {
            _ST[0].Position = false;
            _ST[0].TradeDirection = Direction.None;
            _ST[0].TradeTrigger = Trigger.None;

           
        }

        private void Triggers()
        {
            for (int x = 1; x < _ST.Count; x++)
                if (_ST[x].TradeTrigger == Trigger.TakeProfit || _ST[x].TradeTrigger == Trigger.StopLoss ||
                    _ST[x].TradeTrigger == Trigger.EndOfDayCloseLong || _ST[x].TradeTrigger == Trigger.EndOfDayCloseShort ||
                    _ST[x].TradeTrigger == Trigger.ContractExpires
                    ) ;

                else
                {
                    _ST[x].TradeTrigger = Trigger.None;
                    _Trigger(_ST, x);
                }
        }

        private static void PositionAndDirection()
        {
            for (int x = 1; x < _ST.Count; x++)
            {

                if (_ST[x].TradeTrigger != Trigger.None)

                    ApplyTradeLogic(_ST, x);
                else
                {
                    _ST[x].Position = _ST[x - 1].Position;
                    _ST[x].TradeDirection = _ST[x - 1].TradeDirection;
                }

                if (_ST[x].TradeTrigger == Trigger.TakeProfit || _ST[x].TradeTrigger == Trigger.StopLoss ||
                    _ST[x].TradeTrigger == Trigger.EndOfDayCloseLong || _ST[x].TradeTrigger == Trigger.EndOfDayCloseShort ||
                    _ST[x].TradeTrigger == Trigger.ContractExpires
                    )
                {
                    _ST[x].TradeDirection = Direction.None;
                    _ST[x].Position = false;
                }

            }
        }

        private static void ApplyTradeLogic(List<TradeStrategy> ss, int x)
        {
            #region Currently Long or Short




            if (_ST[x - 1].TradeDirection == Direction.Long)
            {
                switch (_ST[x].TradeTrigger)
                {
                    case Trigger.CloseLong:
                        _ST[x].TradeDirection = Direction.None;
                        _ST[x].Position = false;
                        break;

                    case Trigger.CloseShort:
                        _ST[x].TradeDirection = Direction.None;
                        _ST[x].Position = false;
                        break;

                    default:
                        _ST[x].TradeDirection = Direction.Long;
                        _ST[x].Position = true;
                        break;
                }
                return;
            }

            if (_ST[x - 1].TradeDirection == Direction.Short)
            {
                switch (_ST[x].TradeTrigger)
                {
                    case Trigger.CloseLong:
                        _ST[x].TradeDirection = Direction.None;
                        _ST[x].Position = false;
                        break;

                    case Trigger.CloseShort:
                        _ST[x].TradeDirection = Direction.None;
                        _ST[x].Position = false;
                        break;

                    default:
                        _ST[x].TradeDirection = Direction.Short;
                        _ST[x].Position = true;
                        break;
                }
                return;
            }




            #endregion

            #region Currently None

            if (_ST[x - 1].TradeDirection == Direction.None)
            {
                switch (_ST[x].TradeTrigger)
                {
                    case Trigger.OpenLong:
                        _ST[x].TradeDirection = Direction.Long;
                        _ST[x].Position = true;
                        break;

                    case Trigger.OpenShort:
                        _ST[x].TradeDirection = Direction.Short;
                        _ST[x].Position = true;
                        break;

                    default:
                        _ST[x].TradeDirection = Direction.None;
                        _ST[x].Position = false;
                        break;
                }
                return;
            }

            #endregion
        }

        private static void TradeSignals()
        {
            for (int x = 1; x < _ST.Count; x++)
            {
                #region Closing Trades
                if (_ST[x - 1].Position && !_ST[x].Position && _ST[x - 1].TradeDirection == Direction.Long)
                {
                    switch (_ST[x].TradeTrigger)
                    {
                        case Trigger.CloseLong:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                        case Trigger.CloseShort:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                        case Trigger.StopLoss:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                        case Trigger.TakeProfit:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                        case Trigger.EndOfDayCloseLong:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                        case Trigger.ContractExpires:
                            _ST[x].ActualTrade = Trigger.CloseLong;
                            _ST[x].TradedPrice = _ST[x].Price_Close;
                            break;

                    }
                }
                else
                    if (_ST[x - 1].Position && !_ST[x].Position && _ST[x - 1].TradeDirection == Direction.Short)
                    {
                        switch (_ST[x].TradeTrigger)
                        {
                            case Trigger.CloseLong:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;

                            case Trigger.CloseShort:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;

                            case Trigger.StopLoss:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;

                            case Trigger.TakeProfit:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;

                            case Trigger.EndOfDayCloseShort:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;

                            case Trigger.ContractExpires:
                                _ST[x].ActualTrade = Trigger.CloseShort;
                                _ST[x].TradedPrice = _ST[x].Price_Close;
                                break;
                        }
                    }




                #endregion
                    else
                        #region Open Trades
                        if (!_ST[x - 1].Position && _ST[x].Position)
                        {
                            switch (_ST[x].TradeTrigger)
                            {
                                case Trigger.OpenLong:
                                    _ST[x].ActualTrade = Trigger.OpenLong;
                                    _ST[x].TradedPrice = _ST[x].Price_Close;
                                    break;

                                case Trigger.OpenShort:
                                    _ST[x].ActualTrade = Trigger.OpenShort;
                                    _ST[x].TradedPrice = _ST[x].Price_Close;
                                    break;
                            }
                        }
                        #endregion
                        else
                        #region Carry Over
                        {
                            if (_ST[x].Reason != TradeReason.StopLoss || _ST[x].Reason != TradeReason.TakeProfit ||
                                _ST[x].Reason != TradeReason.EndOfDayCloseLong || _ST[x].Reason != TradeReason.EndOfDayCloseShort ||
                                _ST[x].Reason != TradeReason.ContractExpires)
                            {

                                _ST[x].ActualTrade = Trigger.None;
                                _ST[x].TradedPrice = _ST[x - 1].TradedPrice;
                            }

                            else
                            {
                                _ST[x].ActualTrade = Trigger.None;
                                _ST[x].TradedPrice = _ST[x - 1].TradedPrice;
                            }
                        }
                        #endregion

            }
        }

        private static void CalcProfitLoss()
        {
            for (int x = 1; x < _ST.Count; x++)
            {
                if (_ST[x].Position & _ST[x].TradeDirection == Direction.Long)
                    _ST[x].RunningProfit = _ST[x].Price_Close - _ST[x].TradedPrice;

                if (_ST[x].Position & _ST[x].TradeDirection == Direction.Short)
                    _ST[x].RunningProfit = _ST[x].TradedPrice - _ST[x].Price_Close;


                if (_ST[x - 1].Position && !_ST[x].Position)
                {
                    if (_ST[x - 1].TradeDirection == Direction.Long)
                        _ST[x].RunningProfit = _ST[x].Price_Close - _ST[x - 1].TradedPrice;

                    if (_ST[x - 1].TradeDirection == Direction.Short)
                        _ST[x].RunningProfit = _ST[x - 1].TradedPrice - _ST[x].Price_Close;
                }


            }
        }

        private static void AddStopLossTriggers()
        {
            for (int x = 1; x < _ST.Count; x++)
            {
                if (_ST[x].RunningProfit < _p.StopLoss) _ST[x].Reason = TradeReason.StopLoss;
                if (_ST[x].RunningProfit > _p.TakeProfit) _ST[x].Reason = TradeReason.TakeProfit;
                // if (_ST[x].Timestamp.Hour == 17 && _ST[x].Timestamp.Minute == 20 && _ST[x].TradeDirection == Direction.Long) _ST[x].Reason = TradeReason.EndOfDayCloseLong;                
                //if (_ST[x].Timestamp.Hour == 17 && _ST[x].Timestamp.Minute == 20 && _ST[x].TradeDirection == Direction.Short) _ST[x].Reason = TradeReason.EndOfDayCloseShort;
                if (_ST[x - 1].InstrumentName != _ST[x].InstrumentName) _ST[x - 1].Reason = TradeReason.ContractExpires;
               
            }

        }

        private static void Mark()
        {
            #region Mark Object that is Stoploss or Takeprofit
            for (int x = 1; x < _ST.Count; x++)
            {
                if (_ST[x - 1].Reason != TradeReason.TakeProfit && _ST[x].Reason == TradeReason.TakeProfit)
                    _ST[x].markedObjectA = true;

                if (_ST[x - 1].Reason != TradeReason.StopLoss && _ST[x].Reason == TradeReason.StopLoss)
                    _ST[x].markedObjectA = true;

                if (_ST[x - 1].Reason != TradeReason.EndOfDayCloseLong && _ST[x].Reason == TradeReason.EndOfDayCloseLong)
                    _ST[x].markedObjectA = true;

                if (_ST[x - 1].Reason != TradeReason.EndOfDayCloseShort && _ST[x].Reason == TradeReason.EndOfDayCloseShort)
                    _ST[x].markedObjectA = true;

                if (_ST[x - 1].Reason != TradeReason.ContractExpires && _ST[x].Reason == TradeReason.ContractExpires)
                    _ST[x].markedObjectA = true;

                if ((_ST[x - 1].Position && _ST[x].Position) && _ST[x - 1].markedObjectA) _ST[x].markedObjectA = true;
                if (!_ST[x - 1].markedObjectA && _ST[x].markedObjectA) _ST[x].markedObjectB = true;
                
             
            }
            #endregion
        }

        private static void ClearProf()
        {


            #region Clear
            for (int x = 1; x < _ST.Count; x++)
            {
                _ST[x].markedObjectA = false;
                _ST[x].RunningProfit = 0;
                _ST[x].Position = false;
                _ST[x].TradeDirection = Direction.None;
                _ST[x].ActualTrade = Trigger.None;
                _ST[x].TradedPrice = 0;
                _ST[x].TradeTrigger = Trigger.None;


                if (_ST[x].markedObjectB)
                {
                    if (_ST[x].Reason == TradeReason.TakeProfit) _ST[x].TradeTrigger = Trigger.TakeProfit;
                    if (_ST[x].Reason == TradeReason.StopLoss) _ST[x].TradeTrigger = Trigger.StopLoss;
                    if (_ST[x].Reason == TradeReason.EndOfDayCloseLong) _ST[x].TradeTrigger = Trigger.EndOfDayCloseLong;
                    if (_ST[x].Reason == TradeReason.EndOfDayCloseShort) _ST[x].TradeTrigger = Trigger.EndOfDayCloseShort;
                    if (_ST[x].Reason == TradeReason.ContractExpires) _ST[x].TradeTrigger = Trigger.ContractExpires;
                }
                _ST[x].Reason = 0;
            }
            #endregion

        }

        private static void Stats()
        {
            


            double totalProfit = 0;
            double tradeCount =0;
            for (int x = 1; x < _ST.Count; x++)
            {

                if (_ST[x].ActualTrade == Trigger.CloseLong || _ST[x].ActualTrade == Trigger.CloseShort ||
                    _ST[x].ActualTrade == Trigger.StopLoss || _ST[x].ActualTrade == Trigger.TakeProfit ||
                    _ST[x].ActualTrade == Trigger.EndOfDayCloseLong || _ST[x].ActualTrade == Trigger.EndOfDayCloseShort
                    )
                {
                    totalProfit += _ST[x].RunningProfit;
                    tradeCount++;



                    _ST[x].TotalProfit = totalProfit;
                    _ST[x].TradeCount = tradeCount;
                }
            }
            Debug.WriteLine("============STATS====");
            Debug.WriteLine(totalProfit);
            Debug.WriteLine(tradeCount);
        }



        public bool Position { get; set; }
        public Direction TradeDirection { get; set; }
        public Trigger TradeTrigger { get; set; }
        public Trigger ActualTrade { get; set; }
        public TradeReason Reason { get; set; }
        public double RunningProfit { get; set; }
        public string InstrumentName { get; set; }
       
        public double TradedPrice { get; set; }
        public bool markedObjectA { get; set; }
        public bool markedObjectB { get; set; }

        public double TotalProfit { get; set; }
        public double TradeCount { get; set; }


        public enum TradeReason
        {
            Normal = 1,
            StopLoss = 6,
            TakeProfit = 7,
            EndOfDayCloseLong = 8,
            EndOfDayCloseShort = 9,
            ContractExpires = 10
        }

        public enum Direction
        {
            Long = 1,
            Short = 2,
            None = 3,
        }

        public enum Trigger
        {
            OpenLong = 1,
            CloseLong = 2,
            OpenShort = 3,
            CloseShort = 4,
            None = 5,
            StopLoss = 6,
            TakeProfit = 7,
            EndOfDayCloseLong = 8,
            EndOfDayCloseShort = 9,
            ContractExpires = 10
        }

    }


}
