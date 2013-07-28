using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomic_Clock_Synch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Syncronizing time....");
            Console.WriteLine(SynchTime()?"Successfully syncronized !":"Syncronization failed !");
        }

        private static  bool SynchTime()
        {
            bool synched = false;

            try
            {
                Nist.NistClock c = new Nist.NistClock();
                c.SynchronizeLocalClock();
                synched = true;
            }
            catch { }

            return synched;
        }
    }
}
