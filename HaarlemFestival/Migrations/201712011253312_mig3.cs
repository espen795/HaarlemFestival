namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Historics",
                c => new
                {
                    ActivityId = c.Int(nullable: false),
                    TourId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.TourId);
        }
        
        public override void Down()
        {
        }
    }
}
