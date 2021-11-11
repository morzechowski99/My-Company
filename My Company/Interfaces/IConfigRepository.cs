using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IConfigRepository
    {
        Task<Dictionary<string, string>> GetValues();
        Task SetValue(string key, string value);
    }
}
