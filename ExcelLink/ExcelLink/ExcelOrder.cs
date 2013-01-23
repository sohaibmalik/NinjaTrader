using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Excel = Microsoft.Office.Interop.Excel;
using AlsiUtils;
namespace ExcelLink
{
    public delegate void OrderMatched(object sender, OrderMatchEventArgs e);
    public class OrderMatchEventArgs:EventArgs
    {
        public xlTradeOrder Order;
       
    }

    public class ExcelOrder
    {
        private Excel.Application xlApp;
        private Excel.Workbook xlBook;
        private Excel.Worksheet xlSheet;
        private Excel.Range xlRange;
        private Timer _timer =  new Timer();
        private bool _lastOrderMatched;
        xlTradeOrder _lastOrder = new xlTradeOrder();
        public event OrderMatched onMatch;
      
       

        object misVal = System.Reflection.Missing.Value;
        public ExcelOrder()
        {
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        }

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
            int lastUsedRow = last.Row + 1;
            int lastUsedColumn = last.Column + 1;
            //Debug.WriteLine("Rows used " + lastUsedRow);

            xlSheet.Cells[lastUsedRow, 1] = Order.Contract;
            xlSheet.Cells[lastUsedRow, 2] = (Order.BS == Trade.BuySell.Buy) ? "B" : "S";
            xlSheet.Cells[lastUsedRow, 3] = Order.Volume;
            xlSheet.Cells[lastUsedRow, 4] = Order.Price;
            xlSheet.Cells[lastUsedRow, 5] = Order.Principle;
            xlSheet.Cells[lastUsedRow, 6] = Order.Dealer;
            xlSheet.Cells[lastUsedRow, 9] = Order.Member;
            xlSheet.Cells[lastUsedRow, 10] = Order.Type;
            xlSheet.Cells[lastUsedRow, 11] = Order.Exchange;
            xlSheet.Cells[lastUsedRow, 12] = OrderToString(xlTradeOrder.orderStatus.Ready);

            _lastOrderMatched = false;
        }

        public xlTradeOrder ReadLastOrder()
        {
            xlBook = xlApp.ActiveWorkbook;
            xlSheet = xlBook.Worksheets.get_Item(2);
            Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlSheet.get_Range("A1", last);
            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column + 1;
            Debug.WriteLine("Rows used " + lastUsedRow);
            var Order = new xlTradeOrder();

            Order.Contract = xlSheet.Cells[lastUsedRow, 1].Value;
            Order.BS = (xlSheet.Cells[lastUsedRow, 2].Value == "B") ? Trade.BuySell.Buy : Trade.BuySell.Sell;
            Order.Volume = (int)xlSheet.Cells[lastUsedRow, 3].Value;
            Order.Price = (long)xlSheet.Cells[lastUsedRow, 4].Value;
            Order.Status = StringToOrder(xlSheet.Cells[lastUsedRow, 12].Value);
            Order.GetReference();
            return Order;
        }

        public List<xlTradeOrder> ReadAllOrders()
        {
            var Orders = new List<xlTradeOrder>();
            xlBook = xlApp.ActiveWorkbook;
            xlSheet = xlBook.Worksheets.get_Item(2);
            Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlSheet.get_Range("A1", last);
            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column + 1;
            Debug.WriteLine("Rows used " + lastUsedRow);

            for (int x = 2; x <= lastUsedRow; x++)
            {
                var order = new xlTradeOrder();
                order.Contract = xlSheet.Cells[x, 1].Value;
                order.BS = (xlSheet.Cells[x, 2].Value == "B") ? Trade.BuySell.Buy : Trade.BuySell.Sell;
                order.Volume = (int)xlSheet.Cells[x, 3].Value;
                order.Price = (long)xlSheet.Cells[x, 4].Value;
                order.Status = StringToOrder(xlSheet.Cells[x, 12].Value);
                order.GetReference();
                Orders.Add(order);

            }

            return Orders;
        }



        public bool isLastOrderComplete(out xlTradeOrder Order)
        {
            Connect();
            Order = ReadLastOrder();
            if (Order.Status == xlTradeOrder.orderStatus.Completed) return true;
            return false;
        }

        public void StartCheckWhenOrderCompletedTimer(int interval)
        {           
            _timer.Interval=interval;            
            _timer.Enabled = true;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            if (isLastOrderComplete(out _lastOrder) && !_lastOrderMatched)
            {
                OrderMatchEventArgs match = new OrderMatchEventArgs();
                _lastOrderMatched = true;                
                match.Order = _lastOrder;              
                onMatch(this, match);
                _timer.Stop();
                
            }
            Disconnect();

        }

        private string OrderToString(xlTradeOrder.orderStatus status)
        {
            if (status == xlTradeOrder.orderStatus.Ready) return "Ready";
            if (status == xlTradeOrder.orderStatus.Completed) return "Completed";
            if (status == xlTradeOrder.orderStatus.Cancelled) return "Cancelled";
            return "";
        }

        private xlTradeOrder.orderStatus StringToOrder(string status)
        {
            if (status == "Ready") return xlTradeOrder.orderStatus.Ready;
            if (status == "Completed") return xlTradeOrder.orderStatus.Completed;
            if (status == "Cancelled") return xlTradeOrder.orderStatus.Cancelled;
            if (status == "Active") return xlTradeOrder.orderStatus.Active;
            return xlTradeOrder.orderStatus.None;
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
