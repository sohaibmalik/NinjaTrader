using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlsiTrade_Backend
{
    
    public class WebServiceUpdate
    {
      private static  AlsiWebService.AlsiNotifyService service;

        public WebServiceUpdate()
        {
            service = new AlsiWebService.AlsiNotifyService();
        }

        public  void ReportStatus()
        {
            var b = new AlsiWebService.Boodskap()
            {
                TimeStamp=DateTime.UtcNow.AddHours(2),
                Message=AlsiWebService.Messages.isAlive,
                Message_Custom="AlsiTrade"                              

            };

            service.InsertMessage(b);
        }

    }
}
