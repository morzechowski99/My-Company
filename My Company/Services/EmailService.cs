using My_Company.EnumTypes;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailQueue emailQueue;

        public EmailService(IEmailQueue emailQueue)
        {
            this.emailQueue = emailQueue;
        }

        public void SendOrderEmail(EmailReason reason, Order order, string email = null)
        {
            if (order.Email == null && email == null)
                throw new ArgumentNullException("email", "order email is null and email is null");

            emailQueue.AddEmailToQueue(new EmailQueueItem
            {
                Content = reason.GetEmailContent(order),
                Title = reason.GetEmailTitle(order),
                To = email == null ? order.Email : email
            });
        }
    }
}
