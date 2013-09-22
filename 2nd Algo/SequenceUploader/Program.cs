using AlgoSecondLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceUploader
{
    class Program
    {
        static void Main(string[] args)
        {
           var lines= Utils.WriteSequenceToFile();
            Utils.SendDatatoDatabase(lines);
        }
    }
}
