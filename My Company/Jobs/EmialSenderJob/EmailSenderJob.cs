//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Identity.UI.Services;
using My_Company.Interfaces;
using System;
using System.Threading.Tasks;

namespace My_Company.Jobs.EmialSenderJob
{
    public class EmailSenderJob : IEmailSenderJob
    {
        private readonly IEmailQueue queue;
        private readonly IEmailSender emailSender;

        public EmailSenderJob(IEmailQueue queue, IEmailSender emailSender)
        {
            this.queue = queue;
            this.emailSender = emailSender;
        }

        public async Task SendEmails()
        {
            for (int i = 0; i < 10; i++)
            {
                var email = queue.GetItem();
                if (email == null)
                    break;
                try
                {
                    await emailSender.SendEmailAsync(email.To, email.Title, email.Content);
                }
                catch (Exception)
                {
                    queue.AddEmailToQueue(email);
                    throw;
                }
            }
        }
    }
}
