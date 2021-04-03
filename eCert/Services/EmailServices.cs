using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace eCert.Services
{
    public class EmailServices
    {
        public async Task SendEmail(string receiver, string mailSubject, string mailContent)
        {
            using(var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential()
                {
                    UserName = "hoangnguyenthanhdan@gmail.com",
                    Password = "112055148"
                };
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                smtpClient.Send("hoangnguyenthanhdan@gmail.com", receiver, mailSubject, mailContent);
            }
        }
    }
}