using System;
using System.Collections.Generic;
using System.Linq;
using AlsiUtils;

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

        private AlsiTMDataContext dc = new AlsiTMDataContext();


        public void InsertChatMessage(Chat msg)
        {
            var m = new tblMessage()
            {
                TBL_MSG_TEXT = msg.Message,
                TBL_MSG_TIME = msg.Time,
            };

            dc.tblMessages.InsertOnSubmit(m);
            dc.SubmitChanges();

         
                tblMessageUser c = new tblMessageUser()
                {
                    MSG_USER_FROM = msg.FromUserID,
                    MSG_USER_TO = msg.ToUserID,
                    MSG_ID = dc.tblMessages.Where(z => z.TBL_MSG_TIME == msg.Time).First().TBL_MSG_ID,
                };
                dc.tblMessageUsers.InsertOnSubmit(c);
            
            dc.SubmitChanges();
        }

        public List<Chat> GetChatMessages(int ThisUser, int FromUser)
        {
            var msgList = new List<Chat>();

            var All1 = dc.tblMessageUsers.Where(z => z.MSG_USER_FROM == ThisUser && z.MSG_USER_TO == FromUser);
            // var All2 = dc.tblMessageUsers.Where(z => );
            //  var All = All1.Union(All2).OrderBy(z => z.tblMessage.TBL_MSG_TIME).Where(z => z.tblMessage.TBL_MSG_TIME >= DateTime.Now.AddDays(-1));
            var All = All1.OrderBy(z => z.tblMessage.TBL_MSG_TIME).Where(z => z.tblMessage.TBL_MSG_TIME >= DateTime.Now.AddDays(-1));
            foreach (var msg in All)
            {
                //foreach (int i in msg.MSG_USER_TO)
               // {
                    var c = new Chat()
                     {
                         FromUserID = (int)msg.MSG_USER_FROM,
                         ToUserID = (int)msg.MSG_USER_TO,
                         Time = (DateTime)msg.tblMessage.TBL_MSG_TIME,
                         Message = msg.tblMessage.TBL_MSG_TEXT,
                         MessageID = (int)msg.MSG_ID

                     };
                    msgList.Add(c);
              //  }
            }
            return msgList;
        }
    }
}
