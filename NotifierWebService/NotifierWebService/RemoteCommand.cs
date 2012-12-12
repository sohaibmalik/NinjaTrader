using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotifierWebService
{
    public class RemoteCommand
    {
        public static Command DoSomething { get; set; }

      public enum Command
       {
           RestartPC=1,
           Idle=100,

       }
    }
}