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
    public class EmployeesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //[Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllEmployee()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var allEmployeeList = from Employee in db.Employees.AsEnumerable()
                               join Department in db.Departments.AsEnumerable() on Employee.DepartmentID equals
                                   Department.id
                               join Designation in db.Designations.AsEnumerable() on Employee.DesignationID equals Designation.id

                               select new
                               {
                                   id = Employee.EmployeeID,
                                   employeeName = Employee.Name,
                                   employeeDepartment = Department.Name,
                                   employeeDesignation = Designation.Name,
                                   employeePhone = Employee.MobileNo,

                               };

            return Json(new { data = allEmployeeList }, JsonRequestBehavior.AllowGet);
        }


        //[Authorize(Roles = "Manager")]
        public ActionResult DepartmentEmployeeList()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetDepartmentEmployees()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var getUserId = User.Identity.GetUserId();
            var employeeDepartment = db.Employees.FirstOrDefault(v => v.ApplicationEmpID == getUserId).DepartmentID;

            var departmentEmployeeList = from Employee in db.Employees.AsEnumerable()
                                         join Department in db.Departments.AsEnumerable() on Employee.DepartmentID equals Department.id
                                         join Designation in db.Designations.AsEnumerable() on Employee.DesignationID equals Designation.id
                                         where Employee.DepartmentID == employeeDepartment

                                         select new
                                         {
                                             id = Employee.EmployeeID,
                                             employeeName = Employee.Name,
                                             employeeDepartment = Department.Name,
                                             employeeDesignation = Designation.Name,
                                             employeePhone = Employee.MobileNo,
                                         };
            return Json(new { data = departmentEmployeeList }, JsonRequestBehavior.AllowGet);

        }

        //[Authorize(Roles ="Employee")]

        public ActionResult EmployeeProfile()
        {
            this.CountView();
            var getUserId = User.Identity.GetUserId();
            var employeeGet = db.Employees.FirstOrDefault(e => e.ApplicationEmpID == getUserId);

            var employeeProfile = db.Employees.FirstOrDefault(v => v.EmployeeID == employeeGet.EmployeeID);

            return View(employeeProfile);
        }



        // GET: Employees/Details/5
        public ActionResult DetailsPartial(int? id)
        {
           
            Employee employee = db.Employees.Find(id);

            return PartialView("_Details", employee);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "id", "Name");
            ViewBag.DesignationID = new SelectList(db.Designations, "id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,Name,FatherName,MotherName,DateOfBirth,JoiningDate,DesignationID,DepartmentID,MobileNo,Address,NID,ApplicationEmpID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var userID = User.Identity.GetUserId();
                employee.ApplicationEmpID = userID;
                db.Employees.Add(employee);
                db.SaveChanges(); 
                return RedirectToAction("EmployeeProfile");
            }

            ViewBag.DepartmentID = new SelectList(db.Departments, "id", "Name", employee.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.Designations, "id", "Name", employee.DesignationID);
            return View(employee);
        }

        //[Authorize(Roles ="Employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "id", "Name", employee.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.Designations, "id", "Name", employee.DesignationID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,Name,FatherName,MotherName,DateOfBirth,JoiningDate,DesignationID,DepartmentID,MobileNo,Address,NID,ApplicationEmpID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.Entry(employee).Property("ApplicationEmpID").IsModified = false;
                db.SaveChanges();
                TempData["Updated"] = "Profile Updated Sucessfully";
                return RedirectToAction("EmployeeProfile");
            }
            ViewBag.DepartmentID = new SelectList(db.Departments, "id", "Name", employee.DepartmentID);
            ViewBag.DesignationID = new SelectList(db.Designations, "id", "Name", employee.DesignationID);
            return View(employee);
        }

        //[Authorize(Roles ="Admin,Manager")]
        public ActionResult DeletePartial(int? id)
        {
            Employee employee = db.Employees.Find(id);  
            return PartialView("_Delete", employee);
        }

        public ActionResult Delete(int id)
        {
            var employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
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
