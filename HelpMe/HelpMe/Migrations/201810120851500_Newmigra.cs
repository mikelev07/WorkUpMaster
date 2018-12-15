namespace HelpMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newmigra : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Days = c.Int(nullable: false),
                        OfferPrice = c.Int(nullable: false),
                        CustomViewModelId = c.Int(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomViewModels", t => t.CustomViewModelId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.CustomViewModelId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CustomViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        FilePath = c.String(),
                        EndingDate = c.DateTime(nullable: false),
                        TypeTaskId = c.Int(),
                        CategoryTaskId = c.Int(),
                        MinPrice = c.Int(nullable: false),
                        MaxPrice = c.Int(nullable: false),
                        AttachFilePath = c.String(),
                        UserId = c.String(maxLength: 128),
                        ExecutorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskCategories", t => t.CategoryTaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.ExecutorId, cascadeDelete: true)
                .ForeignKey("dbo.TypeCustomViewModels", t => t.TypeTaskId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.TypeTaskId)
                .Index(t => t.CategoryTaskId)
                .Index(t => t.UserId)
                .Index(t => t.ExecutorId);
            
            CreateTable(
                "dbo.TaskCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Age = c.Int(nullable: false),
                        Description = c.String(),
                        ConnectionId = c.String(),
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
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                        OwnerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OwnerId);
            
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
                "dbo.TypeCustomViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CustomViewModels", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomViewModels", "TypeTaskId", "dbo.TypeCustomViewModels");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomViewModels", "ExecutorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentViewModels", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CommentViewModels", "CustomViewModelId", "dbo.CustomViewModels");
            DropForeignKey("dbo.CustomViewModels", "CategoryTaskId", "dbo.TaskCategories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "OwnerId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Notes", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.CustomViewModels", new[] { "ExecutorId" });
            DropIndex("dbo.CustomViewModels", new[] { "UserId" });
            DropIndex("dbo.CustomViewModels", new[] { "CategoryTaskId" });
            DropIndex("dbo.CustomViewModels", new[] { "TypeTaskId" });
            DropIndex("dbo.CommentViewModels", new[] { "UserId" });
            DropIndex("dbo.CommentViewModels", new[] { "CustomViewModelId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TypeCustomViewModels");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Reviews");
            DropTable("dbo.Notes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TaskCategories");
            DropTable("dbo.CustomViewModels");
            DropTable("dbo.CommentViewModels");
        }
    }
}
