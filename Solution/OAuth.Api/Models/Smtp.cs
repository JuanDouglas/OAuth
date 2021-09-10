using System;
using System.Net;
using System.Net.Mail;

namespace OAuth.Api.Models
{
    public static class Smtp
    {
        static string smtpAddress = "smtp.gmail.com";
        static int portNumber = 587;
        static string fromAddress = ""; //Sender Email Address  
        static string password = ""; //Sender Password   

        public static void SendEmail(string receiver, string subject, string body, bool isHtml)
        {
            using MailMessage mail = new();
            mail.From = new MailAddress(fromAddress);
            mail.To.Add(receiver);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
            using SmtpClient smtp = new(smtpAddress, portNumber);
            smtp.Credentials = new NetworkCredential(fromAddress, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        internal static void SendEmail(string email, string v1, object enconding, bool v2)
        {
            throw new NotImplementedException();
        }
    }
}