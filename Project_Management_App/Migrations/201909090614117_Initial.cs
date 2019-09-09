namespace Project_Management_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        UserTaskId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.UserTasks", t => t.UserTaskId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.UserTaskId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        UserTaskId = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        ProjectID = c.Int(nullable: false),
                        CompletedPercentage = c.Int(nullable: false),
                        IsTaskCompleted = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        priority = c.Int(nullable: false),
                        IsTaskAddedToNotification = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserTaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        Startdate = c.DateTime(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                        CompletePercentage = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleType = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.NotificationForUsers",
                c => new
                    {
                        NotificationForUserId = c.Int(nullable: false, identity: true),
                        IsOpened = c.Boolean(nullable: false),
                        Message = c.String(),
                        CurrentTime = c.DateTime(nullable: false),
                        project_ProjectId = c.Int(),
                        Task_UserTaskId = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NotificationForUserId)
                .ForeignKey("dbo.Projects", t => t.project_ProjectId)
                .ForeignKey("dbo.UserTasks", t => t.Task_UserTaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.project_ProjectId)
                .Index(t => t.Task_UserTaskId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ApplicationUserUserTasks",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        UserTask_UserTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.UserTask_UserTaskId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserTasks", t => t.UserTask_UserTaskId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.UserTask_UserTaskId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NotificationForUsers", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.NotificationForUsers", "Task_UserTaskId", "dbo.UserTasks");
            DropForeignKey("dbo.NotificationForUsers", "project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ApplicationUserUserTasks", "UserTask_UserTaskId", "dbo.UserTasks");
            DropForeignKey("dbo.ApplicationUserUserTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserTasks", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Notes", "UserTaskId", "dbo.UserTasks");
            DropIndex("dbo.ApplicationUserUserTasks", new[] { "UserTask_UserTaskId" });
            DropIndex("dbo.ApplicationUserUserTasks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NotificationForUsers", new[] { "user_Id" });
            DropIndex("dbo.NotificationForUsers", new[] { "Task_UserTaskId" });
            DropIndex("dbo.NotificationForUsers", new[] { "project_ProjectId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserTasks", new[] { "ProjectID" });
            DropIndex("dbo.Notes", new[] { "User_Id" });
            DropIndex("dbo.Notes", new[] { "UserTaskId" });
            DropTable("dbo.ApplicationUserUserTasks");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NotificationForUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.UserTasks");
            DropTable("dbo.Notes");
        }
    }
}
