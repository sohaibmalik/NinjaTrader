//  All code copyright (c) 2003 Barry Lapthorn
//  Website:  http://www.lapthorn.net
//
//  Disclaimer:  
//  All code is provided on an "AS IS" basis, without warranty. The author 
//  makes no representation, or warranty, either express or implied, with 
//  respect to the code, its quality, accuracy, or fitness for a specific 
//  purpose. Therefore, the author shall not have any liability to you or any 
//  other person or entity with respect to any liability, loss, or damage 
//  caused or alleged to have been caused directly or indirectly by the code
//  provided.  This includes, but is not limited to, interruption of service, 
//  loss of data, loss of profits, or consequential damages from the use of 
//  this code.
//
//
//  $Author: barry $
//  $Revision: 1.1 $
//
//  $Id: Test.cs,v 1.1 2003/08/19 20:59:05 barry Exp $






using BTL.generic;
using System;



public class Test
{
    //  optimal solution for this is (0.5,0.5)
    public static double theActualFunction(double[] values)
    {
        if (values.GetLength(0) != 9)
            throw new ArgumentOutOfRangeException("should only have 9 args");


        var bb = @" this.Fast_K = int.Parse(s[0]);
                    this.Slow_K = int.Parse(s[1]);
                    this.Slow_D = int.Parse(s[2]);
                    this.UPPER_75 = int.Parse(s[3]);
                    this.LOWER_25 = int.Parse(s[4]);
                    this.LIMIT_HIGH = int.Parse(s[5]);
                    this.LIMIT_LOW = int.Parse(s[6]);
                    this.STOPLOSS = int.Parse(s[7]);
                    this.TAKEPROFIT = int.Parse(s[8]);
                    this.CLOSE_END_OF_DAY = false;";
     
       

        double a = values[0];
        double b = values[1];
        double c = values[2];
        double d = values[3];
        double e = values[4];
        double f = values[5];
        double g = values[6];
        double h = values[7];
        double i = values[8];
        // double f1 = Math.Pow(15 * x * y * (1 - x) * (1 - y) * Math.Sin(n * Math.PI * x) * Math.Sin(n * Math.PI * y), 2);
        //return f1;

        var seq = ((int)a).ToString() + "," + ((int)b).ToString() + "," + ((int)c).ToString() +
            "," + ((int)d).ToString() + "," + ((int)e).ToString() + "," + ((int)f).ToString() +
             "," + ((int)g).ToString() + "," + ((int)h).ToString() + "," + ((int)i).ToString();

        var s = new AlgoSecondLayer.StochPOP();
        var o = s.Start(seq, null, true);


        //AddOutput("=================", true);
        //AddOutput("Sequence " + o[0], true);
        //AddOutput("Profit " + o[1], true);
        //AddOutput("Trades " + o[2], true);

        double profit = double.Parse(o[1]);
        double trades = double.Parse(o[2]);
        double avg = profit / (trades+1);

        return profit;
       
    }

    public static void Main()
    {

        string localdata = @"Data Source=PITER-PC;Initial Catalog=AlsiTrade;Integrated Security=True";
        string remotedata = @"Data Source=85.214.244.19;Initial Catalog=AlsiTrade;Persist Security Info=True;User ID=Tradebot;Password=boeboe;MultipleActiveResultSets=True";

        Console.WindowWidth = 100;
        Console.WriteLine("Getting Prices...");
        AlsiUtils.Data_Objects.GlobalObjects.CustomConnectionString = remotedata;

        AlsiUtils.Data_Objects.GlobalObjects.Points = AlsiUtils.DataBase.readDataFromDataBase(AlsiUtils.Data_Objects.GlobalObjects.TimeInterval.Minute_5, AlsiUtils.DataBase.dataTable.MasterMinute, new DateTime(2012, 01, 01), new DateTime(2014, 01, 01), false);
        Console.WriteLine("Done.");



        //  Crossover		= 80%
        //  Mutation		=  5%
        //  Population size = 100
        //  Generations		= 2000
        //  Genome size		= 2
        GA ga = new GA(0.8, 0.05, 10000, 10, 9,MaxMin.Minimize );

        ga.FitnessFunction = new GAFunction(theActualFunction);

        //ga.FitnessFile = @"E:\fitness.csv";
        ga.Elitism = true;
        ga.Go();

        double[] values;
        double fitness;
        ga.GetBest(out values, out fitness);
        Console.WriteLine("Best ({0}):", fitness);
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine("{0} ", values[i]);

        ga.GetWorst(out values, out fitness);
        Console.WriteLine("\nWorst ({0}):", fitness);
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine("{0} ", values[i]);

        Console.ReadLine();
    }
}
