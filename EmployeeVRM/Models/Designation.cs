using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class Designation
    {
        public int id { get; set; }

        [Display(Name = "Designation")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees
        {
            get; set;
        }
    }
}