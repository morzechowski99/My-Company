using My_Company.EnumTypes;
using My_Company.Services.DeliveryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace My_Company.Extensions
{
    public static class DeliveryServiceExtensions
    {
        public static IDeliveryService GetService(this DeliveryType type)
        {
            //pobieranie wsyzstkich co implementuja interfejs strategii i rpzekonwertowanie do slownika klucz wartosc gdzie klucz to wartosc emuma
            Dictionary<DeliveryType, Type> strategytypes = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IDeliveryService)))
                .ToDictionary(type => type.GetCustomAttribute<DeliveryTypeAttribute>().DeliveryType);

            //wybieramy konkretny typ
            Type chosenStrategy = strategytypes[type];

            //tworzymy obiekt za pomoca Reflection
            IDeliveryService strategy = Activator.CreateInstance(chosenStrategy) as IDeliveryService;

            if (strategy is null)
            {
                throw new InvalidOperationException("Strategy is not in correctFormat");
            }

            return strategy;
        }
    }
}
