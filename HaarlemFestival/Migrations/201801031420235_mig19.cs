namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dinners", "timestring", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dinners", "timestring");
        }
    }
}
