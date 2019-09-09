using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_Management_App.Models;

namespace Project_Management_App.Controllers
{
    public class UserTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> userManager;
        RoleManager<IdentityRole> roleManager;


        public UserTasksController()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        [Authorize(Roles = "ProjectManager,Developer")]
        public ActionResult TaskByPriority()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var Task = user.Tasks.OrderBy(x => x.priority);
            return View(Task);
        }
        [Authorize(Roles = "ProjectManager,Developer")]
        public ActionResult GetUsersTask()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var Task = user.Tasks;
            return View(Task);
        }

        [Authorize(Roles = "ProjectManager")]
        public ActionResult AssignTask()
        {

            var users = roleManager.FindByName("Developer").Users;
            List<ApplicationUser> Developers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var User = db.Users.Where(x => x.Id == user.UserId).First();
                Developers.Add(User);
            }


            ViewBag.DeveloperId = new SelectList(Developers, "Id", "UserName");
            ViewBag.TaskId = new SelectList(db.Tasks, "UserTaskId", "TaskName");
            return View();
        }

        [HttpPost]
        public ActionResult AssignTask(string DeveloperId, int TaskId)
        {
            var Task = db.Tasks.Find(TaskId);
            var Devloper = db.Users.Find(DeveloperId);
            Devloper.Tasks.Add(Task);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: UserTasks

        [Authorize(Roles = "ProjectManager,Developer")]
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(u => u.Project);
            return View(tasks.ToList());
        }

        // GET: UserTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.Tasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // GET: UserTasks/Create
        [Authorize(Roles = "ProjectManager")]
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserTaskId,TaskName,ProjectID,CompletedPercentage,IsTaskCompleted,StartDate,Deadline,priority")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(userTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectId", "ProjectName", userTask.ProjectID);
            return View(userTask);
        }
        [Authorize(Roles = "ProjectManager,Developer")]
        // GET: UserTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.Tasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectId", "ProjectName", userTask.ProjectID);
            return View(userTask);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTaskId,TaskName,ProjectID,CompletedPercentage,IsTaskCompleted,StartDate,Deadline,priority")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ProjectId", "ProjectName", userTask.ProjectID);
            return View(userTask);
        }
        [Authorize(Roles = "ProjectManager")]
        // GET: UserTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTask userTask = db.Tasks.Find(id);
            if (userTask == null)
            {
                return HttpNotFound();
            }
            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserTask userTask = db.Tasks.Find(id);
            db.Tasks.Remove(userTask);
            db.SaveChanges();
            return RedirectToAction("Index");
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
