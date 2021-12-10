using My_Company.Models;
using System.IO;
using System.Threading.Tasks;


namespace My_Company.Services.DocumentGeneratorService
{
    public interface IDocumentGenerator
    {
        Task<Stream> GetInvoice(Order order);
        Task<Stream> GetWZDocument(Order order);
        Task<Stream> GetDeliveryDocument(Delivery delivery);
        Task<Stream> GetDeliveryCorrecingDocument(Delivery correcting, Delivery corrected);
    }
}
