using System;
using System.Diagnostics;
using System.Windows.Forms;
using ExcelLink;

namespace NinjaTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExcelOrder x = new ExcelOrder();
            x.Connect();

            foreach (var o in x.ReadAllOrders()) Debug.WriteLine(o.Contract + "  " + o.BS + "  " + o.Volume + "  " + o.Price + "  " + o.Status);



        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExcelOrder x = new ExcelOrder();
            x.Connect();
            var o = new xlTradeOrder()
            {
                Contract = "test",
                BS = xlTradeOrder.BuySell.Buy,
                Price = 31200,
                Volume = 3,
                Status = xlTradeOrder.orderStatus.Ready,
            };
            x.WriteOrder(o);
            x.onMatch += new OrderMatched(x_onMatch);
            x.StartCheckWhenOrderCompletedTimer(10000);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExcelOrder x = new ExcelOrder();
            x.Connect();
            x.onMatch += new OrderMatched(x_onMatch);
            x.StartCheckWhenOrderCompletedTimer(10000);
            //Debug.WriteLine("Completed "+x.isLastOrderComplete(out o));
            //Debug.WriteLine(o.Contract + "  " + o.BS + "  " + o.Volume + "  " + o.Price + "  " + o.Status);


        }

        void x_onMatch(object sender, OrderMatchEventArgs e)
        {
            Debug.WriteLine("Event " + e.Order.Status);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AlsiWebService.AlsiNotifyService n = new AlsiWebService.AlsiNotifyService();
            var o = n.getLastOrder();

            Debug.WriteLine(o.BS + " " + o.Price + "  " + o.Status);
        }


    }
}
