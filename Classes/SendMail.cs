using System;
using System.Net;
using System.Net.Mail;

namespace RegIN_Fadeev.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("fadeevsasha0000@yandex.ru", "blhlbievoaadxhef"),
                EnableSsl = true,
            };
            smtpClient.Send("fadeevsasha0000@yandex.ru", To, "Проект RegIn" ,Message);
        }
    }
}
