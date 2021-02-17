using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class VehicleRequisition
    {
        public int id { get; set; }

        [Display(Name = "Reporting Date/Time")]
        public DateTime ReportingDateTime { get; set; }

        [Display(Name = "Employee")]
        public int EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]

        public virtual Employee Employee { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        [Required]
        [Display(Name = "Cause")]
        public string Cause { get; set; }

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleID { get; set; }
        [ForeignKey("VehicleID")]

        public virtual Vehicle Vehicle { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Verification")]
        public string VerificationStatus { get; set; }

        [Display(Name = "Verification Date/Time")]
        public string VerificationDateTime { get; set; }

        [Display(Name = "Verified By")]
        public int VerifiedBy { get; set; }

        [Display(Name = "Approval")]
        public string ApprovalStatus { get; set; }

        [Display(Name = "Approval Date/Time")]
        public string ApprovalDateTime { get; set; }

        [Display(Name = "Approved By")]
        public int ApprovedBy { get; set; }

        public string Status { get; set; }

    }
}