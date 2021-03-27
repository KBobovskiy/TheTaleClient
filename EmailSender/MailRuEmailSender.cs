using NLog;
using System;
using System.Net;
using System.Net.Mail;

namespace EmailSender
{
    public class MailRuEmailSender : IEmailSender
    {
        private readonly MailAddress AddressFrom;

        private readonly string emailLogin;
        private readonly string emailPassword;
        private readonly string emailFrom;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MailRuEmailSender(string login, string password, string from)
        {
            emailLogin = login;
            emailPassword = password;
            emailFrom = from;

            if (string.IsNullOrWhiteSpace(emailLogin)
                || string.IsNullOrWhiteSpace(emailPassword)
                || string.IsNullOrWhiteSpace(emailFrom))
            {
                logger.Info($"Email login or password or From not set!");
                return;
            }

            AddressFrom = new MailAddress(emailLogin, emailFrom);
        }

        public bool TrySendEmail(EmailMessage message)
        {
            if (string.IsNullOrWhiteSpace(emailLogin) || string.IsNullOrWhiteSpace(emailPassword))
            {
                return false;
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.mail.ru",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailLogin, emailPassword)
            };

            using (var email =
                new MailMessage(AddressFrom, message.AddressTo)
                {
                    From = new MailAddress(emailFrom),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = message.IsBodyInHtml,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                })
            {
                try
                {
                    smtp.Send(email);
                    logger.Info($"Sent email to: {email.To} with subject: {email.Subject}");
                    return true;
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to send email with error: {ex.Message}. Mail to: {email.To} with subject: {email.Subject}");
                    return false;
                }
            }
        }
    }
}