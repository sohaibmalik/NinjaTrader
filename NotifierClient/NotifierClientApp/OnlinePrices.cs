using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections;
using System.Diagnostics;

namespace NotifierClientApp
{
   public class OnlinePrices
    {
       public static double GetALSI()
       {

           string Link = "";
           double indexvalue, nettchange, percentchange;
           Link = "http://www.forexpros.com/indices/south-africa-40-futures";        
         getFUTpriceFromString_futPRO(getSourceCode(Link), out indexvalue, out nettchange, out percentchange, "Futures");

         return indexvalue;
       }

       private static string getSourceCode(string URL)
       {
           try
           {
               WebClient web = new WebClient();
               String htmlSourceCode = web.DownloadString(URL);
               return htmlSourceCode;
           }
           catch (Exception ex)
           {

               return null;
           }
       }

       private static void getFUTpriceFromString_futPRO(string source, out double indexvalue, out double nettChange, out double percentChange, string patternID)
       {
           try
           {
               const string quote = "\"";
               MatchCollection m1 = Regex.Matches(source, @"/recentQuotesBoxWithTabsTop\s*(.+?)\s*recent_quotes_table", RegexOptions.Multiline);
               MatchCollection m2 = Regex.Matches(m1[0].ToString(), patternID + @"</a></td><td\s*(.+?)\s*nbsp", RegexOptions.Multiline);
               MatchCollection m3 = Regex.Matches(m2[0].ToString(), @">\s*(.+?)\s*<", RegexOptions.Multiline);
               ArrayList results = new ArrayList();




               foreach (Match m in m3)
               {
                   string price = m.Groups[1].Value;

                   results.Add(price);

                   //Debug.WriteLine(price);

               }


               //INDEX VALUE
               string foundindex = results[1].ToString();
               //string foundindex = GetStringInBetween(">", "<", results[1].ToString());
               //Debug.WriteLine("======" + foundindex + "======= indexValue");



               ////Percent Change
               //string tempstring = results[0].ToString().Remove(0, 1);
               ////Debug.WriteLine(tempstring + " change");
               string foundpercent_raw = GetStringInBetween(">", "%", results[5].ToString());
               string foundpercent = foundpercent_raw;//.Replace("%", "");



               ////NETTCHANGE
               //int count = foundindex.Length;
               //string test = tempstring.Replace(foundindex, "");
               //string test2 = test.Replace(" ", "#");
               //string test3 = test2.Remove(0, 12);
               string foundnett = results[3].ToString();//GetStringInBetween("", "#", test3, false);




               indexvalue = Double.Parse(foundindex);
               nettChange = Double.Parse(foundnett);
               percentChange = Double.Parse(foundpercent);

           }
           catch (Exception e)
           {
               indexvalue = 0;
               nettChange = 0;
               percentChange = 0;
           }

       }

       private static string GetStringInBetween(string strStart, string strEnd, string strSource)
       {
           try
           {
               string input = strSource;
               int start = input.IndexOf(strStart);
               int stop = input.IndexOf(strEnd);
               string output = input.Substring(start + 1, stop - start - 1);


               //Debug.WriteLine(output);

               return output;
           }
           catch (Exception ex)
           {
               Debug.WriteLine("Error");
               return "error";
           }

       }

       private static string GetStringInBetween(string strStart, string strEnd, string strSource, bool firstCharIsNothing)
       {
           string input = strSource;
           int start = input.IndexOf(strStart);
           int stop = input.IndexOf(strEnd);

           string output = input.Substring(start, stop - start);


           // Debug.WriteLine(output);

           return output;
       }

    }
}
