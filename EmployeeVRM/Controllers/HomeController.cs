using EmployeeVRM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeVRM.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var empUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == empUserId);

            if (getEmployee == null)
            {
                ViewBag.EmployeeWelcome = "Complete your profile first";

            }
            else
            {
                ViewBag.EmployeeName = getEmployee.Name;
                ViewBag.EmployeeDeignation = getEmployee.designation.Name;
                ViewBag.EmployeeDepartment = getEmployee.department.Name;

            }

            /// Employee Count View
            if (User.IsInRole("Employee"))
            {
                if (getEmployee != null)
                {
                    ViewBag.OnGoingEmployeeRequistitionCount = db.VehicleRequisitions.Where(emp => emp.EmployeeID == getEmployee.EmployeeID).Where(vr => vr.Status == "OnGoing").Count();
                    ViewBag.PreviousEmployeeRequistitionCount = db.VehicleRequisitions.Where(emp => emp.EmployeeID == getEmployee.EmployeeID).Where(vr => vr.Status == "Completed" || vr.Status == "Cancelled" || vr.Status == "Rejected").Count();
                }
            }

            /// Manager Count View
            /// 
            if (User.IsInRole("Manager"))
            {
                var departmentRequisition = db.VehicleRequisitions.Where(v => v.Employee.DepartmentID == getEmployee.DepartmentID);

                ViewBag.PendingDepartmentVerificationCount = departmentRequisition.Where(vr => vr.Status == "Pending").Count();
                ViewBag.OnGoingDepartmentRequistitionCount = departmentRequisition.Where(vr => vr.Status == "OnGoing").Count();
                ViewBag.TotalDepartmentRequistitionCount = departmentRequisition.Where(vr => vr.Status == "Completed" || vr.Status == "Cancelled" || vr.Status == "Rejected").Count();
                ViewBag.TotalDepartmentEmployeeCount = db.Employees.Where(emp => emp.DepartmentID == getEmployee.DepartmentID).Count();
            }

            if (User.IsInRole("VMT") || User.IsInRole("Director"))
            {
                /// VMT Count View (Requisition)
                ViewBag.PendingApprovalCount = db.VehicleRequisitions.Where(vr => vr.Status == "Verified").Count();
                ViewBag.OnGoingAllRequistitionsCount = db.VehicleRequisitions.Where(vr => vr.Status == "OnGoing").Count();
                ViewBag.TotalRequistitionCount = db.VehicleRequisitions.Where(vr => vr.Status == "Completed" || vr.Status == "Cancelled" || vr.Status == "Rejected").Count();
                ViewBag.TotalEmployeeCount = db.Employees.Count();
                ViewBag.PendingAllVerificationsCount = db.VehicleRequisitions.Where(vr => vr.Status == "Pending").Count();


                /// VMT Count View (Maintainance)
                ViewBag.TotalVehiclesCount = db.Vehicles.Count();
                ViewBag.PendingMaintainanceCount = db.MaintainanceRequisitions.Where(mr => mr.WorkDoneStatus == "RFQPending").Count();
                ViewBag.OnGoingMaintainanceCount = db.MaintainanceRequisitions.Where(mr => mr.WorkDoneStatus == "OnGoing").Count();
                ViewBag.TotalMaintainanceCount = db.MaintainanceRequisitions.Where(mr => mr.WorkDoneStatus == "Completed").Count();
            }

            if (User.IsInRole("Admin"))
            {
                // Admin Count View
                ViewBag.DepartmentsCount = db.Departments.Count();
                ViewBag.SuppliersCount = db.Suppliers.Count();
                ViewBag.RegisteredUsersCount = db.Users.Count();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}