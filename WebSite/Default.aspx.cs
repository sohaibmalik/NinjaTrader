using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
public partial class _Default : System.Web.UI.Page
{

    AlsiService.AlsiNotifyService service = new AlsiService.AlsiNotifyService();
    //localhost.AlsiNotifyService service = new localhost.AlsiNotifyService();

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (pswTextBox.Text.Length > 0)
            service.SendCommand(AlsiService.Command.RestartPC);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        var mesg = service.GetAllMessages();

        alsiTradeLabel.BackColor = Color.Black;
        alsiTradeLabel.ForeColor = Color.Orange;
        dataManagerLabel.BackColor = Color.Black;
        dataManagerLabel.ForeColor = Color.Orange;
        dataManagerLabel.Text = "Not Running";
        alsiTradeLabel.Text = "Not Running";

        foreach (var m in mesg)
        {
            if (m.Message == "AlsiTrade")
            {
                alsiTradeLabel.Text = "Alsi Trade : " + m.TimeStamp.ToShortTimeString();
                if (m.TimeStamp < DateTime.Now.AddMinutes(-2)) alsiTradeLabel.ForeColor = Color.Red;
                else
                    alsiTradeLabel.ForeColor = Color.LightGreen;
            }
            if (m.Message == "DataManager")
            {
                dataManagerLabel.Text = "DataManager :  " + m.TimeStamp.ToShortTimeString();
                if (m.TimeStamp < DateTime.Now.AddMinutes(-2)) dataManagerLabel.ForeColor = Color.Red;
                else
                    dataManagerLabel.ForeColor = Color.LightGreen;
            }
        }




    }
}




   