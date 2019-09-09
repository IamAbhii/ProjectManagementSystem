namespace Project_Management_App.Migrations
{
    using Project_Management_App.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project_Management_App.Models.ApplicationDbContext>
    {
        private RoleManagement roleManager;
        private ApplicationDbContext dbContext;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            dbContext = new ApplicationDbContext();
            roleManager = new RoleManagement(dbContext);
        }

        protected override void Seed(Project_Management_App.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            roleManager.CreateRole("Admin"); //.ToString().ToUpper();
            roleManager.CreateRole("ProjectManager"); //.ToString().ToUpper();
            roleManager.CreateRole("Developer"); //.ToString().ToUpper();

        }
    }
}
