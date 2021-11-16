using My_Company.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class Config : IConfig
    {
        private Dictionary<string, string> configDictionary;

        public Config()
        {
            configDictionary = null;
        }

        public async Task<string> GetValue(string key, IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            return configDictionary[key];
        }

        private async Task Setup(IConfigRepository configRepository)
        {
            configDictionary = await configRepository.GetValues();
        }

        public async Task SetValue(string key, string value, IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            await configRepository.SetValue(key, value);
            configDictionary[key] = value;
        }
    }
}
