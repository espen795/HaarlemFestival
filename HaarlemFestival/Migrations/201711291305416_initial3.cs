namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Activities", new[] { "RestaurantId" });
            DropIndex("dbo.Activities", new[] { "TourId" });
            DropIndex("dbo.Activities", new[] { "ArtistId" });
            DropIndex("dbo.Activities", new[] { "Talk_TalkId" });
            CreateTable(
                "dbo.Jazzs",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        ConcertLocation = c.String(),
                        ConcertHall = c.String(),
                        AllDayPassPartout = c.Single(nullable: false),
                        ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .Index(t => t.ActivityId)
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.Dinners",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        RestaurantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .Index(t => t.ActivityId)
                .Index(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Historics",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        TourId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .Index(t => t.ActivityId)
                .Index(t => t.TourId);
            
            CreateTable(
                "dbo.Talkings",
                c => new
                    {
                        ActivityId = c.Int(nullable: false),
                        Talk_TalkId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .Index(t => t.ActivityId)
                .Index(t => t.Talk_TalkId);
            
            DropColumn("dbo.Activities", "RestaurantId");
            DropColumn("dbo.Activities", "TourId");
            DropColumn("dbo.Activities", "ConcertLocation");
            DropColumn("dbo.Activities", "ConcertHall");
            DropColumn("dbo.Activities", "AllDayPassPartout");
            DropColumn("dbo.Activities", "ArtistId");
            DropColumn("dbo.Activities", "Discriminator");
            DropColumn("dbo.Activities", "Talk_TalkId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "Talk_TalkId", c => c.Int());
            AddColumn("dbo.Activities", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Activities", "ArtistId", c => c.Int());
            AddColumn("dbo.Activities", "AllDayPassPartout", c => c.Single());
            AddColumn("dbo.Activities", "ConcertHall", c => c.String());
            AddColumn("dbo.Activities", "ConcertLocation", c => c.String());
            AddColumn("dbo.Activities", "TourId", c => c.Int());
            AddColumn("dbo.Activities", "RestaurantId", c => c.Int());
            DropForeignKey("dbo.Talkings", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Historics", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Dinners", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Jazzs", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Talkings", new[] { "Talk_TalkId" });
            DropIndex("dbo.Talkings", new[] { "ActivityId" });
            DropIndex("dbo.Historics", new[] { "TourId" });
            DropIndex("dbo.Historics", new[] { "ActivityId" });
            DropIndex("dbo.Dinners", new[] { "RestaurantId" });
            DropIndex("dbo.Dinners", new[] { "ActivityId" });
            DropIndex("dbo.Jazzs", new[] { "ArtistId" });
            DropIndex("dbo.Jazzs", new[] { "ActivityId" });
            DropTable("dbo.Talkings");
            DropTable("dbo.Historics");
            DropTable("dbo.Dinners");
            DropTable("dbo.Jazzs");
            CreateIndex("dbo.Activities", "Talk_TalkId");
            CreateIndex("dbo.Activities", "ArtistId");
            CreateIndex("dbo.Activities", "TourId");
            CreateIndex("dbo.Activities", "RestaurantId");
        }
    }
}
