using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string ApplicationEmpID { get; set; }
        [ForeignKey("ApplicationEmpID")]

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Date of Joining")]
        public string JoiningDate { get; set; }

        [Display(Name = "Designation")]
        public int DesignationID { get; set; }

        [ForeignKey("DesignationID")]

        public virtual Designation designation { get; set; }

        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]

        public virtual Department department { get; set; }

        [StringLength(11, ErrorMessage = " mobile number must have 11 digits")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        public string Address { get; set; }
        public string NID { get; set; }

      
    }
}
