using My_Company.EnumTypes;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Text.Encodings.Web;

namespace My_Company.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailQueue emailQueue;

        public EmailService(IEmailQueue emailQueue)
        {
            this.emailQueue = emailQueue;
        }

        public void SendEmail(string email, string title, string content)
        {
            emailQueue.AddEmailToQueue(new EmailQueueItem
            {
                Content = content,
                Title = title,
                To = email
            });
        }

        public void SendOrderEmail(OrderEmailReason reason, Order order, string url, string email = null)
        {
            if (order.Email == null && email == null)
                throw new ArgumentNullException("email", "order email is null and email is null");

            emailQueue.AddEmailToQueue(new EmailQueueItem
            {
                Content = reason.GetEmailContent(order, url),
                Title = reason.GetEmailTitle(order),
                To = email == null ? order.Email : email
            });
        }

        public void SendRegistrationEmail(string email, string url)
        {
            emailQueue.AddEmailToQueue(new EmailQueueItem
            {
                Content = $"Dziękujemy za rejestrację w naszym sklepie" +
                $"Potwierdź swoje konto klikając w <a href='{HtmlEncoder.Default.Encode(url)}'>link</a>.",
                Title = "Potwierdzenie rejestracji",
                To = email
            });
        }
    }
}
