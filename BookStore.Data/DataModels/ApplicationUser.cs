using Microsoft.AspNetCore.Identity;

namespace BookStore.Data.DataModels
{
    public class ApplicationUser:IdentityUser<int>
    {

        public string StreetAdress { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public int? CompanyId { get; set; }

        public Company Company { get; set; }

        public int AppliactionRoleId { get; set; }

        public ApplicationRole ApplicationRole { get; set; }


    }
}
