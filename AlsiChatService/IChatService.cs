using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AlsiChatService
{

    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        void Login(User user);

        [OperationContract]
        void Logout(User user);

        [OperationContract]
        void SendMessage(User FromUSer, User ToUser);

        [OperationContract]
        void GetMessage(User ToUser);

        [OperationContract]
        DateTime GetServerTime();
    }



    [DataContract]
    public class User
    {
        private bool LoggedIn;

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int UserId { get; set; }

    }

    [DataContract]
    public class Message
    {
        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public string MsgString { get; set; }

        [DataMember]
        public User To { get; set; }

        [DataMember]
        public User From { get; set; }

        [DataMember]
        public int MsgID { get; set; }
    }
}
