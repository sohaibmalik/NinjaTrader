using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifierWebService
{
    public class Chat
    {
        public DateTime Time { get; set; }
        public long FromUserID { get; set; }
        public List<long> ToUserID { get; set; }
        public string Message { get; set; }
        public int MessageID { get; set; }
       


       public static int MesID = 0;
        public static List<Chat> ChatList = new List<Chat>();
    }
}