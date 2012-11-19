using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelLink
{
    public class ExcelOrder
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlBook;
        private Excel.Worksheet xlSheet;
        private Excel.Range xlRange;

        object misVal = System.Reflection.Missing.Value;

        public void Connect()
        {
            xlApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            Debug.WriteLine("Connected to : " + xlApp.ActiveWorkbook.Name);
        }

        public void WriteOrder(xlTradeOrder Order)
        {
            xlBook = xlApp.ActiveWorkbook;
            xlSheet = xlBook.Worksheets.get_Item(2);
            Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlSheet.get_Range("A1", last);
            int lastUsedRow = last.Row+1;
            int lastUsedColumn = last.Column+1;
            Debug.WriteLine("Rows used " + lastUsedRow);

            xlSheet.Cells[lastUsedRow, 1] = Order.Contract;
            xlSheet.Cells[lastUsedRow, 2] = (Order.BS == xlTradeOrder.BuySell.Buy) ? "B" : "S";
            xlSheet.Cells[lastUsedRow, 3] = Order.Volume;
            xlSheet.Cells[lastUsedRow, 4] = Order.Price;
            xlSheet.Cells[lastUsedRow, 5] = Order.Principle;
            xlSheet.Cells[lastUsedRow, 6] = Order.Dealer;
            xlSheet.Cells[lastUsedRow, 9] = Order.Member;
            xlSheet.Cells[lastUsedRow, 10] = Order.Type;
            xlSheet.Cells[lastUsedRow, 11] = Order.Exchange;
            xlSheet.Cells[lastUsedRow, 12] = Order.Status;

        }

        public void Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //Marshal.FinalReleaseComObject(xlRange);
            Marshal.FinalReleaseComObject(xlSheet);
            // xlBook.Close(Type.Missing, Type.Missing, Type.Missing);
            Marshal.FinalReleaseComObject(xlBook);
            Marshal.FinalReleaseComObject(xlApp);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Debug.WriteLine("Disconnected");
        }
    }
}
