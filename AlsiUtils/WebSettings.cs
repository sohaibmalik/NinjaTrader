using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    public class WebSettings
    {
        public static bool _settingChanged;
        private static System.Data.Linq.Table<tblSetting> Settings;
        private static System.Data.Linq.Table<tblIndicatorSetting> IndicatorSettings;
        public static AlsiWebDataContext DC = new AlsiWebDataContext();
        public static void GetSettings()
        {
            Settings = DC.tblSettings;
            IndicatorSettings = DC.tblIndicatorSettings;
        }

        public static void SaveSettings()
        {
            DC.SubmitChanges();
        }
        public class TradeApproach
        {
            private static TradeMode _Mode;
            public static TradeMode Mode
            {
                get
                {
                    if (_Mode == 0) _Mode = GetTradeMode();
                    return _Mode;
                }
                set
                {
                    _Mode = value;
                    SetTradeMode(value);
                    _settingChanged = true;
                }
            }

            private static double _Spread;
            public static double Spread
            {
                get
                {
                    if (_Spread == 0) _Spread = GetSpread();
                    return _Spread;
                }
                set
                {
                    _Spread = value;
                    SetSpread(value);
                    _settingChanged = true;
                }
            }

            private static void SetTradeMode(TradeMode Mode)
            {

                var setting = Settings.Where(z => z.SettingName == "TRADE_MODE").First();
                if (Mode == TradeMode.Normal) setting.ValueString = "NORMAL";
                if (Mode == TradeMode.Hit) setting.ValueString = "HIT";
                if (Mode == TradeMode.Aggressive) setting.ValueString = "AGGRESSIVE";
                if (Mode == TradeMode.BestBidOffer) setting.ValueString = "BEST";
                if (Mode == TradeMode.BestAgressive) setting.ValueString = "BESTAGGRESSIVE";

            }

            private static TradeMode GetTradeMode()
            {
                var Mode = Settings.Where(z => z.SettingName == "TRADE_MODE").First().ValueString;
                if (Mode == "HIT") return TradeMode.Hit;
                if (Mode == "AGGRESSIVE") return TradeMode.Aggressive;
                if (Mode == "BEST") return TradeMode.BestBidOffer;
                if (Mode == "BESTAGGRESSIVE") return TradeMode.BestAgressive;
                return TradeMode.Normal;
            }

            private static double GetSpread()
            {
                var setting = Settings.Where(z => z.SettingName == "TRADE_MODE").First();
                return (double)setting.ValueNumber;
            }

            private static void SetSpread(double spread)
            {
                var setting = Settings.Where(z => z.SettingName == "TRADE_MODE").First();
                setting.ValueNumber = (int)spread;
            }

            public static double AdjustPriceToStrategy(Trade trade,double bid,double offer)
            {
                double last = trade.CurrentPrice;             
                double spread = WebSettings.TradeApproach.Spread;

                if (trade.BuyorSell == Trade.BuySell.Buy)
                {
                    if (Mode == TradeMode.Hit) return offer;
                    if (Mode == TradeMode.Aggressive) return last + spread;
                    if (Mode == TradeMode.BestBidOffer) return bid + 1;
                    if (Mode == TradeMode.BestAgressive) return bid + spread;

                }


                if (trade.BuyorSell == Trade.BuySell.Sell)
                {
                    if (Mode == TradeMode.Hit) return bid;
                    if (Mode == TradeMode.Aggressive) return last - spread;
                    if (Mode == TradeMode.BestBidOffer) return offer- 1;
                    if (Mode == TradeMode.BestAgressive) return offer - spread;
                }

                return last;
            }

            /// <summary>
            /// Normal=Generated Price
            /// Hit=Take Bid or Offer
            /// Agressive = Normal +- Spread
            /// BestBidOffer=Bid+1 Offer-1
            /// BestAgressive=Bid+Spread Offer-Spread
            /// </summary>
            public enum TradeMode
            {
                Normal = 1,
                Hit = 2,
                Aggressive = 3,
                BestBidOffer=4,
                BestAgressive=5,
            }
        }

        public class General
        {
            #region HISAT_INST

            private static string _HISAT_INST;
            public static string HISAT_INST
            {
                get
                {
                    if (_HISAT_INST == null) _HISAT_INST = GetHiSat_Inst();
                    return _HISAT_INST;
                }
                set
                {
                    _HISAT_INST = value;
                    SetHiSat_Inst(value);
                    _settingChanged = true;
                }
            }

            private static string GetHiSat_Inst()
            {
                return Settings.Where(z => z.SettingName == "HISAT_INST").First().ValueString;
               
            }

            private static void SetHiSat_Inst(string Instrument)
            {
                var setting = Settings.Where(z => z.SettingName == "HISAT_INST").First();
                setting.ValueString = Instrument;
            }

            #endregion

            #region OTS_INST

            private static string _OTS_INST;
            public static string OTS_INST
            {
                get
                {
                    if (_OTS_INST == null) _OTS_INST = GetOTS_Inst();
                    return _OTS_INST;
                }
                set
                {
                    _OTS_INST = value;
                    SetOTS_Inst(value);
                    _settingChanged = true;
                }
            }

            private static string GetOTS_Inst()
            {
                return Settings.Where(z => z.SettingName == "OTS_INST").First().ValueString;
            }

            private static void SetOTS_Inst(string Instrument)
            {
                var setting = Settings.Where(z => z.SettingName == "OTS_INST").First();
                setting.ValueString = Instrument;
            }





            #endregion

            #region VOL

            private static int _VOL;
            public static int VOL
            {
                get
                {
                    if (_VOL == 0) _VOL = GetVOL();
                    return _VOL;
                }
                set
                {
                    _VOL = value;
                    SetVOL(value);
                    _settingChanged = true;
                }
            }

            private static int GetVOL()
            {

                return (int)Settings.Where(z => z.SettingName == "VOL").First().ValueNumber;
            }

            private static void SetVOL(int vol)
            {
                var setting = Settings.Where(z => z.SettingName == "VOL").First();
                setting.ValueNumber = vol;
            }





            #endregion

            #region STOPLOSS

            private static int _STOPLOSS;
            public static int STOPLOSS
            {
                get
                {
                    if (_STOPLOSS == 0) _STOPLOSS = GetSTOPLOSS();
                    return _STOPLOSS;
                }
                set
                {
                    _STOPLOSS = value;
                    SetSTOPLOSS(value);
                    _settingChanged = true;
                }
            }

            private static int GetSTOPLOSS()
            {
                return (int)Settings.Where(z => z.SettingName == "STOPLOSS").First().ValueNumber;
            }

            private static void SetSTOPLOSS(int STOPLOSS)
            {
                var setting = Settings.Where(z => z.SettingName == "STOPLOSS").First();
                setting.ValueNumber = STOPLOSS;
            }



            #endregion

            #region TAKE_PROFIT

            private static int _TAKE_PROFIT;
            public static int TAKE_PROFIT
            {
                get
                {
                    if (_TAKE_PROFIT == 0) _TAKE_PROFIT = GetTAKE_PROFIT();
                    return _TAKE_PROFIT;
                }
                set
                {
                    _TAKE_PROFIT = value;
                    SetTAKE_PROFIT(value);
                    _settingChanged = true;
                }
            }

            private static int GetTAKE_PROFIT()
            {
                return (int)Settings.Where(z => z.SettingName == "TAKE_PROFIT").First().ValueNumber;
            }

            private static void SetTAKE_PROFIT(int TAKE_PROFIT)
            {
                var setting = Settings.Where(z => z.SettingName == "TAKE_PROFIT").First();
                setting.ValueNumber = TAKE_PROFIT;
            }





            #endregion

            #region LIVE_START_DATE

            private static DateTime _LIVE_START_DATE;
            public static DateTime LIVE_START_DATE
            {
                get
                {
                    if (_LIVE_START_DATE.Year < 1985) _LIVE_START_DATE = GetLIVE_START_DATE();
                    return _LIVE_START_DATE;
                }
                set
                {
                    _LIVE_START_DATE = value;
                    SetLIVE_START_DATE(value);
                    _settingChanged = true;
                }
            }

            private static DateTime GetLIVE_START_DATE()
            {
                return (DateTime)Settings.Where(z => z.SettingName == "LIVE_START_DATE").First().ValueDate;
            }

            private static void SetLIVE_START_DATE(DateTime LIVE_START_DATE)
            {
                var setting = Settings.Where(z => z.SettingName == "LIVE_START_DATE").First();
                setting.ValueDate = LIVE_START_DATE;
            }





            #endregion

            #region MANUAL_CLOSE_TRIGGER

        
            private static bool _MANUAL_CLOSE_TRIGGER;
            public static bool MANUAL_CLOSE_TRIGGER
            {  
                get
                {
                     _MANUAL_CLOSE_TRIGGER = GetMANUAL_CLOSE_TRIGGER();
                    return _MANUAL_CLOSE_TRIGGER;
                }
                set
                {
                    _MANUAL_CLOSE_TRIGGER = value;
                    SetMANUAL_CLOSE_TRIGGER(value);
                    _settingChanged = true;
                  
                }
            }

            private static bool GetMANUAL_CLOSE_TRIGGER()
            {
                AlsiWebDataContext dc = new AlsiWebDataContext();
                return bool.Parse(dc.tblSettings.Where(z => z.SettingName == "MANUAL_CLOSE_TRIGGER").First().ValueString);
            }

            private static void SetMANUAL_CLOSE_TRIGGER(bool Triggered)
            {
                AlsiWebDataContext dc = new AlsiWebDataContext();
                var setting = dc.tblSettings.Where(z => z.SettingName == "MANUAL_CLOSE_TRIGGER").First();
                setting.ValueString = Triggered.ToString();
                dc.SubmitChanges();
            }

            #endregion

            #region SMS
            private static bool _ENABLE_SMS;
            public static bool ENABLE_SMS
            {
                get
                {
                    _ENABLE_SMS = GetENABLE_SMS();
                    return _ENABLE_SMS;
                }
                set
                {
                    _ENABLE_SMS = value;
                    Set_ENABLE_SMS(value);
                    _settingChanged = true;

                }
            }

            private static bool GetENABLE_SMS()
            {
                AlsiWebDataContext dc = new AlsiWebDataContext();
                return bool.Parse(dc.tblSettings.Where(z => z.SettingName == "ENABLE_SMS").First().ValueString);
            }

            private static void Set_ENABLE_SMS(bool Triggered)
            {
                AlsiWebDataContext dc = new AlsiWebDataContext();
                var setting = dc.tblSettings.Where(z => z.SettingName == "ENABLE_SMS").First();
                setting.ValueString = Triggered.ToString();
                dc.SubmitChanges();
            }
            #endregion
        }

        public class Indicators
        {
            public class EmaScalp
            {
                #region A1

                private static int _A1;
                public static int A1
                {
                    get
                    {
                        if (_A1 == 0) _A1 = GetA1();
                        return _A1;
                    }
                    set
                    {
                        _A1 = value;
                        SetA1(value);
                        _settingChanged = true;
                    }
                }

                private static int GetA1()
                {
                    return (int)IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().A1;
                }

                private static void SetA1(int A1)
                {
                    var setting = IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.A1 = A1;
                }






                #endregion

                #region A2

                private static int _A2;
                public static int A2
                {
                    get
                    {
                        if (_A2 == 0) _A2 = GetA2();
                        return _A2;
                    }
                    set
                    {
                        _A2 = value;
                        SetA2(value);
                        _settingChanged = true;
                    }
                }

                private static int GetA2()
                {
                    return (int)IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().A2;
                }

                private static void SetA2(int A2)
                {
                    var setting = IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.A2 = A2;
                }





                #endregion

                #region B1

                private static int _B1;
                public static int B1
                {
                    get
                    {
                        if (_B1 == 0) _B1 = GetB1();
                        return _B1;
                    }
                    set
                    {
                        _B1 = value;
                        SetB1(value);
                        _settingChanged = true;
                    }
                }

                private static int GetB1()
                {
                    return (int)IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().B1;
                }

                private static void SetB1(int B1)
                {
                    var setting = IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.B1 = B1;
                }





                #endregion

                #region B2

                private static int _B2;
                public static int B2
                {
                    get
                    {
                        if (_B2 == 0) _B2 = GetB2();
                        return _B2;
                    }
                    set
                    {
                        _B2 = value;
                        SetB2(value);
                        _settingChanged = true;
                    }
                }

                private static int GetB2()
                {
                    return (int)IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().B2;
                }

                private static void SetB2(int B2)
                {
                    var setting = IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.B2 = B2;
                }





                #endregion

                #region C1

                private static int _C1;
                public static int C1
                {
                    get
                    {
                        if (_C1 == 0) _C1 = GetC1();
                        return _C1;
                    }
                    set
                    {
                        _C1 = value;
                        SetC1(value);
                        _settingChanged = true;
                    }
                }

                private static int GetC1()
                {
                    return (int)IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().C1;
                }

                private static void SetC1(int C1)
                {
                    var setting = IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.C1 = C1;
                }





                #endregion
            }
        }
    }
}
