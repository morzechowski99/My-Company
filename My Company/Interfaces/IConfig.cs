using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IConfig
    {
        string GetValue(string key);
        Task SetValue(string key, string value);
    }
}
