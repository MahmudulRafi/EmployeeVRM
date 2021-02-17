using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeVRM.Models;
using Microsoft.AspNet.Identity;

namespace EmployeeVRM.Controllers
{
    public class MaintainanceRequisitionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MaintainanceRequisitions
        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult MainatinanceRFQPending()
        {
            this.CountView();
            return View();
        }
        public ActionResult GetAllMaintainanceRequisition()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var maintainanceRequisitions = from MaintainanceRequisition in db.MaintainanceRequisitions.AsEnumerable()
                                           join Vehicle in db.Vehicles.AsEnumerable() on MaintainanceRequisition.VehicleID equals
                                               Vehicle.VehicleID
                                           join Employee in db.Employees.AsEnumerable() on MaintainanceRequisition.DriverID equals Employee.EmployeeID
                                           join Supplier in db.Suppliers.AsEnumerable() on MaintainanceRequisition.Supplierid equals Supplier.SupplierID
                                           where MaintainanceRequisition.WorkDoneStatus == "RFQPending"
                                           select new
                                           {
                                               id = MaintainanceRequisition.id,
                                               VehicleID = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                               DriverID = Employee.Name,
                                               ReportingDateTime = MaintainanceRequisition.ReportingDateTime.ToString(),
                                               Cause = MaintainanceRequisition.Cause,
                                               PartsCosting = MaintainanceRequisition.PartsCost,
                                               Supplier = Supplier.CompanyName,
                                               EstimatedDelivery = MaintainanceRequisition.EstimitedDeliveryDate,

                                           };

