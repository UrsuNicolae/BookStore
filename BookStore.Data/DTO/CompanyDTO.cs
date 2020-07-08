using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StreetAdress { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public List<ApplicationUserDTO> ApplicationUsers { get; set; }
    }
}
