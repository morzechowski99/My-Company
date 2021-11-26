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
        }

        public const string ImagePlaceholder = "/img/photoPlaceholder.png";
        public const string CART_COOKIE = "cart";
        public const string AVAILABLE_PICKING_METHODS = "pickingMethods";
        public const string AVAILABLE_PAYMENT_METHODS = "paymentMethods";

        public static class ConfigKeys
        {
            public const string Description = "description";
            public const string Keywords = "keywords";
            public const string Title = "title";
            public const string CartSubtitle = "cartSubtitle";
        }
    }
}
