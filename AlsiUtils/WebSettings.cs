﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiUtils
{
    public class WebSettings
    {
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
                }
            }

            private static void SetTradeMode(TradeMode Mode)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "TRADE_MODE").First();
                if (Mode == TradeMode.Normal) setting.ValueString = "NORMAL";
                if (Mode == TradeMode.Hit) setting.ValueString = "HIT";
                if (Mode == TradeMode.Aggressive) setting.ValueString = "AGGRESSIVE";
                dc.SubmitChanges();
            }

            private static TradeMode GetTradeMode()
            {
                var dc = new WebDbDataContext();
                var Mode = dc.Settings.Where(z => z.Setting_Name == "TRADE_MODE").First().ValueString;
                if (Mode == "HIT") return TradeMode.Hit;
                if (Mode == "AGGRESSIVE") return TradeMode.Aggressive;
                return TradeMode.Normal;
            }

            private static double GetSpread()
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "TRADE_MODE").First();
                return (double)setting.ValueNumber;
            }

            private static void SetSpread(double spread)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "TRADE_MODE").First();
                setting.ValueNumber = (int)spread;
                dc.SubmitChanges();
            }



            public enum TradeMode
            {
                Normal = 1,
                Hit = 2,
                Aggressive = 3,
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
                }
            }

            private static string GetHiSat_Inst()
            {
                var dc = new WebDbDataContext();
                return dc.Settings.Where(z => z.Setting_Name == "HISAT_INST").First().ValueString;
            }

            private static void SetHiSat_Inst(string Instrument)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "HISAT_INST").First();
                setting.ValueString = Instrument;
                dc.SubmitChanges();
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
                }
            }

            private static string GetOTS_Inst()
            {
                var dc = new WebDbDataContext();
                return dc.Settings.Where(z => z.Setting_Name == "OTS_INST").First().ValueString;
            }

            private static void SetOTS_Inst(string Instrument)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "OTS_INST").First();
                setting.ValueString = Instrument;
                dc.SubmitChanges();
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
                }
            }

            private static int GetVOL()
            {
                var dc = new WebDbDataContext();
                return (int)dc.Settings.Where(z => z.Setting_Name == "VOL").First().ValueNumber;
            }

            private static void SetVOL(int vol)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "VOL").First();
                setting.ValueNumber = vol;
                dc.SubmitChanges();
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
                }
            }

            private static int GetSTOPLOSS()
            {
                var dc = new WebDbDataContext();
                return (int)dc.Settings.Where(z => z.Setting_Name == "STOPLOSS").First().ValueNumber;
            }

            private static void SetSTOPLOSS(int STOPLOSS)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "STOPLOSS").First();
                setting.ValueNumber = STOPLOSS;
                dc.SubmitChanges();
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
                }
            }

            private static int GetTAKE_PROFIT()
            {
                var dc = new WebDbDataContext();
                return (int)dc.Settings.Where(z => z.Setting_Name == "TAKE_PROFIT").First().ValueNumber;
            }

            private static void SetTAKE_PROFIT(int TAKE_PROFIT)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "TAKE_PROFIT").First();
                setting.ValueNumber = TAKE_PROFIT;
                dc.SubmitChanges();
            }





            #endregion

            #region LIVE_START_DATE

            private static DateTime _LIVE_START_DATE;
            public static DateTime  LIVE_START_DATE
            {
                get
                {
                    if (_LIVE_START_DATE.Year<1985) _LIVE_START_DATE = GetLIVE_START_DATE();
                    return _LIVE_START_DATE;
                }
                set
                {
                    _LIVE_START_DATE = value;
                    SetLIVE_START_DATE(value);
                }
            }

            private static DateTime GetLIVE_START_DATE()
            {
                var dc = new WebDbDataContext();
                return (DateTime)dc.Settings.Where(z => z.Setting_Name == "LIVE_START_DATE").First().ValueDate;
            }

            private static void SetLIVE_START_DATE(DateTime  LIVE_START_DATE)
            {
                var dc = new WebDbDataContext();
                var setting = dc.Settings.Where(z => z.Setting_Name == "LIVE_START_DATE").First();
                setting.ValueDate = LIVE_START_DATE;
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
                    }
                }

                private static int GetA1()
                {
                    var dc = new WebDbDataContext();
                    return (int)dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().A1;
                }

                private static void SetA1(int A1)
                {
                    var dc = new WebDbDataContext();
                    var setting = dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.A1= A1;
                    dc.SubmitChanges();
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
                    }
                }

                private static int GetA2()
                {
                    var dc = new WebDbDataContext();
                    return (int)dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().A2;
                }

                private static void SetA2(int A2)
                {
                    var dc = new WebDbDataContext();
                    var setting = dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.A2 = A2;
                    dc.SubmitChanges();
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
                    }
                }

                private static int GetB1()
                {
                    var dc = new WebDbDataContext();
                    return (int)dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().B1;
                }

                private static void SetB1(int B1)
                {
                    var dc = new WebDbDataContext();
                    var setting = dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.B1 = B1;
                    dc.SubmitChanges();
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
                    }
                }

                private static int GetB2()
                {
                    var dc = new WebDbDataContext();
                    return (int)dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().B2;
                }

                private static void SetB2(int B2)
                {
                    var dc = new WebDbDataContext();
                    var setting = dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.B2 = B2;
                    dc.SubmitChanges();
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
                    }
                }

                private static int GetC1()
                {
                    var dc = new WebDbDataContext();
                    return (int)dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First().C1;
                }

                private static void SetC1(int C1)
                {
                    var dc = new WebDbDataContext();
                    var setting = dc.IndicatorSettings.Where(z => z.Name == "EMA_SCALP").First();
                    setting.C1 = C1;
                    dc.SubmitChanges();
                }





                #endregion
            }
        }
    }
}
