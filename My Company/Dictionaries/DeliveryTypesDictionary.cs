//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.Collections.Generic;

namespace My_Company.Dictionaries
{
    public static class DeliveryTypesDictionary
    {
        private static Dictionary<DeliveryType, string> deliveryTypesDictionary = new()
        {
            { DeliveryType.PersonalPickup, "Odbiór osobisty" },
            { DeliveryType.PaczkomatyInPost, "Paczkomaty InPost" },
        };
        public static Dictionary<DeliveryType, string> Dictionary { get { return deliveryTypesDictionary; } }
    }
}
