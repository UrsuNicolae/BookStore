using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookStore.Data.DataModels
{
    public class ApplicationRole: IdentityRole<int>
    {
        public ApplicationRole(string name):base (name)
        {
        }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
