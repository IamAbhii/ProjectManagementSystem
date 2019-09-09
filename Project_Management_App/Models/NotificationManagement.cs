using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Management_App.Models
{
    public class NotificationManagement
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public NotificationManagement(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void AddNotification(string UserId)
        {
            var user = db.Users.Find(UserId);
            var tasks = user.Tasks;
            foreach (var task in tasks)
            {
                if (!task.IsTaskAddedToNotification && task.Deadline < DateTime.Now && !task.IsTaskCompleted && !task.IsTaskAddedToNotification)
                {

                    var notification = new NotificationForUser()
                    {
                        CurrentTime = DateTime.Now,
                        IsOpened = false,
                        Message = task.TaskName + " is passed its due date " + task.Deadline,
                        project = task.Project,
                        user = user,
                        Task = task,
                    };
                    task.IsTaskAddedToNotification = true;
                    db.NotificationsForUser.Add(notification);
                    db.SaveChanges();
                }
            }
        }
    }
}