using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeVRM.Models
{
    public class MaintainanceRequisition
    {

        public int id { get; set; }

        [Display(Name ="Vehicle")]
        public int VehicleID { get; set; }
        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }

        [Display(Name = "Driver")]
        public int DriverID { get; set; }
        [ForeignKey("DriverID")]

        public virtual Employee Employee { get; set; }

        [Display(Name = "Reporting Date Time")]
        public DateTime ReportingDateTime { get; set; }

        [Display(Name = "Issue")]
        public string Cause { get; set; }
        public string Remarks { get; set; }

        [Display(Name = "Verification Status")]
        public string VerificationStatus { get; set; }

        [Display(Name = "Verified By")]
        public int VerifiedBy { get; set; }

        [Display(Name = "Approval Status")]
        public string ApprovalStatus { get; set; }

        [Display(Name = "Approval Date Time")]
        public string ApprovalDateTime { get; set; }

        [Display(Name = "Approved By")]
        public int ApprovedBy { get; set; }

        [Display(Name = "Parts Needed")]
        public string PartsNeeded { get; set; }

        [Display(Name = "Parts Cost")]
        public double PartsCost { get; set; }

        [Display(Name = "Servicing Cost")]
        public double ServiceCosting { get; set; }

        [Display(Name = "Estimited Delivery Date")]
        public string EstimitedDeliveryDate { get; set; }

        public string WorkDoneStatus { get; set; }

        public int Supplierid { get; set; }
        [ForeignKey("Supplierid")]
  
        public virtual Supplier Supplier { get; set; }
    }
}