            return Json(new { data = maintainanceRequisitions }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovalFormPartial(int? id)
        {
            MaintainanceRequisition maintainanceRequisition = db.MaintainanceRequisitions.Find(id);

            ViewBag.DriverID = new SelectList(db.Employees, "EmployeeID", "Name", maintainanceRequisition.DriverID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistationNo", maintainanceRequisition.VehicleID);
            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierID", "CompanyName", maintainanceRequisition.Supplierid);
            if (maintainanceRequisition.VerifiedBy != 0)
            {
                var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == maintainanceRequisition.VerifiedBy);
                ViewBag.verifierName = fatchVerifier.Name;
                ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
                ViewBag.veirifierDepartment = fatchVerifier.department.Name;
            }

            return PartialView("_RFQApproval", maintainanceRequisition);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaintainanceRFQApproved(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {
                maintainanceRequisition.ApprovalStatus = "Approved";
                maintainanceRequisition.ApprovalDateTime = DateTime.Now.ToString();
                maintainanceRequisition.WorkDoneStatus = "OnGoing";

                db.Entry(maintainanceRequisition).State = EntityState.Modified;

                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId).EmployeeID;
                maintainanceRequisition.ApprovedBy = getEmployee;
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaintainanceRFQRejected(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {

                maintainanceRequisition.ApprovalStatus = "Rejected";
                maintainanceRequisition.ApprovalDateTime = DateTime.Now.ToString();
                maintainanceRequisition.WorkDoneStatus = "Rejected";

                var vehicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == maintainanceRequisition.VehicleID);
                if (vehicleAvailability == null)
                    return HttpNotFound("Vehicle is not Valid");
                vehicleAvailability.Available = "Yes";

                db.Entry(maintainanceRequisition).State = EntityState.Modified;



                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId).EmployeeID;
                maintainanceRequisition.ApprovedBy = getEmployee;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DetailsPartial(int? id)
        {
           
            MaintainanceRequisition maintainanceRequisition = db.MaintainanceRequisitions.Find(id);

            var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == maintainanceRequisition.VerifiedBy);
            if (fatchVerifier != null)
            {
                ViewBag.verifierName = fatchVerifier.Name;
                ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
                ViewBag.veirifierDepartment = fatchVerifier.department.Name;
            }
            var fatchApprover = db.Employees.FirstOrDefault(emp => emp.EmployeeID == maintainanceRequisition.ApprovedBy);
            if (fatchApprover != null)
            {
                ViewBag.approverName = fatchApprover.Name;
                ViewBag.approverDesignation = fatchApprover.designation.Name;
                ViewBag.approverDepartment = fatchApprover.department.Name;
                ViewBag.comma = " , ";
            }

            return PartialView("_Details", maintainanceRequisition);
        }


        public ActionResult CreatePartial()
        {
            var vehicleDriver = db.Employees.Where(emp => emp.designation.Name == "Driver")
                 .Select(driver => new
                 {
                     Text = driver.Name,
                     Value = driver.EmployeeID
                 }).ToList();

            ViewBag.VehicleDriver = new SelectList(vehicleDriver, "Value", "Text");

            var vehicleSelection = db.Vehicles.Where(v => v.Available == "Yes")
                             .Select(vehicle => new
                             {
                                 Text = vehicle.RegistrationNo + " - " + vehicle.Name + " " + vehicle.Type + "",
                                 Value = vehicle.VehicleID
                             }).ToList();

            ViewBag.VehicleList = new SelectList(vehicleSelection, "Value", "Text");

            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierID", "CompanyName");


            var maintainanceReq = new MaintainanceRequisition();
            return PartialView("_Create", maintainanceReq);
        }

        public ActionResult Create(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {
                maintainanceRequisition.ReportingDateTime = DateTime.Now;
                maintainanceRequisition.VerificationStatus = "Verified";

                var getUser = User.Identity.GetUserId();
                var getEmployeeId = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUser);
                maintainanceRequisition.VerifiedBy = getEmployeeId.EmployeeID;

                maintainanceRequisition.WorkDoneStatus = "RFQPending";
                maintainanceRequisition.ApprovalStatus = "Pending";


                db.MaintainanceRequisitions.Add(maintainanceRequisition);

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == maintainanceRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "No";

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditPartial(int? id)
        {
            var vehicleDriver = db.Employees
                 .Select(driver => new
                 {
                     Text = driver.Name,
                     Value = driver.EmployeeID
                 }).ToList();

            ViewBag.VehicleDriver = new SelectList(vehicleDriver, "Value", "Text");

            var vehicleSelection = db.Vehicles
                             .Select(vehicle => new
                             {
                                 Text = vehicle.RegistrationNo + " - " + vehicle.Name + " " + vehicle.Type + "",
                                 Value = vehicle.VehicleID
                             }).ToList();

            ViewBag.VehicleList = new SelectList(vehicleSelection, "Value", "Text");

            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierID", "CompanyName");


            var maintainanceReq = db.MaintainanceRequisitions.Find(id);
            return PartialView("_Edit", maintainanceReq);

        }

        public ActionResult Edit(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {
                maintainanceRequisition.VerificationStatus = "Verified";

                db.Entry(maintainanceRequisition).State = EntityState.Modified;
                db.Entry(maintainanceRequisition).Property("ReportingDateTime").IsModified = false;
                db.Entry(maintainanceRequisition).Property("VerifiedBy").IsModified = false;
                db.Entry(maintainanceRequisition).Property("WorkDoneStatus").IsModified = false;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // GET: MaintainanceRequisitions/Delete/5
        public ActionResult DeletePartial(int? id)
        {


            MaintainanceRequisition maintainanceRequisition = db.MaintainanceRequisitions.Find(id);

            return PartialView("_Delete", maintainanceRequisition);
        }


        public ActionResult Delete(int id)
        {
            var maintainanceRequisition = db.MaintainanceRequisitions.Find(id);

            var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == maintainanceRequisition.VehicleID);
            if (vahicleAvailability == null)
                return HttpNotFound("Vahicle is not valid");
            vahicleAvailability.Available = "Yes";

            db.MaintainanceRequisitions.Remove(maintainanceRequisition);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult OnGoingAllMaintainance()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllOnGoingMaintainanceRequisition()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var maintainanceRequisitions = from MaintainanceRequisition in db.MaintainanceRequisitions.AsEnumerable()
                                           join Vehicle in db.Vehicles.AsEnumerable() on MaintainanceRequisition.VehicleID equals
                                               Vehicle.VehicleID
                                           join Employee in db.Employees.AsEnumerable() on MaintainanceRequisition.DriverID equals Employee.EmployeeID
                                           join Supplier in db.Suppliers.AsEnumerable() on MaintainanceRequisition.Supplierid equals Supplier.SupplierID
                                           where MaintainanceRequisition.WorkDoneStatus == "OnGoing"
                                           select new
                                           {
                                               id = MaintainanceRequisition.id,
                                               VehicleID = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                               DriverID = Employee.Name,
                                               ReportingDateTime = MaintainanceRequisition.ReportingDateTime.ToString(),
                                               Cause = MaintainanceRequisition.Cause,
                                               PartsCosting = MaintainanceRequisition.PartsCost,
                                               Supplier = Supplier.CompanyName,
                                               EstimatedDelivery = MaintainanceRequisition.EstimitedDeliveryDate,

                                           };

            return Json(new { data = maintainanceRequisitions }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelOrCompleteFormView(int? id)
        {
            MaintainanceRequisition maintainanceRequisition = db.MaintainanceRequisitions.Find(id);

            ViewBag.DriverID = new SelectList(db.Employees, "EmployeeID", "Name", maintainanceRequisition.DriverID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "RegistationNo", maintainanceRequisition.VehicleID);
            ViewBag.Supplier = new SelectList(db.Suppliers, "SupplierID", "CompanyName", maintainanceRequisition.Supplierid);

            var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == maintainanceRequisition.VerifiedBy);
            if (fatchVerifier != null)
            {
                ViewBag.verifierName = fatchVerifier.Name;
                ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
                ViewBag.veirifierDepartment = fatchVerifier.department.Name;
            }
            var fatchApprover = db.Employees.FirstOrDefault(emp => emp.EmployeeID == maintainanceRequisition.ApprovedBy);
            if (fatchApprover != null)
            {
                ViewBag.approverName = fatchApprover.Name;
                ViewBag.approverDesignation = fatchApprover.designation.Name;
                ViewBag.approverDepartment = fatchApprover.department.Name;
            }

            return PartialView("_CancelOrComplete", maintainanceRequisition);
        }

        public ActionResult CompletedOnGoingMaintainance(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == maintainanceRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "Yes";

                maintainanceRequisition.WorkDoneStatus = "Completed";

                db.Entry(maintainanceRequisition).State = EntityState.Modified;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CancelledOnGoingMaintainance(MaintainanceRequisition maintainanceRequisition)
        {
            if (ModelState.IsValid)
            {

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == maintainanceRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "Yes";

                maintainanceRequisition.WorkDoneStatus = "Cancelled";

                db.Entry(maintainanceRequisition).State = EntityState.Modified;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllCompletedMaintainance()
        {
            this.CountView();
            var allCompletedMaintainances = db.MaintainanceRequisitions.Where(vr => vr.WorkDoneStatus == "Completed");

            return View(allCompletedMaintainances.ToList());
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
