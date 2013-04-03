using System;
using System.Collections.Generic;
using System.Web.Services;
using ExcelLink;
using System.Diagnostics;
using System.Linq;
using Communicator;
using System.Text;

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
            Debug.WriteLine(message.TimeStamp + "  " + message.Message);
        }

        [WebMethod]
        public Boodskap getLastMessage()
        {
            if (TradeUpdate.Messages.Count == 0) return new Boodskap { Message = Boodskap.Messages.isDead, TimeStamp = DateTime.UtcNow.AddHours(2) };
            return TradeUpdate.Messages.FindLast(z => z.Message_Custom != null);
        }
        [WebMethod]
        public List<Boodskap> GetAllMessages()
        {
            var mesges = TradeUpdate.Messages;
            if (mesges == null)
                mesges.Add(new Boodskap() { Message = Boodskap.Messages.Startup, TimeStamp = DateTime.UtcNow.AddHours(2) });
            return mesges;
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

        [WebMethod]
        public void SendCommand(RemoteCommand.Command Command)
        {
            RemoteCommand.DoSomething = Command;
        }

        [WebMethod]
        public RemoteCommand.Command GetCommand()
        {
            RemoteCommand.Command CurrentCommand = RemoteCommand.DoSomething;
            RemoteCommand.DoSomething = RemoteCommand.Command.Idle;
            if (CurrentCommand != null)
                return CurrentCommand;
            else
                RemoteCommand.DoSomething = RemoteCommand.Command.Idle;
            CurrentCommand = RemoteCommand.DoSomething;
            return CurrentCommand;
        }


        [WebMethod]
        public void SendEmail(Communicator.EmailMsg Msg, bool AdminMail)
        {

        }

        [WebMethod]
        public void TriggerManualTrade(bool Triggered)
        {
            ManualTrade.PositionManuallyClosed = Triggered;
        }

        [WebMethod]
        public bool GetManualTradeTrigger()
        {
            return ManualTrade.PositionManuallyClosed;
        }

        [WebMethod]
        public void InsertChatMessage(Chat Message)
        {
            Chat.MesID++;
            Message.MessageID = Chat.MesID;
            Chat.ChatList.Add(Message);
        }

        [WebMethod]
        public List<Chat> GetChatMessages()
        {
            return Chat.ChatList;
        }

        [WebMethod]
        public string GetPrice()
        {
            return
                @"[
/* Jan 2013 */
[1357084800000,549.03],
[1357171200000,542.10],
[1357257600000,527.00],
[1357516800000,523.90],
[1357603200000,525.31],
[1357689600000,517.10],
[1357776000000,523.51],
[1357862400000,520.30],
[1358121600000,501.75],
[1358208000000,485.92],
[1358294400000,506.09],
[1358380800000,502.68],
[1358467200000,500.00],
[1358812800000,504.77],
[1358899200000,514.00],
[1358985600000,450.50],
[1359072000000,439.88],
[1359331200000,449.83],
[1359417600000,458.27],
[1359504000000,456.83],
[1359590400000,455.49],
/* Feb 2013 */
[1359676800000,453.62],
[1359936000000,442.32],
[1360022400000,457.84],
[1360108800000,457.35],
[1360195200000,468.22],
[1360281600000,474.98],
[1360540800000,479.93],
[1360627200000,467.90],
[1360713600000,467.01],
[1360800000000,466.59],
[1360886400000,460.16],
[1361232000000,459.99],
[1361318400000,448.85],
[1361404800000,446.06],
[1361491200000,450.81],
[1361750400000,442.80],
[1361836800000,448.97],
[1361923200000,444.57],
[1362009600000,441.40],
/* Mar 2013 */
[1362096000000,430.47],
[1362355200000,420.05],
[1362441600000,431.14],
[1362528000000,425.66],
[1362614400000,430.58],
[1362700800000,431.72],
[1362960000000,437.87],
[1363046400000,428.43],
[1363132800000,428.35],
[1363219200000,432.50],
[1363305600000,443.66]
];";
        }
    }
}
