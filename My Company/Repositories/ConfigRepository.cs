using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class ConfigRepository : RepositoryBase<Config>, IConfigRepository
    {
        public ConfigRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Dictionary<string, string>> GetValues()
        {
            return await FindAll().ToDictionaryAsync(c => c.Id, c => c.Value);
        }

        public async Task SetValue(string key, string value)
        {
            var item = new Config { Id = key, Value = value };
            Update(item);
            await Context.SaveChangesAsync();
        }
    }
}
