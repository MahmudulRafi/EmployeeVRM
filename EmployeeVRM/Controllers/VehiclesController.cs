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
    public class VehiclesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            this.CountView();
            return View();
        }

        public ActionResult GetAllVehicles()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Vehicles = from Vehicle in db.Vehicles.AsEnumerable()
                           select new
                           {
                               id = Vehicle.VehicleID,
                               RegistrationNo = Vehicle.RegistrationNo,
                               VehicleName = Vehicle.Name,
                               SeatAllocated = Vehicle.SeatAllocated,
                               VehicleType = Vehicle.Type,
                           };

            return Json(new { data = Vehicles }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartial()
        {
            var vehicle = new Vehicle();
            return PartialView("_Create", vehicle);
        }

        public ActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                vehicle.Available = "Yes";

                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPartial(int? id)
        {
            var vehicle = db.Vehicles.Find(id);
            return PartialView("_Edit", vehicle);
        }

        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {

                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeletePartial(int? id)
        {
            var vehicle = db.Vehicles.Find(id);
            return PartialView("_Delete", vehicle);
        }

        public ActionResult Delete(int id)
        {
            var vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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
