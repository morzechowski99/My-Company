//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly int port;
        private readonly string pass;
        private readonly string smpt;
        private readonly string userAdress;

        public EmailSender(IConfiguration config)
        {
            userAdress = config.GetValue<string>("SmtpServers:login");
            port = config.GetValue<int>("SmtpServers:port");
            pass = config.GetValue<string>("SmtpServers:password");
            smpt = config.GetValue<string>("SmtpServers:host");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(userAdress));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await client.ConnectAsync(smpt, port, SecureSocketOptions.Auto);
            await client.AuthenticateAsync(userAdress, pass);
            await client.SendAsync(message);
            client.Disconnect(true);
        }
    }
}
