using System;
using System.Collections.Generic;
using System.Web.Services;
using ExcelLink;
namespace NotifierWebService
{
    /// <summary>
    /// Summary description for AlsiNotifyService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]


    public class AlsiNotifyService : System.Web.Services.WebService
    {

        [WebMethod]
        public void InsertNewOrder(xlTradeOrder Order)
        {
            
            TradeUpdate.Orders.Add(Order);
        }

        [WebMethod]
        public void InsertMessage(Boodskap message)
        {
            TradeUpdate.Messages.Add(message);
        }

        [WebMethod]
        public Boodskap getLastMessage()
        {
            if (TradeUpdate.Messages.Count == 0) return new Boodskap { Message = "None Found", TimeStamp = DateTime.Now };
            return TradeUpdate.Messages.FindLast(z => z.Message != null);
        }

        [WebMethod]
        public xlTradeOrder getLastOrder()
        {
            if (TradeUpdate.Orders.Count == 0) return null;
            return TradeUpdate.Orders.FindLast(z => z.Contract != null);
        }

        [WebMethod]
        public List<xlTradeOrder> getAllOrders()
        {
            return TradeUpdate.Orders;
        }

        [WebMethod]
        public void clearLists()
        {
            TradeUpdate.Orders.Clear();
            TradeUpdate.Messages.Clear();
        }





    }
}
