using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class UserVM
    {

        public string Name { get; set; }

        public string Role { get; set; }

        public IEnumerable<SelectListItem> CompanyList { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public int? CompanyId { get; set; }

        public int ApplicationRoleId { get; set; }
    }
}
