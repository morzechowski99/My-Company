using My_Company.Interfaces;
using My_Company.Models.InPostModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services
{
    public class ParcelLockersService : IParcelLockersService
    {
        private readonly RestClient restClient;

        public ParcelLockersService()
        {
            restClient = new RestClient("https://api-shipx-pl.easypack24.net/v1/");
        }

        public async Task<ParcelLockerInfo> GetParcelLockerInfo(string code)
        {
            var request = new RestRequest($"points/{code}", DataFormat.Json);
            var info = await restClient.GetAsync<ParcelLockerInfo>(request);

            return info;
        }
    }
}
