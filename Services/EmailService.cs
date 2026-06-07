using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GoogleClassroom.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GoogleClassroom.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var host = emailSettings["SmtpHost"]!;
            var port = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var username = emailSettings["SmtpUsername"]!;
            var password = emailSettings["SmtpPassword"]!;
            var senderEmail = emailSettings["SenderEmail"] ?? "no-reply@localhost";
            var senderName = emailSettings["SenderName"];

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
