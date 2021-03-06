﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using Excel = Microsoft.Office.Interop.Excel;
using AlsiUtils;
using System.Linq;
using System.Data.Linq;
namespace ExcelLink
{
    public delegate void OrderMatched(object sender, OrderMatchEventArgs e);
    public class OrderMatchEventArgs:EventArgs
    {
        public xlTradeOrder LastOrder;
        public List<xlTradeOrder> AllMatchedOrders;
        public bool Matched { get; set; }
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

        private xlTradeOrder ReadLastOrder()
        {
            xlBook = xlApp.ActiveWorkbook;
            xlSheet = xlBook.Worksheets.get_Item(2);
            Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlSheet.get_Range("A1", last);
            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column + 1;
            Debug.WriteLine("Rows used " + lastUsedRow);
            var Order = new xlTradeOrder();

            Order.TimeStamp = DateTime.UtcNow.AddHours(2);
            Order.Contract = xlSheet.Cells[lastUsedRow, 1].Value;
            Order.BS = (xlSheet.Cells[lastUsedRow, 2].Value == "B") ? Trade.BuySell.Buy : Trade.BuySell.Sell;
            Order.Volume = (int)xlSheet.Cells[lastUsedRow, 3].Value;
            Order.Price = (long)xlSheet.Cells[lastUsedRow, 4].Value;
            Order.Status = StringToOrder(xlSheet.Cells[lastUsedRow, 12].Value);
            Order.GetReference();
            return Order;
        }

        private List<xlTradeOrder> ReadAllInputOrders()
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
                order.Reference = xlSheet.Cells[x, 8].Value;
                Orders.Add(order);

            }

            return Orders;
        }

        private List<xlTradeOrder> ReadAllMatchedOrders()
        {
            var Orders = new List<xlTradeOrder>();
            xlBook = xlApp.ActiveWorkbook;
            xlSheet = xlBook.Worksheets.get_Item(3);
            Excel.Range last = xlSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range = xlSheet.get_Range("A1", last);
            int lastUsedRow = last.Row;
            int lastUsedColumn = last.Column + 1;
            Debug.WriteLine("Rows used " + lastUsedRow);

            for (int x = 2; x <= lastUsedRow; x++)
            {
                var order = new xlTradeOrder();
                order.TimeStamp = DateTime.Parse(xlSheet.Cells[x, 1].Text);
                order.Contract = xlSheet.Cells[x, 2].Value;
                order.BS = (xlSheet.Cells[x, 3].Value == "B") ? Trade.BuySell.Buy : Trade.BuySell.Sell;
                order.Volume = (int)xlSheet.Cells[x, 4].Value;
                order.Price = (long)xlSheet.Cells[x, 5].Value;
                order.Status = xlTradeOrder.orderStatus.Completed;
                order.Reference = xlSheet.Cells[x, 9].Value;
                Orders.Add(order);

            }

            return Orders;
        }

        private List<xlTradeOrder> GetMatchedOrders(out xlTradeOrder LastMatched)
        {
            Connect();
            List<xlTradeOrder> insert = ReadAllInputOrders();
            List<xlTradeOrder> matched = ReadAllMatchedOrders();
            Disconnect();

            var merged =  from i in insert
                         join m in matched
                         on i.Reference equals m.Reference
                         select m;
                       
            LastMatched = merged.OrderByDescending(z => z.TimeStamp).First();
            return merged.ToList();
        }


        public bool isLastOrderComplete()
        {
            Connect();
            var Order = ReadLastOrder();
            _lastOrder = Order;
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
            
            if (isLastOrderComplete() && !_lastOrderMatched)
            {
                xlTradeOrder last;
                OrderMatchEventArgs match = new OrderMatchEventArgs();
                _lastOrderMatched = true;             
                match.LastOrder = _lastOrder;
                match.Matched = true;
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
