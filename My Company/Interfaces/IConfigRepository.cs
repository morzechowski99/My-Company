//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IConfigRepository : IRepositoryBase<Config>
    {
        Task<Dictionary<string, string>> GetValues();
        Task SetValue(string key, string value);
    }
}
