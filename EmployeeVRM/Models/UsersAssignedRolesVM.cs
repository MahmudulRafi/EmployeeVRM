using EmployeeVRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class UsersAssignedRolesVM
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<string> RoleNames { get; set; }
    }
}