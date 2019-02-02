using System;
using System.Net.Mail;

namespace CommonUtilsX64
{
    public class Smtp
    {
        public string SendMessage(SmtpClient c, MailMessage m)
        {
            try
            {
                c.Send(m);
                return $"Email send to: {m.To}";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

}
