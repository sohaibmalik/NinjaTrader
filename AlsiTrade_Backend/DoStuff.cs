using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlsiUtils;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
namespace AlsiTrade_Backend
{
    public class DoStuff
    {
        public static void ExportToText(List<Trade> Trades)
        {
           
            SaveFileDialog fs = new SaveFileDialog();
            fs.ShowDialog();
            FileInfo f;
            try
            {
                f = new FileInfo(fs.FileName);
            }
            catch
            {
                return;
            }
            
           


            StreamWriter sr = new StreamWriter(f.FullName);
            foreach (var t in Trades)
            {
                sr.WriteLine(
                    t.TimeStamp + ","
                   + t.Reason + ","
                   + t.BuyorSell + ","
                   + t.CurrentDirection + ","
                   + t.CurrentPrice + ","
                   + t.Position + ","
                   + t.RunningProfit + ","
                   + t.TotalPL + ","
                   + t.InstrumentName
                    );
            }
            sr.Close();
            MessageBox.Show("Export Complete");
        }
    }
}
