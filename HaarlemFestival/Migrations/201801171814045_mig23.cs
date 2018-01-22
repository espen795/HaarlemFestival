namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig23 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Dinners", "TimeString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dinners", "TimeString", c => c.String());
        }
    }
}
