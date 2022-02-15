using Microsoft.Extensions.DependencyInjection;
using My_Company.EnumTypes;
using My_Company.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace My_Company.Extensions
{
    public static class PaymentServiceExtensions
    {
        public static IPaymentService GetService(this PaymentMethodEnum type, IServiceProvider serviceProvider)
        {
            //pobieranie wsyzstkich serwisów implementujacych interfejs strategii i konwersja do slownika, gdzie klucz to wartosc emuma
            Dictionary<PaymentMethodEnum, Type> strategytypes = Assembly
                .GetExecutingAssembly()
                .GetExportedTypes()
                .Where(type => type.GetInterfaces().Contains(typeof(IPaymentService)))
                .ToDictionary(type => type.GetCustomAttribute<PaymentTypeAttribute>().PaymentMethod);

            //wybieramy konkretny typ
            Type choosenServiceType = strategytypes[type];

            //tworzenie 
            IPaymentService strategy = ActivatorUtilities.CreateInstance(serviceProvider, choosenServiceType) as IPaymentService;

            if (strategy is null)
            {
                throw new InvalidOperationException("Strategy is not in correctFormat");
            }

            return strategy;
        }
    }
}
