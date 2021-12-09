using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Helpers
{
    public static class Constants
    {
        public static class Roles
        {
            public const string MainAdministrator = "Main Administrator";
            public const string WarehouseEmployee = "Warehouse Employee";
            public const string ShopUser = "ShopUser";
        }

        public static class AuthorizationPolicies
        {
            public const string WarehousePolicy = "WarehousePolicy";
            public const string ShopAccountPolicy = "ShopAccountPolicy";
        }

        public const string ImagePlaceholder = "/img/photoPlaceholder.png";
        public const string CART_COOKIE = "cart";
        public const string AVAILABLE_PICKING_METHODS = "pickingMethods";
        public const string AVAILABLE_PAYMENT_METHODS = "paymentMethods";
        public const string DOT_PAY_IPS = "dotPayIpAddresses";

        public static class ConfigKeys
        {
            public const string Description = "description";
            public const string Keywords = "keywords";
            public const string Title = "title";
            public const string CartSubtitle = "cartSubtitle";
            public const string OrderConfirmText = "orderConfirmText";
            public const string DataToPayment = "dataToPayment";
            public const string IsShopEnabled = "shopEnabled";
            public const string LogoPath = "logoPath";
            public const string PersonalPickupAddress = "personalPickupAddress";
            public const string DocumentAddress = "documentAddress";

            public static class DotPayKeys
            {
                public const string Id = "dotPayId";
                public const string Pin = "dotPayPIN";

            }
        }
    }
}
