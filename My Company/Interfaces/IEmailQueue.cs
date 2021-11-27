using My_Company.Models;

namespace My_Company.Interfaces
{
    public interface IEmailQueue
    {
        EmailQueueItem GetItem();
        void AddEmailToQueue(EmailQueueItem email);
    }
}
