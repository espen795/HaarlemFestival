namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Locations", "LocationURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "LocationURL", c => c.String());
        }
    }
}
