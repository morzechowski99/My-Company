using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Dictionaries
{
    public static class ConfigKeysDictrionary
    {
        private static Dictionary<string, string> configKeysDictrionary = new Dictionary<string, string>() {
            {ConfigKeys.Title,"Nazwa sklepu" },
            {ConfigKeys.Description,"Opis sklepu" },
            {ConfigKeys.Keywords,"Słowa kluczowe" },
            {ConfigKeys.CartSubtitle,"Tekst na widoku koszyka" },
            {ConfigKeys.OrderConfirmText,"Tekst na stronie potwierdzenia zamówienia" },
        };

        public static Dictionary<string, string> ConfigDictionary { get { return configKeysDictrionary; } }
    }
}
