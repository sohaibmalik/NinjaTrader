using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Communicator
{
  public  class EmailMsg
    {
      public string Body { get; set; }
      public string Title { get; set; }
      public bool Html { get; set; }
    }

    public  class SmsMsg
    {
        public string text { get; set; }
        public string DestinationNr { get; set; }
    }

}
