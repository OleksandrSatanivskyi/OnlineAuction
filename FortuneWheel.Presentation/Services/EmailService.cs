﻿using System.Net;
using System.Net.Mail;

namespace OnlineAuc.Services
{
    public class EmailService
    {
        private SmtpClient SmtpClient { get; set; }
        private MailAddress FromAddress { get; set; }

        public EmailService(IConfiguration configuration)
        {
            FromAddress = new MailAddress(configuration["Email:Login"], "[ServiceName]");
            SmtpClient = new SmtpClient();
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.Credentials = new NetworkCredential(FromAddress.Address, configuration["Email:Password"]);
        }

        public async Task SendEmail(string messageBody, string toEmail, string subject)
        {
            var mailMessage = new MailMessage
            {
                From = FromAddress,
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            SmtpClient.Send(mailMessage);
        }
    }
}
