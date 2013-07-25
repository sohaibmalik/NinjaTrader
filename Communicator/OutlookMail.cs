using Outlook = Microsoft.Office.Interop.Outlook;

namespace Communicator
{
	public class OutlookMail
	{
		private Outlook.Application oApp;
		private Outlook._NameSpace oNameSpace;
		private Outlook.MAPIFolder oOutboxFolder;

		public OutlookMail()
		{
			//http://www.c-sharpcorner.com/uploadfile/casperboekhoudt/sendingemailsthroughoutlook12052005000124am/sendingemailsthroughoutlook.aspx

			oApp = new Outlook.Application();
			oNameSpace = oApp.GetNamespace("MAPI");
			oNameSpace.Logon(null, null, true, true);

			//Calender: Outlook.OlDefaultFolders.olFolderCalendar 
			//Contacts: Outlook.OlDefaultFolders.olFolderContacts 
			//Inbox: Outlook.OlDefaultFolders.olFolderInbox 

			oOutboxFolder = oNameSpace.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderOutbox);
		}
		/// <summary>
		/// Send Email Using Outlook
		/// </summary>
		/// <param name="toValue">Receipient Email Adress</param>
		/// <param name="subjectValue">Email Subject</param>
		/// <param name="bodyValue">Email Body</param>
		public void addToOutBox(string toValue, string subjectValue, string bodyValue)
		{
			try
			{
				Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
				oMailItem.To = toValue;
				oMailItem.Subject = subjectValue;
				oMailItem.Body = bodyValue;
				oMailItem.SaveSentMessageFolder = oOutboxFolder;
				//uncomment this to also save this in your draft 
				//oMailItem.Save(); 
				//adds it to the outbox 
				oMailItem.Send();
			}
			catch
			{
			}
		}

	}
}
