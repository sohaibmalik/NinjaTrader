using System;
using System.Collections.Generic;
using System.Linq;
using AlsiUtils;
using System.Text;
using System.Diagnostics;

namespace NotifierClientApp
{
    public class Chat
    {


        public DateTime Time { get; set; }
        public int FromUserID { get; set; }
        public List<int> ToUsersIDs { get; set; }
        public int ToUserID { get; set; }
        public string Message { get; set; }
        public int MessageID { get; set; }
        public bool Viewed { get; set; }


        public AlsiTMDataContext dc = new AlsiTMDataContext();


        public override string ToString()
        {

            var c = new StringBuilder();
            c.Append("FromID:" + FromUserID + "(" + dc.tblUsers.Where(x => x.ID == FromUserID).First().USER_NAME + ")"
                + " ToID:" + ToUserID + "(" + dc.tblUsers.Where(x => x.ID == ToUserID).First().USER_NAME + ")" + "  Msg:" + Message);
            return c.ToString();
        }

        public void InsertChatMessage(Chat msg)
        {
            var m = new tblMessage()
            {
                TBL_MSG_TEXT = msg.Message,
                TBL_MSG_TIME = msg.Time,
            };

            dc.tblMessages.InsertOnSubmit(m);
            dc.SubmitChanges();

        }

        public void InsertChatUserMessage(Chat msg)
        {
            tblMessageUser c = new tblMessageUser()
            {
                MSG_USER_FROM = msg.FromUserID,
                MSG_USER_TO = msg.ToUserID,
                MSG_ID = dc.tblMessages.Where(z => z.TBL_MSG_TIME == msg.Time).First().TBL_MSG_ID,

            };
            dc.tblMessageUsers.InsertOnSubmit(c);
            dc.SubmitChanges();
        }

        private NewMessageEventArgs e;
        private int _InboxMsgCount;
        public int InboxMsgCount
        {
            get { return _InboxMsgCount; }
            set
            {
                if (_InboxMsgCount != 0)
                    if (_InboxMsgCount != value)
                    {
                        Debug.WriteLine("New msg from " + e.FromUser + " : " + e.Message);
                        NewMessage(this, e);
                    }
                _InboxMsgCount = value;

            }
        }

        public List<Chat> GetChatMessages(int ThisUser, int FromUser)
        {
            var msgList = new List<Chat>();
            var Sent = dc.tblMessageUsers.Where(z => z.MSG_USER_FROM == ThisUser && z.MSG_USER_TO == FromUser);
            var Inbox = dc.tblMessageUsers.Where(z => z.MSG_USER_TO == ThisUser && z.MSG_USER_FROM == FromUser);
            var All = Sent.Union(Inbox).OrderBy(z => z.tblMessage.TBL_MSG_TIME);//.Where(z => z.tblMessage.TBL_MSG_TIME >= DateTime.Now.AddDays(-1));

            int fromid = Inbox.AsEnumerable().Last().MSG_USER_FROM;
                e = new NewMessageEventArgs();
                e.FromUser = dc.tblUsers.Where(z => z.ID == fromid).First().USER_NAME;
                e.Message = Inbox.ToList().Last().tblMessage.TBL_MSG_TEXT;
                InboxMsgCount = dc.tblMessageUsers.Where(z => z.MSG_USER_TO == ThisUser).Count();
            
            foreach (var msg in All)
            {
                bool V = false;
                if (msg.MSG_VIEWED == null) V = false;
                else
                    V = (bool)msg.MSG_VIEWED;


                var c = new Chat()
                 {
                     FromUserID = (int)msg.MSG_USER_FROM,
                     ToUserID = (int)msg.MSG_USER_TO,
                     Time = (DateTime)msg.tblMessage.TBL_MSG_TIME,
                     Message = msg.tblMessage.TBL_MSG_TEXT,
                     MessageID = (int)msg.MSG_ID,
                     Viewed = V,

                 };
                msgList.Add(c);

            }
            return msgList;
        }


        public event OnNewMessage NewMessage;
        public delegate void OnNewMessage(object sender, NewMessageEventArgs e);
        public class NewMessageEventArgs : EventArgs
        {
            public string FromUser { get; set; }
            public string Message { get; set; }
        }
    }
}
