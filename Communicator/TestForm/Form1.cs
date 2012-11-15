using System;
using System.Windows.Forms;
using Communicator;
using System.Net.Mail;
using System.Collections.Generic;
using System.Diagnostics;


namespace TestForm
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
			Gmail.SendEmail("pietpiel27@gmail.com",textBox1.Text,"", null, "pieterf33@gmail.com", "1rachelle", "piet SkIET",true);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			OutlookMail outlookmail = new OutlookMail();
			outlookmail.addToOutBox("pietpel27@gmail.com", "testSubject", "testbody 123");

		}

		private void button4_Click(object sender, EventArgs e)
		{
			
			
			List<MailMessage> msg =  Gmail.FetchAllUnreadMessages("pietpiel27@gmail.com", "1rachelle");

			foreach (MailMessage m in msg)
			{
				Debug.WriteLine(m.Sender +  "  " + m.Subject);
				Debug.WriteLine(m.Body);
			}

		}
	}
}
