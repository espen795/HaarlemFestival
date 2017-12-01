namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Historics", "Activity", c => c.Int(nullable: false));
            AlterColumn("dbo.Activities", "Price", c => c.Single());
            AlterColumn("dbo.Activities", "AlternativePrice", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "AlternativePrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Activities", "Price", c => c.Single(nullable: false));
            DropColumn("dbo.Historics", "Activity");
        }
    }
}
