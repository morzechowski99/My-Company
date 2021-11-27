using System.Threading.Tasks;

namespace My_Company.Jobs.EmialSenderJob
{
    public interface IEmailSenderJob
    {
        Task SendEmails();
    }
}
