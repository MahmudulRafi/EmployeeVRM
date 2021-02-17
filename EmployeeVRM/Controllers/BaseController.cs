using EmployeeVRM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeVRM.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public virtual void CountView()
        {
            var empUserId = User.Identity.GetUserId();
            var getEmployee = db.Employees.FirstOrDefault(emp => emp.ApplicationEmpID == empUserId);


            if (getEmployee != null)
            {
                ViewBag.EmployeeName = getEmployee.Name;
                ViewBag.EmployeeDeignation = getEmployee.designation.Name;
                ViewBag.EmployeeDepartment = getEmployee.department.Name;
            }
            var departmentRequisition = db.VehicleRequisitions.Where(v => v.Employee.DepartmentID == getEmployee.DepartmentID);

            ViewBag.PendingDepartmentVerificationCount = departmentRequisition.Where(vr => vr.Status == "Pending").Count();
            ViewBag.OnGoingDepartmentRequistitionCount = departmentRequisition.Where(vr => vr.Status == "OnGoing").Count();

            ViewBag.PendingApprovalCount = db.VehicleRequisitions.Where(vr => vr.Status == "Verified").Count();
            ViewBag.OnGoingAllRequistitionsCount = db.VehicleRequisitions.Where(vr => vr.Status == "OnGoing").Count();

            ViewBag.PendingMaintainanceCount = db.MaintainanceRequisitions.Where(mr => mr.WorkDoneStatus == "RFQPending").Count();
            ViewBag.OnGoingMaintainanceCount = db.MaintainanceRequisitions.Where(mr => mr.WorkDoneStatus == "OnGoing").Count();

        }

    }
}