using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlsiUtils
{
    public class Strategy_Live
    {
        private class RSI_Temp_80_20 : Rsi
        {
            public bool Touched_80 { get; set; }

            public bool Touched_20 { get; set; }

            public bool Reset { get; set; }

            public double minPrice { get; set; }

            public double maxPrice { get; set; }

            public double RunningProfit { get; set; }
        }

        private class MACD_Temp : Macd
        {
            public bool Reset { get; set; }

            public bool CrossFromAbove { get; set; }

            public bool CrossFromBelow { get; set; }
        }

        private class Slow_Stoch_POP : SlowStoch
        {
             public bool Position{ get; set; }
             public double TradedPrice { get; set; }
            public string CurrentPositionDirection { get; set; }            

            public bool CrossFromAbove { get; set; }

            public bool CrossFromBelow { get; set; }

            public double RunningProfit { get; set; }
        }

        #region Stochastic POP

        public static void StochasticPOP_TRIX_STOP_Min_D(Strategies.Parameter_SlowStoch Parameters, List<Price> price, out List<Trade> trades, bool ReverseTrades, out List<SummaryStats> Stats, bool LiveTrade)
        {

            int Fast_K = Parameters.Fast_K;
            int Slow_K = Parameters.Slow_K;
            int Slow_D = Parameters.Slow_D;
            double POP_80_Open = (double)Parameters.Open_80;
            double POP_20_Open = (double)Parameters.Open_20;
            double POP_80_Close = (double)Parameters.Close_80;
            double POP_20_Close = (double)Parameters.Close_20;
            int Volume = Parameters.Volume;
            int TakeProfit = Parameters.TakeProfit;
            int StopLoss = Parameters.StopLoss;

            List<SlowStoch> SlowStochastic = Factory_Indicator.createSlowStochastic(Fast_K, Slow_K, Slow_D, price);
            // List<Trix> Trix = Factory_Indicator.createTRIX(Trix_N, Trix_M, price);
            List<Slow_Stoch_POP> SSpopList = new List<Slow_Stoch_POP>();
            List<Trade> TRADE = new List<Trade>();





            int index = SlowStochastic.Count;
            double tradeCount = 0;
            double lossCount = 0;
            double profitCount = 0;
            double tradeProfit = 0;
            double tradeLoss = 0;
            double totalPofit_Loss = 0;
            double maxDrawBack = 0;
            double maxProfits = 0;
            double biggestWin = 0;
            double biggestLoss = 0;
            double periodLoss = 0;
            double periodWin = 0;
            double LongProf = 0;
            double LongLoss = 0;
            double ShortProf = 0;
            double ShortLoss = 0;
            int LongTrades = 0;
            int ShortTrades = 0;
            int StoppedOut = 0;


            Slow_Stoch_POP ssPOP0 = new Slow_Stoch_POP();
            ssPOP0.Price_Close = SlowStochastic[0].Price_Close;
            ssPOP0.Timestamp = SlowStochastic[0].Timestamp;
            ssPOP0.K = SlowStochastic[0].K;
            ssPOP0.D = SlowStochastic[0].D;
            ssPOP0.InstrumentName = SlowStochastic[0].InstrumentName;
            SSpopList.Add(ssPOP0);


            Trade trade0 = new Trade();
            trade0.TimeStamp = SlowStochastic[0].Timestamp;
            trade0.Position = false;
            trade0.RunningProfit = 0;
            trade0.TotalPL = 0;
            TRADE.Add(trade0);


            for (int x = 1; x < index; x++)
            {
                bool tradeAction = false; // if there were any action this will be true..to filter.

                Slow_Stoch_POP ssPOP = new Slow_Stoch_POP();
                Trade t = new Trade();


                ssPOP.Price_Close = SlowStochastic[x].Price_Close;
                ssPOP.Timestamp = SlowStochastic[x].Timestamp;
                ssPOP.K = SlowStochastic[x].K;
                ssPOP.D = SlowStochastic[x].D;
                ssPOP.Position = SSpopList[x - 1].Position;
                ssPOP.TradedPrice = SSpopList[x - 1].TradedPrice;
                ssPOP.CurrentPositionDirection = SSpopList[x - 1].CurrentPositionDirection;
                ssPOP.InstrumentName = SlowStochastic[x].InstrumentName;

                t.CurrentDirection = TRADE[x - 1].CurrentDirection;
                t.TimeStamp = SlowStochastic[x].Timestamp;
                t.Position = SSpopList[x - 1].Position;
                t.TradedPrice = SSpopList[x - 1].TradedPrice;
                t.TradeReason = "No Trade";
                t.BuyorSell = "NONE";
                t.CurrentPrice = ssPOP.Price_Close;
                if (ssPOP.Position)
                {
                    if (ssPOP.CurrentPositionDirection == "Long") ssPOP.RunningProfit = (ssPOP.Price_Close - ssPOP.TradedPrice);
                    if (ssPOP.CurrentPositionDirection == "Short") ssPOP.RunningProfit = (ssPOP.TradedPrice - ssPOP.Price_Close);

                }

                //Debug.WriteLine(ssPOP.Timestamp + "  TRADED " + ssPOP.TradedPrice + "  Current " + ssPOP.Price_Close + "    Running Prof " + ssPOP.RunningProfit);

                //Verify If ContractName Has Changed
                bool isNewContract = false;
                if (SSpopList[x - 1].InstrumentName != ssPOP.InstrumentName) isNewContract = true;

                // Debug.WriteLine(ssPOP.Timestamp + " " + ssPOP.InstrumentName + "  " + SSpopList[x-1].InstrumentName);


                #region CROSS FROM ABOVE & BELOW
                if (SlowStochastic[x].K > SlowStochastic[x].D && SlowStochastic[x - 1].D > SlowStochastic[x - 1].K)
                {
                    ssPOP.CrossFromBelow = true;

                    //Debug.WriteLine("Cross from Below : " + ssPOP.Timestamp + "\t" + ssPOP.K + "\t" + ssPOP.D + "\t" + ssPOP.Price_Close + "\t" + ssPOP.Position);

                }

                if (SlowStochastic[x].K < SlowStochastic[x].D && SlowStochastic[x - 1].D < SlowStochastic[x - 1].K)
                {

                    ssPOP.CrossFromAbove = true;
                    //Debug.WriteLine("Cross from Above : " + ssPOP.Timestamp + "\t" + ssPOP.K + "\t" + ssPOP.D + "\t" + ssPOP.Price_Close + "\t" + ssPOP.Position);

                }

                #endregion

                #region Close if ContractName Changes


                if (ssPOP.Position && isNewContract)
                {
                    // Debug.WriteLine("T R I G G E R " + isNewContract);
                    StoppedOut++;
                    t.Notes = "Contract Expire";
                    if (ssPOP.CurrentPositionDirection == "Long") // Only Close Long Trades
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        //ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (SSpopList[x - 2].Price_Close - SSpopList[x - 1].TradedPrice);
                        //Debug.WriteLine("CLOSE LONG (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//
                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = SSpopList[x - 1].InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Short #";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Long #";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////


                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            LongProf += profit;

                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            LongLoss += profit;
                        }
                    }
                    else //Close Short
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        //ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (SSpopList[x - 1].TradedPrice - (SSpopList[x - 2].Price_Close));
                        //Debug.WriteLine("## CLOSE SHORT (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//
                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = SSpopList[x - 1].InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Long #";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Short #";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////


                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            ShortProf += profit;
                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            ShortLoss += profit;
                        }


                    }
                }




                #endregion

                #region CLOSE TRADE

                #region close if crossover - OPTONAL
                //if (ssPOP.CrossFromAbove || ssPOP.CrossFromBelow)
                //{
                //  if (ssPOP.Position)
                //  {
                //    ssPOP.Position = false;
                //    if (ssPOP.CurrentPositionDirection == "Long")
                //    {
                //      double profit = (ssPOP.Price_Close - SSpopList[x - 1].TradedPrice);
                //      //Debug.WriteLine("CLOSE LONG : " + ssPOP.Timestamp + "\t Traded at:" + ssPOP.TradedPrice + "\t Last Price:" + ssPOP.Price_Close + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                //      //Debug.WriteLine("Profit : " + profit);
                //      tradeCount++;
                //      totalPofit_Loss += profit;
                //      if (profit > 0)
                //      {
                //        profitCount++;
                //        tradeProfit += profit;
                //      }
                //      else
                //      {
                //        lossCount++;
                //        tradeLoss += profit;
                //      }


                //    }
                //    if (ssPOP.CurrentPositionDirection == "Short")
                //    {
                //      double profit = (SSpopList[x - 1].TradedPrice - ssPOP.Price_Close);
                //      //Debug.WriteLine("CLOSE SHORT : " + ssPOP.Timestamp + "\t Traded at:" + ssPOP.TradedPrice + "\t Last Price:" + ssPOP.Price_Close + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                //      //Debug.WriteLine("Profit : " + profit);
                //      tradeCount++;
                //      totalPofit_Loss += profit;
                //      if (profit > 0)
                //      {
                //        profitCount++;
                //        tradeProfit += profit;
                //      }
                //      else
                //      {
                //        lossCount++;
                //        tradeLoss += profit;
                //      }
                //    }

                //  }
                //}

                #endregion

                #region Close at End Of Day - OPTIONAL

                //if (ssPOP.Position)
                //{


                //    if (ssPOP.Timestamp.Hour == 17)
                //    {
                //        ssPOP.Position = false;
                //        ssPOP.TradedPrice = ssPOP.Price_Close;
                //        double profit = 0;
                //        if (ssPOP.CurrentPositionDirection == "Long") profit = (ssPOP.Price_Close - SSpopList[x - 1].TradedPrice);
                //        if (ssPOP.CurrentPositionDirection == "Short") profit = (SSpopList[x - 1].TradedPrice - ssPOP.Price_Close);
                //        //Debug.WriteLine("CLOSE END OF DAY : " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                //        //Debug.WriteLine("Profit : " + profit);
                //        tradeCount++;
                //        totalPofit_Loss += profit;
                //        if (profit > 0)
                //        {
                //            profitCount++;
                //            tradeProfit += profit;
                //        }
                //        else
                //        {
                //            lossCount++;
                //            tradeLoss += profit;
                //        }

                //    }

                //}





                #endregion

                #region STOP lOSS & TAKE PROFIT
                if (ssPOP.Position)
                {
                    if (ssPOP.RunningProfit < StopLoss || ssPOP.RunningProfit > TakeProfit)
                    {
                        StoppedOut++;
                        t.Notes = "Stopped Out";
                        if (ssPOP.CurrentPositionDirection == "Long") // Only Close Long Trades
                        {
                            tradeAction = true;
                            ssPOP.Position = false;
                            ssPOP.TradedPrice = ssPOP.Price_Close;
                            double profit = (ssPOP.Price_Close - SSpopList[x - 1].TradedPrice);
                            //Debug.WriteLine("CLOSE LONG (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                            //Debug.WriteLine("Profit : " + profit);

                            if (biggestLoss > profit) biggestLoss = profit;
                            if (biggestWin < profit) biggestWin = profit;

                            //populate Trade Object//
                            t.Position = false;
                            t.TimeStamp = ssPOP.Timestamp;
                            t.TradedPrice = ssPOP.Price_Close;
                            t.CurrentDirection = "None";
                            t.InstrumentName = ssPOP.InstrumentName;

                            if (ReverseTrades)
                            {
                                t.TradeReason = "Close Short #";
                                t.TradeVolume = Volume * 1;
                                t.BuyorSell = "Buy";
                                t.RunningProfit = profit * -1;
                            }
                            else
                            {
                                t.TradeReason = "Close Long #";
                                t.TradeVolume = Volume * -1;
                                t.BuyorSell = "Sell";
                                t.RunningProfit = profit;
                            }


                            ///////////////////////////////////


                            tradeCount++;
                            totalPofit_Loss += profit;
                            if (profit > 0)
                            {
                                profitCount++;
                                tradeProfit += profit;
                                LongProf += profit;

                            }
                            else
                            {
                                lossCount++;
                                tradeLoss += profit;
                                LongLoss += profit;
                            }
                        }
                        else //Close Short
                        {
                            tradeAction = true;
                            ssPOP.Position = false;
                            ssPOP.TradedPrice = ssPOP.Price_Close;
                            double profit = (SSpopList[x - 1].TradedPrice - ssPOP.Price_Close);
                            //Debug.WriteLine("## CLOSE SHORT (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                            //Debug.WriteLine("Profit : " + profit);

                            if (biggestLoss > profit) biggestLoss = profit;
                            if (biggestWin < profit) biggestWin = profit;

                            //populate Trade Object//
                            t.Position = false;
                            t.TimeStamp = ssPOP.Timestamp;
                            t.TradedPrice = ssPOP.Price_Close;
                            t.CurrentDirection = "None";
                            t.InstrumentName = ssPOP.InstrumentName;

                            if (ReverseTrades)
                            {
                                t.TradeReason = "Close Long #";
                                t.TradeVolume = Volume * -1;
                                t.BuyorSell = "Sell";
                                t.RunningProfit = profit * -1;
                            }
                            else
                            {
                                t.TradeReason = "Close Short #";
                                t.TradeVolume = Volume * 1;
                                t.BuyorSell = "Buy";
                                t.RunningProfit = profit;
                            }


                            ///////////////////////////////////


                            tradeCount++;
                            totalPofit_Loss += profit;
                            if (profit > 0)
                            {
                                profitCount++;
                                tradeProfit += profit;
                                ShortProf += profit;
                            }
                            else
                            {
                                lossCount++;
                                tradeLoss += profit;
                                ShortLoss += profit;
                            }


                        }
                    }
                }

                #endregion

                #region TRIX Close

                #endregion

                #region Normal Close
                //close if D (Slower) POP BELOW 80
                if (SlowStochastic[x].D < POP_80_Close && SlowStochastic[x - 1].D > POP_80_Close && ssPOP.Position)
                {
                    t.Notes = "Normal Trade";
                    if (ssPOP.CurrentPositionDirection == "Long") // Only Close Long Trades
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (ssPOP.Price_Close - SSpopList[x - 1].TradedPrice);
                        //Debug.WriteLine("CLOSE LONG (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//
                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = ssPOP.InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Short";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Long";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////


                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            LongProf += profit;

                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            LongLoss += profit;
                        }
                    }
                    else //Close Short
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (SSpopList[x - 1].TradedPrice - ssPOP.Price_Close);
                        //Debug.WriteLine("## CLOSE SHORT (curdir=Long): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//
                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = ssPOP.InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Long";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Short";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////


                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            ShortProf += profit;
                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            ShortLoss += profit;
                        }

                    }
                }

                if (SlowStochastic[x].D > POP_20_Close && SlowStochastic[x - 1].D < POP_20_Close && ssPOP.Position)
                {
                    t.Notes = "Normal Trade";
                    if (ssPOP.CurrentPositionDirection == "Short") // Only Close Short Trades
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (SSpopList[x - 1].TradedPrice - ssPOP.Price_Close);
                        //Debug.WriteLine("CLOSE SHORT (curdir=Short): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//
                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = ssPOP.InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Long";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Short";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////
                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            ShortProf += profit;
                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            ShortLoss += profit;
                        }
                    }
                    else //Close Long
                    {
                        tradeAction = true;
                        ssPOP.Position = false;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        double profit = (ssPOP.Price_Close - SSpopList[x - 1].TradedPrice);
                        //Debug.WriteLine("## CLOSE LONG (curdir=Short): " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //Debug.WriteLine("Profit : " + profit);

                        if (biggestLoss > profit) biggestLoss = profit;
                        if (biggestWin < profit) biggestWin = profit;

                        //populate Trade Object//

                        t.Position = false;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.CurrentDirection = "None";
                        t.InstrumentName = ssPOP.InstrumentName;

                        if (ReverseTrades)
                        {
                            t.TradeReason = "Close Short";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.RunningProfit = profit * -1;
                        }
                        else
                        {
                            t.TradeReason = "Close Long";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.RunningProfit = profit;
                        }


                        ///////////////////////////////////
                        tradeCount++;
                        totalPofit_Loss += profit;
                        if (profit > 0)
                        {
                            profitCount++;
                            tradeProfit += profit;
                            LongProf += profit;
                        }
                        else
                        {
                            lossCount++;
                            tradeLoss += profit;
                            LongLoss += profit;
                        }
                    }
                }
                #endregion



                #endregion

                #region OPEN TRADE
                if (!SSpopList[x - 1].Position)
                {
                    // D (Slower) POP ABOVE 80  - LONG
                    if (SlowStochastic[x].D > POP_80_Open && SlowStochastic[x - 1].D < POP_80_Open && ssPOP.Position == false && t.Notes != "Stopped Out")
                    {
                        t.Notes = "Normal Trade";
                        tradeAction = true;
                        ssPOP.Position = true;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        ssPOP.CurrentPositionDirection = "Long";
                        //Debug.WriteLine("TRADE LONG : " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //populate Trade Object//

                        t.Position = true;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.InstrumentName = ssPOP.InstrumentName;


                        if (ReverseTrades)
                        {
                            t.TradeReason = "Open Short";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.CurrentDirection = "Short";
                            ShortTrades++;
                        }
                        else
                        {
                            t.TradeReason = "Open Long";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.CurrentDirection = "Long";
                            LongTrades++;
                        }


                        ///////////////////////////////////
                    }
                }

                {
                    // D (Slower) POP ABOVE 80  - LONG
                    if (SlowStochastic[x].D < POP_20_Open && SlowStochastic[x - 1].D > POP_20_Open && ssPOP.Position == false && t.Notes != "Stopped Out")
                    {
                        t.Notes = "Normal Trade";
                        tradeAction = true;
                        ssPOP.Position = true;
                        ssPOP.TradedPrice = ssPOP.Price_Close;
                        ssPOP.CurrentPositionDirection = "Short";
                        //Debug.WriteLine("TRADE SHORT : " + ssPOP.Timestamp + "\t" + ssPOP.TradedPrice + "\t D " + ssPOP.D + "\t K " + ssPOP.K + "\t" + ssPOP.Position);
                        //populate Trade Object//

                        t.Position = true;
                        t.TimeStamp = ssPOP.Timestamp;
                        t.TradedPrice = ssPOP.Price_Close;
                        t.InstrumentName = ssPOP.InstrumentName;


                        if (ReverseTrades)
                        {
                            t.TradeReason = "Open Long";
                            t.TradeVolume = Volume * 1;
                            t.BuyorSell = "Buy";
                            t.CurrentDirection = "Long";
                            LongTrades++;
                        }
                        else
                        {
                            t.TradeReason = "Open Short";
                            t.TradeVolume = Volume * -1;
                            t.BuyorSell = "Sell";
                            t.CurrentDirection = "Short";
                            ShortTrades++;
                        }


                        ///////////////////////////////////
                    }
                }
                #endregion

                if (maxDrawBack > totalPofit_Loss) maxDrawBack = totalPofit_Loss;
                if (maxProfits < totalPofit_Loss) maxProfits = totalPofit_Loss;

                if (totalPofit_Loss < 0) periodLoss++;
                if (totalPofit_Loss > 0) periodWin++;

                if (ReverseTrades)
                {
                    t.TotalPL = totalPofit_Loss * -1;
                    t.RunningProfit = ssPOP.RunningProfit * -1;

                }
                else
                {
                    t.TotalPL = totalPofit_Loss;
                    t.RunningProfit = ssPOP.RunningProfit;

                }

                t.IndicatorNotes = "D " + Math.Round(ssPOP.D, 2).ToString();
                TRADE.Add(t);
                SSpopList.Add(ssPOP);
                //for each index loop
            }

            #region Summary Stats
            //Stats Summary
            double avg_Profit = Math.Round(tradeProfit / profitCount, 2);
            double avg_Loss = Math.Round(tradeLoss / lossCount, 2);
            double avg_Tot_PL = Math.Round(totalPofit_Loss / tradeCount, 2);
            double pct_Profit = Math.Round(profitCount / tradeCount, 2) * 100;
            double pct_Loss = Math.Round(lossCount / tradeCount, 2) * 100;
            double avg_Long_Prof = Math.Round(LongProf / LongTrades, 2);
            double avg_Long_Loss = Math.Round(LongLoss / LongTrades, 2);
            double avg_Short_Prof = Math.Round(ShortProf / ShortTrades, 2);
            double avg_Short_Loss = Math.Round(ShortLoss / ShortTrades, 2);
            double long_prof_loss_Relation = Math.Round(avg_Long_Prof / avg_Long_Loss, 2) * -1;
            double short_prof_loss_Relation = Math.Round(avg_Short_Prof / avg_Short_Loss, 2) * -1;

            Debug.WriteLine("=================================================");
            Debug.WriteLine("FAST K " + Fast_K + "\t SLOW K " + Slow_K + "\t SLOW D " + Slow_D);
            Debug.WriteLine("OPEN " + POP_80_Open + "\t " + POP_20_Open);
            Debug.WriteLine("CLOSE " + POP_80_Close + "\t " + POP_20_Close);
            Debug.WriteLine("Trades : " + tradeCount + "\t AVG : " + avg_Tot_PL);
            Debug.WriteLine("Profit : " + pct_Profit + " %");
            Debug.WriteLine("Loss   : " + pct_Loss + " %");
            Debug.WriteLine("AVG Loss : " + avg_Loss);
            Debug.WriteLine("AVG Profit : " + avg_Profit);
            Debug.WriteLine("TOTAL PROFIT : " + totalPofit_Loss);
            Debug.WriteLine("MAX DRAWBACK : " + maxDrawBack);
            Debug.WriteLine("MAX PROFITS : " + maxProfits);
            Debug.WriteLine("MAX SINGLE WIN : " + biggestWin);
            Debug.WriteLine("MAX SINGLE LOSS : " + biggestLoss);
            Debug.WriteLine("PERIOD IN LOSS : " + periodLoss);
            Debug.WriteLine("PERIOD IN WIN : " + periodWin);
            Debug.WriteLine("Number Of Times Stopped : " + StoppedOut + "  " + Math.Round(StoppedOut / tradeCount, 4) * 100 + "%");
            Debug.WriteLine("====Long Short Ananysis===");
            Debug.WriteLine("Number of Long Trades : " + LongTrades);
            Debug.WriteLine("Number of Short Trades : " + ShortTrades);
            Debug.WriteLine("Total Long Profit : " + LongProf);
            Debug.WriteLine("Total Short Profit : " + ShortProf);
            Debug.WriteLine("Total Long Loss : " + LongLoss);
            Debug.WriteLine("Total Short Loss : " + ShortLoss);
            Debug.WriteLine("AVG Long Profit : " + avg_Long_Prof);
            Debug.WriteLine("AVG Short Profit : " + avg_Short_Prof);
            Debug.WriteLine("AVG Long Loss : " + avg_Long_Loss);
            Debug.WriteLine("AVG Short Loss : " + avg_Short_Loss);
            Debug.WriteLine("Long Profit/Loss Ratio : " + long_prof_loss_Relation);
            Debug.WriteLine("Short Profit/Loss Ratio : " + short_prof_loss_Relation);


            #region Summary Stats Object Output
            List<SummaryStats> StatsList = new List<SummaryStats>();

            StatsList.Add(Calc_Trading.createSummaryStatistic("Fast K", Fast_K.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Slow K", Slow_K.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Slow D", Slow_D.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Open 80", POP_80_Open.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Close 80", POP_80_Close.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Open 20", POP_20_Open.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Close 20", POP_20_Close.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("# Trades", tradeCount.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Total Average PL", avg_Tot_PL.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Trades Count", tradeCount.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Profit Trades", pct_Profit.ToString() + " %"));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Loss Trades", pct_Loss.ToString() + " %"));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Profit", avg_Profit.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Loss", avg_Loss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("TOTAL PROFIT", totalPofit_Loss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Max Profits", maxProfits.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Max Drawback", maxDrawBack.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Max Single Win", biggestWin.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Max Single Loss", biggestLoss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Period in Win", periodWin.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Period in Loss", periodLoss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Number of Times Stopped", StoppedOut.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("% of Times Stopped", (Math.Round(StoppedOut / tradeCount, 4) * 100).ToString() + "%"));
            StatsList.Add(Calc_Trading.createSummaryStatistic("# Long Trades", LongTrades.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("# Short Trades", ShortTrades.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Long Profit Trades", LongProf.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Short Profit Trades", ShortProf.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Total Long Loss", LongLoss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Total Short Loss", ShortLoss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Long Profit", avg_Long_Prof.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Short Profit", avg_Short_Prof.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Long Loss", avg_Long_Loss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Average Short Loss", avg_Short_Loss.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Long Profit/Loss Ratio", long_prof_loss_Relation.ToString()));
            StatsList.Add(Calc_Trading.createSummaryStatistic("Short Profit/Loss Ratio", short_prof_loss_Relation.ToString()));



            Stats = StatsList;
            //Debug.WriteLine("=================================================");
            #endregion



            #endregion

            #region DATABASE

            //try
            //{
            //  SimDBDataContext dc = new SimDBDataContext();

            //  SS_POP_10_Min db = new SS_POP_10_Min()
            //  {
            //    Slow_D = Slow_D,
            //    Slow_K = Slow_K,
            //    Fast_K = Fast_K,
            //    OPEN_80 = Convert.ToInt16(POP_80_Open),
            //    CLOSE_80 = Convert.ToInt16(POP_80_Close),
            //    OPEN_20 = Convert.ToInt16(POP_20_Open),
            //    CLOSE_20 = Convert.ToInt16(POP_20_Close),
            //    n_Trades = Convert.ToInt16(tradeCount),
            //    n_Profits = Convert.ToInt16(profitCount),
            //    n_Losses = Convert.ToInt16(lossCount),
            //    pct_Loss = Convert.ToDecimal(pct_Loss),
            //    pct_Profit = Convert.ToDecimal(pct_Profit),
            //    TotalPL = Convert.ToInt16(totalPofit_Loss),
            //    avg_Profit = Convert.ToDecimal(avg_Profit),
            //    avg_Loss = Convert.ToDecimal(avg_Loss),
            //    avg_TotalPL = Convert.ToDecimal(avg_Tot_PL)


            //  };

            //  dc.SS_POP_10_Mins.InsertOnSubmit(db);
            //  dc.SubmitChanges();
            //}
            //catch
            //{

            //}


            #endregion


            trades = TRADE;

        }



        #endregion


    }




}

