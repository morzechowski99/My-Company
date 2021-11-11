using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class Config : IConfig
    {
        private IConfigRepository configRepository;
        private Dictionary<string, string> configDictionary;

        public Config(IConfigRepository configRepository)
        {
            this.configRepository = configRepository;
            configDictionary = configRepository.GetValues().GetAwaiter().GetResult();
        }

        public string GetValue(string key)
        {
            return configDictionary[key];
        }

        public async Task SetValue(string key, string value)
        {
            await configRepository.SetValue(key, value);
            configDictionary[key] = value;
        }
    }
}
