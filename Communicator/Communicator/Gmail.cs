using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace Communicator
{
    public class Gmail
    {


        /// <summary>
        /// Send Gmail Email Using Specified Gmail Account
        /// </summary>
        /// <param name="address">Receipient Adress</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="message">Enail Body</param>
        /// <param name="AttachmentLocations">List of File locations, null if no Attachments</param>
        /// <param name="yourEmailAdress">Gmail Login Adress</param>
        /// <param name="yourPassword">Gmail Login Password</param>
        /// <param name="yourName">Display Name that Receipient Will See</param>
        /// <param name="IsBodyHTML">Is Message Body HTML</param>
        public static void SendEmail(string address, string subject, string message, List<string> AttachmentLocations, string yourEmailAdress, string yourPassword, string yourName, bool IsBodyHTML)
        {
            try
            {
                string email = yourEmailAdress;
                string password = yourPassword;

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);


                msg.From = new MailAddress(email, yourName);
                msg.To.Add(new MailAddress(address));
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = IsBodyHTML;
                if (AttachmentLocations != null)
                {
                    foreach (string attachment in AttachmentLocations)
                    {
                        msg.Attachments.Add(new Attachment(attachment));
                    }
                }
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);
            }
            catch { }
        }



        /// <summary>
        /// Send Gmail Email Using Specified Gmail Account
        /// </summary>
        /// <param name="address">Receipients Adresses</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="message">Enail Body</param>
        /// <param name="AttachmentLocations">List of File locations, null if no Attachments</param>
        /// <param name="yourEmailAdress">Gmail Login Adress</param>
        /// <param name="yourPassword">Gmail Login Password</param>
        /// <param name="yourName">Display Name that Receipient Will See</param>
        /// <param name="IsBodyHTML">Is Message Body HTML</param>
        public static void SendEmail(List<string> addresses, string subject, string message, List<string> AttachmentLocations, string yourEmailAdress, string yourPassword, string yourName, bool IsBodyHTML)
        {
            try
            {
                string email = yourEmailAdress;
                string password = yourPassword;

                var loginInfo = new NetworkCredential(email, password);
                var msg = new MailMessage();
                var smtpClient = new SmtpClient("smtp.gmail.com", 587);


                msg.From = new MailAddress(email, yourName);
                foreach (string address in addresses)
                {
                    msg.To.Add(new MailAddress(address));
                }
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = IsBodyHTML;
                if (AttachmentLocations != null)
                {
                    foreach (string attachment in AttachmentLocations)
                    {
                        msg.Attachments.Add(new Attachment(attachment));
                    }
                }
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(msg);

            }
            catch { }
        }



        /// <summary>
        /// gets all unread email from Gmail and returns a List of System.Net.Email.MailMessage Objects
        /// </summary>
        /// <param name="username">Gmail Login</param>
        /// <param name="password">mail Password</param>
        public static List<MailMessage> FetchAllUnreadMessages(string username, string password)
        {
            List<MailMessage> mlist = new List<MailMessage>();
            // The client disconnects from the server when being disposed

            try
            {
                using (Pop3Client client = new Pop3Client())
                {
                    // Connect to the server
                    client.Connect("pop.gmail.com", 995, true);

                    // Authenticate ourselves towards the server
                    client.Authenticate(username, password);

                    // Get the number of messages in the inbox
                    int messageCount = client.GetMessageCount();





                    for (int x = messageCount; x > 0; x--)
                    {

                        Message m = new Message(client.GetMessage(x).RawMessage);
                        System.Net.Mail.MailMessage mm = m.ToMailMessage();

                        mlist.Add(mm);

                    }
                    return mlist;
                }
            }
            catch { return mlist; }
        }







    }
}
