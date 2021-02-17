using EmployeeVRM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeVRM.Controllers
{
    public class UserRolesController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: UsersRole
        public ActionResult Index()
        {
            this.CountView();
            List<UsersAssignedRolesVM> usersRolesVMs = new List<UsersAssignedRolesVM>();
            List<ApplicationUser> users = db.Users.ToList();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            foreach(ApplicationUser user in users)
            {
                UsersAssignedRolesVM usersRolesVM = new UsersAssignedRolesVM();
                usersRolesVM.User = user;
                usersRolesVM.RoleNames = userManager.GetRoles(user.Id);
                usersRolesVMs.Add(usersRolesVM);
            }

            return View(usersRolesVMs);
        }

        // GET: UsersRole/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersRole/Create
        public ActionResult Create()
        {
            this.CountView();
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roles = roleManager.Roles.ToList();

            var users = db.Users.ToList();

            ViewBag.UserId = new SelectList(users, "Id", "UserName");
            ViewBag.RoleName = new SelectList(roles, "Name", "Name");


            return View();
        }

        // POST: UsersRole/Create
        [HttpPost]
        public ActionResult Create(UserRoleVM userRole)
        {
            try
            {
                // TODO: Add insert logic here
                if(userRole != null)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    userManager.AddToRole(userRole.UserId, userRole.RoleName);
                }
                TempData["Assigned"] = "Role Assigned Sucessfully";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersRole/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersRole/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersRole/Delete/5
        public ActionResult Delete()
        {
            this.CountView();

            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var roles = roleManager.Roles.ToList();

            var users = db.Users.ToList();

            ViewBag.UserId = new SelectList(users, "Id", "UserName");
            ViewBag.RoleName = new SelectList(roles, "Name", "Name");

            return View();
        }

        // POST: UsersRole/Delete/5
        [HttpPost]
        public ActionResult Delete(UserRoleVM userRole)
        {
            try
            {
                // TODO: Add insert logic here
                if (userRole != null)
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    userManager.RemoveFromRole(userRole.UserId, userRole.RoleName);
                }
                TempData["Removed"] = "Role Removed Sucessfully";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        
        }
    }
}
