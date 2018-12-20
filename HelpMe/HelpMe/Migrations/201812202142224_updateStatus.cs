namespace HelpMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomViewModels", "AttachStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomViewModels", "AttachStatus");
        }
    }
}
