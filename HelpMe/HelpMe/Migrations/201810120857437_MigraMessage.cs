namespace HelpMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigraMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageStoreViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DateSend = c.DateTime(nullable: false),
                        UserFromId = c.String(maxLength: 128),
                        UserToId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserFromId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserToId, cascadeDelete: true)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageStoreViewModels", "UserToId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MessageStoreViewModels", "UserFromId", "dbo.AspNetUsers");
            DropIndex("dbo.MessageStoreViewModels", new[] { "UserToId" });
            DropIndex("dbo.MessageStoreViewModels", new[] { "UserFromId" });
            DropTable("dbo.MessageStoreViewModels");
        }
    }
}
