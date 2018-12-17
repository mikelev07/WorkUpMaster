namespace HelpMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCustomClassAddPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomViewModels", "ExecutorPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomViewModels", "ExecutorPrice");
        }
    }
}
