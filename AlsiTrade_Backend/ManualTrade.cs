using System;
using System.Drawing;
using AlsiUtils;
namespace AlsiTrade_Backend
{
    public class ManualTrade
    {

       public Trade LastTrade;
       //public static bool PositionManuallyClosed;                           
        
        public static bool CanCloseTrade(Trade NewTrade)
        {
            
            if (!WebSettings.General.MANUAL_CLOSE_TRIGGER) return true;
            else
            {
                if (NewTrade.Reason == Trade.Trigger.CloseLong || NewTrade.Reason == Trade.Trigger.CloseShort)
                    return false;

                if (NewTrade.Reason == Trade.Trigger.OpenLong || NewTrade.Reason == Trade.Trigger.OpenShort)
                {
                    WebSettings.General.MANUAL_CLOSE_TRIGGER = false;
                    return true;
                }
            }
            return false;
        }

        public Trade GetCloseTrade()
        {

            Trade nT = new Trade();
            nT = LastTrade;
            nT.TimeStamp = DateTime.UtcNow.AddHours(2);
            nT.IndicatorNotes = "MANUAL CLOSE";
            nT.ForeColor = Color.Orange;
            nT.BackColor = Color.Black;
            
            if (LastTrade.Reason == Trade.Trigger.OpenLong)
            {
                nT.Reason = Trade.Trigger.CloseLong;
                nT.BuyorSell = Trade.BuySell.Sell;
            }
            if (LastTrade.Reason == Trade.Trigger.OpenShort)
            {
                nT.Reason = Trade.Trigger.CloseShort;
                nT.BuyorSell = Trade.BuySell.Buy;
            }

            return nT;

        }

       

       
    }


}
