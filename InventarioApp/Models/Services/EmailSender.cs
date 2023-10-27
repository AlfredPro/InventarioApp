using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace InventarioApp.Models.Services
{
    public class EmailSender : IEmailSender
    {

        private readonly IConfiguration Configuration;

        public EmailSender(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string? fromMail = Configuration.GetValue<string>("SMTPSender:UserName");
            string? fromPassword = Configuration.GetValue<string>("SMTPSender:Password");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(Configuration.GetValue<string>("SMTPSender:Host"))
            {
                Port = Configuration.GetValue<int>("SMTPSender:Port"),
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = Configuration.GetValue<bool>("SMTPSender:EnableSSL"),
            };
            await smtpClient.SendMailAsync(message);
        }

    }
}
