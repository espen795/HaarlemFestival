namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Guides", "WorksOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guides", "WorksOn", c => c.DateTime(nullable: false));
        }
    }
}
