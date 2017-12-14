namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Day_DayId", c => c.Int());
            CreateIndex("dbo.Activities", "Day_DayId");
            AddForeignKey("dbo.Activities", "Day_DayId", "dbo.Days", "DayId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "Day_DayId", "dbo.Days");
            DropIndex("dbo.Activities", new[] { "Day_DayId" });
            DropColumn("dbo.Activities", "Day_DayId");
        }
    }
}
