using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BookStore.Data.DTO
{
    public class ApplicationRoleDTO 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string ConcurrencyStamp { get; set; }

        public List<ApplicationUserDTO> ApplicationUsers { get; set; }
    }
}
