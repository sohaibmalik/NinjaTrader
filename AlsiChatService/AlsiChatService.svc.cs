using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AlsiChatService
{
   

    public class AlsiChat : IChatService
    {
       

        public void Login(User user)
        {
            throw new NotImplementedException();
        }

        public void Logout(User user)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(User FromUSer, User ToUser)
        {
            throw new NotImplementedException();
        }

        public void GetMessage(User ToUser)
        {
            throw new NotImplementedException();
        }


        public DateTime GetServerTime()
        {
            throw new NotImplementedException();
        }
    }
}
