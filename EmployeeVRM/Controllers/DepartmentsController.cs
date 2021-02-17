using EmployeeVRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace EmployeeVRM.Controllers
{
    public class DepartmentsController : BaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllDepartments()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var departments = db.Departments.ToList();
            return Json(new { data = departments }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartial()
        {
            var department = new Department();
            return PartialView("_Create", department);
        }

        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {

                db.Departments.Add(department);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPartial(int? id)
        {
            var department = db.Departments.Find(id);
            return PartialView("_Edit", department);
        }

        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {

                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePartial(int? id)
        {
            var department = db.Departments.Find(id);
            return PartialView("_Delete", department);
        }

        public ActionResult Delete(int id)
        {
            var department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}