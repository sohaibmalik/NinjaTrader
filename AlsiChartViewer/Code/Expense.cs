using System;
using System.Collections.Generic;
using System.Web;

namespace WebApplication1.Code
{
    public class Expense
    {
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public static List<Expense> GetAmountRandomAmountList()
        {
            Random randomizer = new Random();
            List<Expense> result = new List<Expense>();
            DateTime startDate = new DateTime(2009, 1, 1);

            for (int index = 0; index < 24; index++)
            {
                result.Add(new Expense { 
                    Amount = randomizer.Next(100),
                    Date = startDate
                });
                startDate = startDate.AddMonths(1);
            }

            return result;
        }
    }





}