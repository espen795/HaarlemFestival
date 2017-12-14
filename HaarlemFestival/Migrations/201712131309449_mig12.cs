namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Historics", "Day_DayId", c => c.Int());
            CreateIndex("dbo.Historics", "Day_DayId");
            AddForeignKey("dbo.Historics", "Day_DayId", "dbo.Days", "DayId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Historics", "Day_DayId", "dbo.Days");
            DropIndex("dbo.Historics", new[] { "Day_DayId" });
            DropColumn("dbo.Historics", "Day_DayId");
        }
    }
}
