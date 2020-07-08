using System.Collections.Generic;

namespace BookStore.Data.DataModels
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StreetAdress { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
