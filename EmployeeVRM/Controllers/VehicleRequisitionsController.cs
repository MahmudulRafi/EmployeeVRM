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
    public class VehicleRequisitionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetEmployeeVehicleRequisition()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employeeId = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId).EmployeeID;

            var vehicleRequisitions = from VehicleRequisition in db.VehicleRequisitions.AsEnumerable()
                                      join Vehicle in db.Vehicles.AsEnumerable() on VehicleRequisition.VehicleID equals
                                          Vehicle.VehicleID
                                      join Employee in db.Employees.AsEnumerable() on VehicleRequisition.EmployeeID equals Employee.EmployeeID
                                      where Employee.EmployeeID == employeeId
                                      where VehicleRequisition.Status == "Pending"

                                      select new
                                      {
                                          id = VehicleRequisition.id,
                                          Vehicle = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                          StartDate = VehicleRequisition.StartDate,
                                          EndDate = VehicleRequisition.EndDate,
                                          VerificationStatus = VehicleRequisition.VerificationStatus,
                                          ReportingDateTime = VehicleRequisition.ReportingDateTime.ToString(),
                                      };

            return Json(new { data = vehicleRequisitions }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingVerification()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetPendingVerifications()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);

            var getPendingVehicleRequisition = from VehicleRequisition in db.VehicleRequisitions.AsEnumerable()
                                               join Vehicle in db.Vehicles.AsEnumerable() on VehicleRequisition.VehicleID equals
                                                   Vehicle.VehicleID
                                               join Employee in db.Employees.AsEnumerable() on VehicleRequisition.EmployeeID equals Employee.EmployeeID
                                               join Department in db.Departments.AsEnumerable() on VehicleRequisition.Employee.DepartmentID equals Department.id
                                               join Designation in db.Designations.AsEnumerable() on VehicleRequisition.Employee.DesignationID equals Designation.id
                                               where VehicleRequisition.Status == "Pending"

                                               select new
                                               {
                                                   id = VehicleRequisition.id,
                                                   Employee = Employee.Name + "<br/>" + Designation.Name + " , " + Department.Name,
                                                   Vehicle = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                                   StartDate = VehicleRequisition.StartDate,
                                                   EndDate = VehicleRequisition.EndDate,
                                                   VerificationStatus = VehicleRequisition.VerificationStatus,
                                                   ReportingDateTime = VehicleRequisition.ReportingDateTime.ToString(),
                                               };
            return Json(new { data = getPendingVehicleRequisition }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingApproval()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetPendingApprovals()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);

            var getPendingVehicleRequisition = from VehicleRequisition in db.VehicleRequisitions.AsEnumerable()
                                               join Vehicle in db.Vehicles.AsEnumerable() on VehicleRequisition.VehicleID equals
                                                   Vehicle.VehicleID
                                               join Employee in db.Employees.AsEnumerable() on VehicleRequisition.EmployeeID equals Employee.EmployeeID
                                               join Department in db.Departments.AsEnumerable() on VehicleRequisition.Employee.DepartmentID equals Department.id
                                               join Designation in db.Designations.AsEnumerable() on VehicleRequisition.Employee.DesignationID equals Designation.id
                                               where VehicleRequisition.Status == "Verified"

                                               select new
                                               {
                                                   id = VehicleRequisition.id,
                                                   Employee = Employee.Name + "<br/>" + Designation.Name + " , " + Department.Name,
                                                   Vehicle = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                                   StartDate = VehicleRequisition.StartDate,
                                                   EndDate = VehicleRequisition.EndDate,
                                                   VerificationStatus = VehicleRequisition.VerificationStatus,
                                                   ReportingDateTime = VehicleRequisition.ReportingDateTime.ToString(),

                                               };
            return Json(new { data = getPendingVehicleRequisition }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeOnGoingRquisitions()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);

            var getPendingVehicleRequisition = from VehicleRequisition in db.VehicleRequisitions.AsEnumerable()
                                               join Vehicle in db.Vehicles.AsEnumerable() on VehicleRequisition.VehicleID equals
                                                   Vehicle.VehicleID
                                               join Employee in db.Employees.AsEnumerable() on VehicleRequisition.EmployeeID equals Employee.EmployeeID
                                               where VehicleRequisition.EmployeeID == employee.EmployeeID
                                               where VehicleRequisition.Status == "OnGoing"

                                               select new
                                               {
                                                   id = VehicleRequisition.id,
                                                   Vehicle = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                                   StartDate = VehicleRequisition.StartDate,
                                                   EndDate = VehicleRequisition.EndDate,
                                                   ReportingDateTime = VehicleRequisition.ReportingDateTime.ToString(),

                                               };
            return Json(new { data = getPendingVehicleRequisition }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllOnGoingRquisitions()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);

            var getPendingVehicleRequisition = from VehicleRequisition in db.VehicleRequisitions.AsEnumerable()
                                               join Vehicle in db.Vehicles.AsEnumerable() on VehicleRequisition.VehicleID equals
                                                   Vehicle.VehicleID
                                               join Employee in db.Employees.AsEnumerable() on VehicleRequisition.EmployeeID equals Employee.EmployeeID
                                               join Department in db.Departments.AsEnumerable() on VehicleRequisition.Employee.DepartmentID equals Department.id
                                               join Designation in db.Designations.AsEnumerable() on VehicleRequisition.Employee.DesignationID equals Designation.id

                                               where VehicleRequisition.Status == "OnGoing"

                                               select new
                                               {
                                                   id = VehicleRequisition.id,
                                                   Employee = Employee.Name + "<br/>" + Designation.Name + " , " + Department.Name,
                                                   Vehicle = Vehicle.RegistrationNo + "-" + Vehicle.Name,
                                                   StartDate = VehicleRequisition.StartDate,
                                                   EndDate = VehicleRequisition.EndDate,
                                                   ReportingDateTime = VehicleRequisition.ReportingDateTime.ToString(),

                                               };
            return Json(new { data = getPendingVehicleRequisition }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DetailsPartial(int? id)
        {

            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);

            var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == vehicleRequisition.VerifiedBy);
            if (fatchVerifier != null)
            {
                ViewBag.verifierName = fatchVerifier.Name;
                ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
                ViewBag.comma = " , ";
                ViewBag.veirifierDepartment = fatchVerifier.department.Name;
            }

            var fatchApprover = db.Employees.FirstOrDefault(emp => emp.EmployeeID == vehicleRequisition.ApprovedBy);
            if (fatchApprover != null)
            {
                ViewBag.approverName = fatchApprover.Name;
                ViewBag.approverDesignation = fatchApprover.designation.Name;
                ViewBag.comma = " , ";
                ViewBag.approverDepartment = fatchApprover.department.Name;

            }

            return PartialView("_Details", vehicleRequisition);
        }


        public ActionResult CreatePartial()
        {
            var vehicleSelection = db.Vehicles.Where(v => v.Available == "Yes")
                            .Select(vehicle => new
                            {
                                Text = vehicle.RegistrationNo + " - " + vehicle.Name + " " + vehicle.Type + " [ S.A - " + vehicle.SeatAllocated + " ]",
                                Value = vehicle.VehicleID
                            }).ToList();

            ViewBag.VehicleList = new SelectList(vehicleSelection, "Value", "Text");

            var vehicleRequisition = new VehicleRequisition();
            return PartialView("_Create", vehicleRequisition);
        }


        public ActionResult Create(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {
                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId);
                vehicleRequisition.EmployeeID = getEmployee.EmployeeID;
                vehicleRequisition.ReportingDateTime = DateTime.Now;
                vehicleRequisition.VerificationStatus = "Pending";
                vehicleRequisition.ApprovalStatus = "Pending";
                vehicleRequisition.Status = "Pending";


                db.VehicleRequisitions.Add(vehicleRequisition);

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == vehicleRequisition.VehicleID);
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

            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);

            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);

            var vehicleSelection = db.Vehicles
                        .Select(vehicle => new
                        {
                            Text = vehicle.RegistrationNo + " - " + vehicle.Name + " " + vehicle.Type + " [ S.A - " + vehicle.SeatAllocated + " ]",
                            Value = vehicle.VehicleID
                        }).ToList();

            ViewBag.VehicleList = new SelectList(vehicleSelection, "Value", "Text");
            return PartialView("_Edit", vehicleRequisition);

        }


        public ActionResult Edit(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {

                db.Entry(vehicleRequisition).State = EntityState.Modified;
                db.Entry(vehicleRequisition).Property("ReportingDateTime").IsModified = false;
                db.Entry(vehicleRequisition).Property("VerificationStatus").IsModified = false;
                db.Entry(vehicleRequisition).Property("ApprovalStatus").IsModified = false;
                db.Entry(vehicleRequisition).Property("VerificationDateTime").IsModified = false;
                db.Entry(vehicleRequisition).Property("ApprovalDateTime").IsModified = false;
                db.Entry(vehicleRequisition).Property("VerifiedBy").IsModified = false;
                db.Entry(vehicleRequisition).Property("ApprovedBy").IsModified = false;
                db.Entry(vehicleRequisition).Property("Status").IsModified = false;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DeletePartial(int? id)
        {
            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);
            return PartialView("_Delete", vehicleRequisition);
        }

        public ActionResult Delete(int id)
        {
            var vehicleRequisition = db.VehicleRequisitions.Find(id);
            db.VehicleRequisitions.Remove(vehicleRequisition);

            var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == vehicleRequisition.VehicleID);
            if (vahicleAvailability == null)
                return HttpNotFound("Vahicle is not valid");
            vahicleAvailability.Available = "Yes";

            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult VerificationPartial(int? id)
        {
            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);
            ViewBag.EmployeeID = (db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);

            return PartialView("_Verify", vehicleRequisition);
        }

        public ActionResult RequisitionVerified(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {
                vehicleRequisition.VerificationStatus = "Verified";
                vehicleRequisition.VerificationDateTime = DateTime.Now.ToString();
                vehicleRequisition.Status = "Verified";

                db.Entry(vehicleRequisition).State = EntityState.Modified;
                db.Entry(vehicleRequisition).Property("ApprovalStatus").IsModified = false;
                db.Entry(vehicleRequisition).Property("ApprovalDateTime").IsModified = false;
                db.Entry(vehicleRequisition).Property("ApprovedBy").IsModified = false;

                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId);
                vehicleRequisition.VerifiedBy = getEmployee.EmployeeID;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "Name", vehicleRequisition.VehicleID);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovalPartial(int? id)
        {
            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);
            ViewBag.EmployeeID = (db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);

            var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == vehicleRequisition.VerifiedBy);
            ViewBag.verifierName = fatchVerifier.Name;
            ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
            ViewBag.veirifierDepartment = fatchVerifier.department.Name;

            return PartialView("_Approv", vehicleRequisition);
        }

        public ActionResult RequisitionApproved(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {
                vehicleRequisition.ApprovalStatus = "Approved";
                vehicleRequisition.ApprovalDateTime = DateTime.Now.ToString();
                vehicleRequisition.Status = "OnGoing";

                db.Entry(vehicleRequisition).State = EntityState.Modified;
                db.Entry(vehicleRequisition).Property("VerificationStatus").IsModified = false;
                db.Entry(vehicleRequisition).Property("VerificationDateTime").IsModified = false;
                db.Entry(vehicleRequisition).Property("VerifiedBy").IsModified = false;

                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId);
                vehicleRequisition.ApprovedBy = getEmployee.EmployeeID;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "Name", vehicleRequisition.VehicleID);
            return Json(false, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RequisitionRejected(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == vehicleRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "Yes";

                var employeeUserId = User.Identity.GetUserId();
                var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == employeeUserId);

                if (vehicleRequisition.VerifiedBy == 0)
                {
                    vehicleRequisition.VerificationStatus = "Rejected";
                    vehicleRequisition.VerificationDateTime = DateTime.Now.ToString();
                    vehicleRequisition.Status = "Rejected";
                    vehicleRequisition.VerifiedBy = getEmployee.EmployeeID;
                }
                else
                {
                    vehicleRequisition.ApprovalStatus = "Rejected";
                    vehicleRequisition.ApprovalDateTime = DateTime.Now.ToString();
                    vehicleRequisition.Status = "Rejected";
                    vehicleRequisition.ApprovedBy = getEmployee.EmployeeID;
                }

                db.Entry(vehicleRequisition).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "Name", vehicleRequisition.VehicleID);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OnGoingEmployeeRequisitions()
        {
            this.CountView();
            return View();
        }
        public ActionResult CancelOrCompleteFormView(int? id)
        {
            VehicleRequisition vehicleRequisition = db.VehicleRequisitions.Find(id);
            ViewBag.EmployeeID = (db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);

            var fatchVerifier = db.Employees.FirstOrDefault(emp => emp.EmployeeID == vehicleRequisition.VerifiedBy);
            if (fatchVerifier != null)
            {
                ViewBag.verifierName = fatchVerifier.Name;
                ViewBag.veirifierDesignation = fatchVerifier.designation.Name;
                ViewBag.veirifierDepartment = fatchVerifier.department.Name;
            }

            var fatchApprover = db.Employees.FirstOrDefault(emp => emp.EmployeeID == vehicleRequisition.ApprovedBy);

            if (fatchApprover != null)
            {
                ViewBag.approverName = fatchApprover.Name;
                ViewBag.approverDesignation = fatchApprover.designation.Name;
                ViewBag.approverDepartment = fatchApprover.department.Name;
            }

            return PartialView("_CancelOrComplete", vehicleRequisition);
        }

        public ActionResult CancelOnGoingRequisition(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == vehicleRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "Yes";

                vehicleRequisition.Status = "Cancelled";

                db.Entry(vehicleRequisition).State = EntityState.Modified;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "Name", vehicleRequisition.VehicleID);
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompleteOnGoingRequisition(VehicleRequisition vehicleRequisition)
        {
            if (ModelState.IsValid)
            {

                var vahicleAvailability = db.Vehicles.SingleOrDefault(vehicle => vehicle.VehicleID == vehicleRequisition.VehicleID);
                if (vahicleAvailability == null)
                    return HttpNotFound("Vahicle is not valid");
                vahicleAvailability.Available = "Yes";

                vehicleRequisition.Status = "Completed";

                db.Entry(vehicleRequisition).State = EntityState.Modified;

                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "Name", vehicleRequisition.EmployeeID);
            ViewBag.VehicleID = new SelectList(db.Vehicles, "VehicleID", "Name", vehicleRequisition.VehicleID);
            return Json(false, JsonRequestBehavior.AllowGet);
        }



        public ActionResult OnGoingDepartmentRequisitions()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);
            var onGoingDepartmentRequisition = db.VehicleRequisitions.Where(v => v.Employee.DepartmentID == getEmployee.DepartmentID).Include(v => v.Vehicle).Where(vr => vr.Status == "OnGoing");

            return View(onGoingDepartmentRequisition.ToList());
        }

        public ActionResult OnGoingAllRequisitions()
        {
            this.CountView();
            return View();
        }


        public ActionResult EmployeeCompletedRequisitions()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);
            var employeeCompletedRequisition = db.VehicleRequisitions.Where(v => v.Employee.EmployeeID == getEmployee.EmployeeID).Include(v => v.Vehicle).Where(vr => vr.Status == "Completed");

            return View(employeeCompletedRequisition.ToList());
        }
        public ActionResult EmployeeFailedRequisitions()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);
            var employeeFailedRequisition = db.VehicleRequisitions.Where(v => v.Employee.EmployeeID == getEmployee.EmployeeID).Include(v => v.Vehicle).Where(vr => vr.Status == "Rejected" || vr.Status == "Canceled");

            return View(employeeFailedRequisition.ToList());
        }

        public ActionResult DepartmentCompletedRequisitions()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);
            var departmentCompletedRequisitions = db.VehicleRequisitions.Where(v => v.Employee.DepartmentID == getEmployee.DepartmentID).Include(v => v.Vehicle).Where(vr => vr.Status == "Completed");

            return View(departmentCompletedRequisitions.ToList());
        }
        public ActionResult DepartmentFailedRequisitions()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == getUserId);
            var departmentFailedRequisitions = db.VehicleRequisitions.Where(v => v.Employee.DepartmentID == getEmployee.DepartmentID).Include(v => v.Vehicle).Where(vr => vr.Status == "Rejected" || vr.Status == "Canceled");

            return View(departmentFailedRequisitions.ToList());
        }

        public ActionResult AllCompletedRequisitions()
        {
            this.CountView();

            var allCompletedRequisitions = db.VehicleRequisitions.Where(vr => vr.Status == "Completed");

            return View(allCompletedRequisitions.ToList());
        }
        public ActionResult AllFailedRequisitions()
        {
            this.CountView();
            var allFailedRequisitions = db.VehicleRequisitions.Where(vr => vr.Status == "Rejected" || vr.Status == "Canceled");

            return View(allFailedRequisitions.ToList());
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
