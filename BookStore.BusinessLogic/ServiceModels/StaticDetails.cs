using System;

namespace BookStore.BusinessLogic.ServiceModels
{
    public static class StaticDetails
    {
        public const string Role_IndiUser = "Individual Customer";
        public const string Role_CompUser = "Company User";
        public const string Role_AdminUser = "Admin";

        public const string ssShoppingItem = "Shopping item sesion";

        public static double GetPrice(int count, double price)
        {
            if (count > 49)
            {
                if (count > 99) return Math.Round(price * 0.8, MidpointRounding.ToEven);
                else return Math.Round(price * 0.9, MidpointRounding.ToEven);
            }
            else return price;
        }
    }
}
