using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Timers;
namespace ExcelLink
{
   public class Test
   {
       private Excel.Application xlApp;
       private Excel.Workbook xlBook;
       private Excel.Worksheet xlSheet;
       private Excel.Range xlRange;
       private Timer _timer = new Timer();
       private bool _lastOrderMatched;
       xlTradeOrder _lastOrder = new xlTradeOrder();
       public event OrderMatched onMatch;


       public void Connect()
       {
           xlApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
           Console.WriteLine("Connected to : " + xlApp.ActiveWorkbook.Name);

       }

       public void WriteTest()
       {
           xlBook = xlApp.ActiveWorkbook;
           xlSheet = xlBook.Worksheets.get_Item(1);
           Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
           Excel.Range range = xlSheet.get_Range("A1", last);
           int lastUsedRow = last.Row + 1;
           int lastUsedColumn = last.Column + 1;
           Console.WriteLine("Rows used {0}  Columns used {1}",lastUsedRow,lastUsedColumn );

           for(int row = 1;row<10;row++)
               for (int col = 1; col < 10; col++)
           xlSheet.Cells[row, col] = "Test col:"+col+" row:"+row ;
          

       
       }

       public List<string> ReadTest()
       {
           xlBook = xlApp.ActiveWorkbook;
           xlSheet = xlBook.Worksheets.get_Item(1);
           Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
           Excel.Range range = xlSheet.get_Range("A1", last);
           int lastUsedRow = last.Row;
           int lastUsedColumn = last.Column + 1;
           Console.WriteLine("Rows used {0}  Columns used {1}", lastUsedRow, lastUsedColumn);

           var lines = new List<string>();
           for (int row = 1; row <lastUsedRow; row++)
           {
               
               var cols = new StringBuilder();
               for (int col = 1; col < lastUsedColumn; col++)
               {                  
                   cols.Append(xlSheet.Cells[row, col].Value+",");
               }
               lines.Add(cols.ToString());
           }
           return lines;
       }
    }
}
