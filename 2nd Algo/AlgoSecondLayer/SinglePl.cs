using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
    public class SinglePl
    {
        private List<int> PL = new List<int>();
        public SinglePl()
        {
            var path = @"D:\PL.csv";
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    var s = sr.ReadLine().Split(',');
                    var pl = int.Parse(s[2]);
                    var d = DateTime.Parse(s[0]);
                    var stamp = new DateTime(d.Year, d.Month, d.Day);
                    PL.Add(pl);
                }
            }
            PL.Reverse();
        }

        public void CheckTradeAfterPL(int filterPL, int forwardWindow)
        {
            int change = 0;
            var sr = new StreamWriter(@"D:\PLTEst.CSV");
            for (int x = 0; x < PL.Count - forwardWindow; x++)
            {
                if (PL[x] > filterPL)
                {
                    var fw = PL[x + forwardWindow];
                    var pl = PL[x];
                    Console.WriteLine("Index {0}  PL {1} FW {2}   Total {3}", x, pl, fw,change );
                    sr.WriteLine("Index {0}  PL {1} FW {2}   Total {3}", x, pl, fw, change);
                    change += fw;
                }
            }
            sr.Close();
            Console.WriteLine("Total change {0} ",change);
        }

    }
}
