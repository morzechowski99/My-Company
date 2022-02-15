//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IPhotosRepository : IRepositoryBase<Photo>
    {
        Task<List<Photo>> GetPhotosByProduct(int productId);
    }
}
