using My_Company.Models.InPostModels;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IParcelLockersService
    {
        Task<ParcelLockerInfo> GetParcelLockerInfo(string code);
    }
}
