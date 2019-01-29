using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace StepOutApi.Model
{
    class Mailer
    {
        public static void SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                NetworkCredential networkCredential = new NetworkCredential(Environment.GetEnvironmentVariable("MailUser"), Environment.GetEnvironmentVariable("MailPassword"));
                string mailServer = Environment.GetEnvironmentVariable("SMTPServer");
                MailAddress fromAdres = new MailAddress(from);
                MailAddress toAdres = new MailAddress(to);
                MailMessage message = new MailMessage(fromAdres, toAdres);
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;
                SmtpClient client = new SmtpClient(mailServer, Convert.ToInt32(587));
                client.Credentials = networkCredential;
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
