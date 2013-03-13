using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using AlsiUtils;
using AlsiUtils.Data_Objects;
using AlsiUtils.Strategies;

namespace AlsiTrade_Backend
{
    public class RunCalcs
    {

        public static List<Trade> RunEMAScalpLiveTrade(Parameter_EMA_Scalp Parameter, DateTime StartDate, GlobalObjects.TimeInterval Interval)
        {
            //  DateTime START = new DateTime(2012,06,02);//NB must be set periodically           
            DateTime e = DateTime.UtcNow.AddHours(5);
            UpdateDB.MergeTempWithHisto(Interval);
            GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(Interval, AlsiUtils.DataBase.dataTable.Temp, StartDate, e, false);
            var sp = GlobalObjects.Points.First();
            var l = GlobalObjects.Points.Last();
            return AlsiUtils.Strategies.EMA_Scalp.EmaScalp(Parameter, GlobalObjects.Points, false);

        }

        public static List<Trade> RunEMAScalp(Parameter_EMA_Scalp Parameter, GlobalObjects.TimeInterval Interval, bool TradesOnly, DateTime Start, DateTime End, DataBase.dataTable Table)
        {
            GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(Interval, Table, Start, End, false);
            return AlsiUtils.Strategies.EMA_Scalp.EmaScalp(Parameter, GlobalObjects.Points, TradesOnly);
        }

        public static List<Trade> RunEMASAR(Parameter_EMA_SAR Parameter, GlobalObjects.TimeInterval Interval, bool TradesOnly, DateTime Start, DateTime End, DataBase.dataTable Table)
        {
            GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(Interval, Table, Start, End, false);
            return AlsiUtils.Strategies.EMA_SAR.EmaSar(Parameter, GlobalObjects.Points, TradesOnly);

        }

    }
}
