using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Concurrent;

namespace My_Company.Services
{
    public class EmailQueue : IEmailQueue
    {
        private readonly ConcurrentQueue<EmailQueueItem> emailQueue;

        public EmailQueue()
        {
            emailQueue = new();
        }

        public void AddEmailToQueue(EmailQueueItem email)
        {
            emailQueue.Enqueue(email);
        }

        public EmailQueueItem GetItem()
        {
            EmailQueueItem email = null;
            emailQueue.TryDequeue(out email);
            return email;
        }
    }
}
