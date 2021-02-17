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
    public class SuppliersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suppliers
        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllSupplier()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Suppliers = from Supplier in db.Suppliers.AsEnumerable()
                           select new
                           {
                               id = Supplier.SupplierID,
                               CompanyName = Supplier.CompanyName,
                               Phone = Supplier.Phone,
                           };

            return Json(new { data = Suppliers }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartial()
        {
            var supplier = new Supplier();
            return PartialView("_Create", supplier);
        }

        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {

                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPartial(int? id)
        {
            var supplier = db.Suppliers.Find(id);
            return PartialView("_Edit", supplier);
        }

        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {

                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePartial(int? id)
        {
            var supplier = db.Suppliers.Find(id);
            return PartialView("_Delete", supplier);
        }

        public ActionResult Delete(int id)
        {
            var supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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
