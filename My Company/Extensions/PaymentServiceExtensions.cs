using Microsoft.Extensions.DependencyInjection;
using My_Company.EnumTypes;
using My_Company.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace My_Company.Extensions
{
    public static class PaymentServiceExtensions
    {
        public static IPaymentService GetService(this PaymentMethodEnum type, IServiceProvider serviceProvider)
        {
            //pobieranie wsyzstkich co implementuja interfejs strategii i rpzekonwertowanie do slownika klucz wartosc gdzie klucz to wartosc emuma
            Dictionary<PaymentMethodEnum, Type> strategytypes = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IPaymentService)))
                .ToDictionary(type => type.GetCustomAttribute<PaymentTypeAttribute>().PaymentMethod);

            //wybieramy konkretny typ
            Type choosenServiceType = strategytypes[type];

            //tworzymy obiekt za pomoca Reflection
            IPaymentService strategy = ActivatorUtilities.CreateInstance(serviceProvider,choosenServiceType) as IPaymentService;

            if (strategy is null)
            {
                throw new InvalidOperationException("Strategy is not in correctFormat");
            }

            return strategy;
        }
    }
}
