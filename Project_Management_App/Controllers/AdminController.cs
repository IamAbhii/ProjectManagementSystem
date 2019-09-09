using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_Management_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Project_Management_App.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;
        public AdminController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AssignRole()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult AssignRole(string UserId, String Roles)
        {
            userManager.AddToRole(UserId, Roles);
            var user = db.Users.Find(UserId);
            if (Roles == "ProjectManager")
            {
                user.RoleType = UserRoleType.ProjectManager;
            }
            else if (Roles == "Developer")
            {
                user.RoleType = UserRoleType.Developer;
            }
            db.SaveChanges();
            ViewBag.Users = new SelectList(db.Users, "Id", "UserName");
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");
            return View("Index");
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}