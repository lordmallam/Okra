using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.IO;
using System.Net.Security;

namespace Okra.Mail
{
    public class Mail
    {
        public Mail()
        {

        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="from">From email address</param>
        /// <param name="recepient">Recepient's email address</param>
        /// <param name="bcc">bbc copy email address</param>
        /// <param name="cc">cc copy email address</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="body">Body of the email</param>
        /// <param name="username">Username of the sending email</param>
        /// <param name="password">Password of the sending email</param>
        /// <param name="SMTPHost">SMTP server</param>
        /// <param name="SMTPPort">SMTP Port</param>
        
        public void SendMailMessage(string from, string recepient, string bcc, string cc, string subject, string body, string username, string password, string SMTPHost, int SMTPPort)
        {
            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();

            // Set the sender address of the mail message
            mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            mMailMessage.To.Add(new MailAddress(recepient));

            // Check if the bcc value is nothing or an empty string
            if ((bcc != null) & bcc != string.Empty)
            {
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            // Check if the cc value is nothing or an empty value
            if ((cc != null) & cc != string.Empty)
            {
                // Set the CC address of the mail message
                mMailMessage.CC.Add(new MailAddress(cc));
            }



            // Set the subject of the mail message
            mMailMessage.Subject = subject;
            // Set the body of the mail message
            mMailMessage.Body = body;

            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();
            // Send the mail message

            mSmtpClient.Credentials = new System.Net.NetworkCredential(username,password);
            //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //mSmtpClient.UseDefaultCredentials = true;
            mSmtpClient.Host = SMTPHost ;
            mSmtpClient.Port = SMTPPort;
            mSmtpClient.EnableSsl = true;
            try
            {
                mSmtpClient.Send(mMailMessage);
                
            }
            catch (Exception)
            {
                
                throw;
                
            }
            

        }

        public void SendMailMessageNotSecure(string from, string recepient, string bcc, string cc, string subject, string body, string username, string password, string SMTPHost, int SMTPPort)
        {
            // Instantiate a new instance of MailMessage
            MailMessage mMailMessage = new MailMessage();

            // Set the sender address of the mail message
            mMailMessage.From = new MailAddress(from);
            // Set the recepient address of the mail message
            mMailMessage.To.Add(new MailAddress(recepient));

            // Check if the bcc value is nothing or an empty string
            if ((bcc != null) & bcc != string.Empty)
            {
                // Set the Bcc address of the mail message
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            // Check if the cc value is nothing or an empty value
            if ((cc != null) & cc != string.Empty)
            {
                // Set the CC address of the mail message
                mMailMessage.CC.Add(new MailAddress(cc));
            }



            // Set the subject of the mail message
            mMailMessage.Subject = subject;
            // Set the body of the mail message
            mMailMessage.Body = body;

            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();
            // Send the mail message

            mSmtpClient.Credentials = new System.Net.NetworkCredential(username, password);
            //mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //mSmtpClient.UseDefaultCredentials = true;
            mSmtpClient.Host = SMTPHost;
            mSmtpClient.Port = SMTPPort;
            mSmtpClient.EnableSsl = false;
            try
            {
                mSmtpClient.Send(mMailMessage);

            }
            catch (Exception)
            {

                throw;

            }


        }

        /// <summary>
        /// Gets HTML of a web page
        /// </summary>
        /// <param name="url">Address to the web page</param>
        /// <returns>string result</returns>
        public string getPageHTML(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            string result = string.Empty;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(data))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
