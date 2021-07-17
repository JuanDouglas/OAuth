using System.Net;
using System.Net.Email;

namespace OAuth.Api.Models
{
    public class Smtp
    {
        static string smtpAddress = "smtp.gmail.com";  
        static int portNumber = 587;  
        static bool enableSSL = true;  
        static string emailFromAddress = "sender@gmail.com"; //Sender Email Address  
        static string password = "Abc@123$%^"; //Sender Password   
        static string subject = "Hello";  
        static string body = "Hello, This is Email sending test using gmail.";  
        public static void SendEmail(string receiver) {  
            using(MailMessage mail = new MailMessage()) {  
                mail.From = new MailAddress(emailFromAddress);  
                mail.To.Add(receiver);  
                mail.Subject = subject;  
                mail.Body = body;  
                mail.IsBodyHtml = true;  
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using(SmtpClient smtp = new SmtpClient(smtpAddress, portNumber)) {  
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);  
                    smtp.EnableSsl = enableSSL;  
                    smtp.Send(mail);  
                }  
            }  
        }  
    }
}