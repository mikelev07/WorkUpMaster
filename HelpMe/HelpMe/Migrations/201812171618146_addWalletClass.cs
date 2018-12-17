namespace HelpMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWalletClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summ = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wallets", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Wallets", new[] { "UserId" });
            DropTable("dbo.Wallets");
        }
    }
}
