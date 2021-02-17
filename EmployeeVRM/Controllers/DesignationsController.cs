using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeVRM.Models;

namespace EmployeeVRM.Controllers
{
    public class DesignationsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllDesignations()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var designations = db.Designations.ToList();
            return Json(new { data = designations }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartial()
        {
            var designations = new Designation();
            return PartialView("_Create", designations);
        }

        public ActionResult Create(Designation designation)
        {
            if (ModelState.IsValid)
            {

                db.Designations.Add(designation);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPartial(int? id)
        {
            var designation = db.Designations.Find(id);
            return PartialView("_Edit", designation);
        }

        public ActionResult Edit(Designation designation)
        {
            if (ModelState.IsValid)
            {

                db.Entry(designation).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePartial(int? id)
        {
            var designation = db.Designations.Find(id);
            return PartialView("_Delete", designation);
        }

        public ActionResult Delete(int id)
        {
            var designation = db.Designations.Find(id);
            db.Designations.Remove(designation);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }

}
