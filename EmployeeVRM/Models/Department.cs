using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class Department
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees
        {
            get; set;
        }
    }
}