using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IConfig
    {
        Task<string> GetValue(string key, IConfigRepository configRepository);
        Task SetValue(string key, string value, IConfigRepository configRepository);
    }
}
