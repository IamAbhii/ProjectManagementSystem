using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project_Management_App.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public UserRoleType RoleType { get; set; }
        public virtual ICollection<UserTask> Tasks { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public enum UserRoleType
    {
        ProjectManager,
        Developer,
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime DeadLine { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage should be in between 0-100")]
        public int CompletePercentage { get; set; }
        public virtual ICollection<UserTask> Task { get; set; }
    }

    public class UserTask
    {
        public int UserTaskId { get; set; }
        public string TaskName { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        [Range(0, 100, ErrorMessage = "Percentage should be in between 0-100")]
        public int CompletedPercentage { get; set; }
        public bool IsTaskCompleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public Priority priority { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public bool IsTaskAddedToNotification { get; set; }
    }
    public enum Priority
    {
        High,
        Medium,
        Low,
    }

    public class NotificationForUser
    {
        public int NotificationForUserId { get; set; }
        public bool IsOpened { get; set; }
        public string Message { get; set; }
        public virtual UserTask Task { get; set; }
        public virtual Project project { get; set; }
        public virtual ApplicationUser user { get; set; }
        public DateTime CurrentTime { get; set; }
    }

    public class Note
    {
        public int NoteId { get; set; }
        public string Comment { get; set; }
        public ApplicationUser User { get; set; }
        public int UserTaskId { get; set; }
        public UserTask Task { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<NotificationForUser> NotificationsForUser { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}