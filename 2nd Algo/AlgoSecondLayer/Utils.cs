using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AlgoSecondLayer
{
   public class Utils
    {
       public static void PrintAllProperties(object obj)
       {
           Console.WriteLine("===========Print new object====================");
           foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
           {
               string name = descriptor.Name;
               object value = descriptor.GetValue(obj);
               Console.WriteLine("{0}={1}", name, value);
           }
           Console.WriteLine("===============================================");
       }
    }
}
