using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        [Display(Name = "Reg. No")]
        public string RegistrationNo { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<VehicleRequisition> VehicleRequisition { get; set; }

        [Display(Name = "Seat Allocated")]
        public int SeatAllocated { get; set; }

        public string Type { get; set; }

        public string Available { get; set; }
    }
}