namespace _1291163_MVC_EF_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuestPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guests", "PhotoPath", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Guests", "PhotoPath");
        }
    }
}
