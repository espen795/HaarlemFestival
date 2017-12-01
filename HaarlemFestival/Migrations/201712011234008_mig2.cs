namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Historics", "Activity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Historics", "Activity", c => c.Int(nullable: false));
        }
    }
}
