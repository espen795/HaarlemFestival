namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(),
                        ArtistImage = c.String(),
                        ArtistInformation = c.String(),
                    })
                .PrimaryKey(t => t.ArtistId);
            
            CreateTable(
                "dbo.BesteldeActiviteits",
                c => new
                    {
                        BesteldeActiviteitId = c.Int(nullable: false, identity: true),
                        ReserveringId = c.Int(nullable: false),
                        Aantal = c.Int(nullable: false),
                        Opmerking = c.String(),
                        ActiviteitId = c.Int(nullable: false),
                        Activiteit_ActivityId = c.Int(),
                    })
                .PrimaryKey(t => t.BesteldeActiviteitId)
                .ForeignKey("dbo.Activities", t => t.Activiteit_ActivityId)
                .ForeignKey("dbo.Reserverings", t => t.ReserveringId, cascadeDelete: true)
                .Index(t => t.ReserveringId)
                .Index(t => t.Activiteit_ActivityId);
            
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        EventType = c.Int(nullable: false),
                        DateReservation = c.DateTime(nullable: false),
                        StartSession = c.DateTime(nullable: false),
                        EndSession = c.DateTime(nullable: false),
                        Price = c.Single(nullable: false),
                        AlternativePrice = c.Single(nullable: false),
                        TotalTickets = c.Int(nullable: false),
                        BoughtTickets = c.Int(nullable: false),
                        Phonenumber = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.ActivityId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Rating = c.String(),
                        Description = c.String(),
                        FoodIMG = c.String(),
                        LocationIMG = c.String(),
                    })
                .PrimaryKey(t => t.RestaurantId);
            
            CreateTable(
                "dbo.Cuisines",
                c => new
                    {
                        CuisineId = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Restaurant_RestaurantId = c.Int(),
                    })
                .PrimaryKey(t => t.CuisineId)
                .ForeignKey("dbo.Restaurants", t => t.Restaurant_RestaurantId)
                .Index(t => t.Restaurant_RestaurantId);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        TourId = c.Int(nullable: false, identity: true),
                        Guide_GuideId = c.Int(),
                        Language_LanguageId = c.Int(),
                    })
                .PrimaryKey(t => t.TourId)
                .ForeignKey("dbo.Guides", t => t.Guide_GuideId)
                .ForeignKey("dbo.Languages", t => t.Language_LanguageId)
                .Index(t => t.Guide_GuideId)
                .Index(t => t.Language_LanguageId);
            
            CreateTable(
                "dbo.Guides",
                c => new
                    {
                        GuideId = c.Int(nullable: false, identity: true),
                        GuideName = c.String(),
                        WorksOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GuideId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageAbbr = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.Talks",
                c => new
                    {
                        TalkId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.TalkId);
            
            CreateTable(
                "dbo.Reserverings",
                c => new
                    {
                        ReserveringId = c.Int(nullable: false, identity: true),
                        KlantId = c.Int(nullable: false),
                        TotaalPrijs = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ReserveringId)
                .ForeignKey("dbo.Klants", t => t.KlantId, cascadeDelete: true)
                .Index(t => t.KlantId);
            
            CreateTable(
                "dbo.Klants",
                c => new
                    {
                        KlantId = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Email = c.String(),
                        BetaalMethode = c.String(),
                    })
                .PrimaryKey(t => t.KlantId);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        DayId = c.Int(nullable: false, identity: true),
                        Naam = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DayId);
            
            CreateTable(
                "dbo.InterviewQuestions",
                c => new
                    {
                        InterviewQuestionId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.InterviewQuestionId);
            
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
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
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
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
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
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
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
                .ForeignKey("dbo.Talks", t => t.Talk_TalkId)
                .Index(t => t.ActivityId)
                .Index(t => t.Talk_TalkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Talkings", "Talk_TalkId", "dbo.Talks");
            DropForeignKey("dbo.Talkings", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Historics", "TourId", "dbo.Tours");
            DropForeignKey("dbo.Historics", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Dinners", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Dinners", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Jazzs", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.Jazzs", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Reserverings", "KlantId", "dbo.Klants");
            DropForeignKey("dbo.BesteldeActiviteits", "ReserveringId", "dbo.Reserverings");
            DropForeignKey("dbo.BesteldeActiviteits", "Activiteit_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Tours", "Language_LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Tours", "Guide_GuideId", "dbo.Guides");
            DropForeignKey("dbo.Cuisines", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Talkings", new[] { "Talk_TalkId" });
            DropIndex("dbo.Talkings", new[] { "ActivityId" });
            DropIndex("dbo.Historics", new[] { "TourId" });
            DropIndex("dbo.Historics", new[] { "ActivityId" });
            DropIndex("dbo.Dinners", new[] { "RestaurantId" });
            DropIndex("dbo.Dinners", new[] { "ActivityId" });
            DropIndex("dbo.Jazzs", new[] { "ArtistId" });
            DropIndex("dbo.Jazzs", new[] { "ActivityId" });
            DropIndex("dbo.Reserverings", new[] { "KlantId" });
            DropIndex("dbo.Tours", new[] { "Language_LanguageId" });
            DropIndex("dbo.Tours", new[] { "Guide_GuideId" });
            DropIndex("dbo.Cuisines", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.BesteldeActiviteits", new[] { "Activiteit_ActivityId" });
            DropIndex("dbo.BesteldeActiviteits", new[] { "ReserveringId" });
            DropTable("dbo.Talkings");
            DropTable("dbo.Historics");
            DropTable("dbo.Dinners");
            DropTable("dbo.Jazzs");
            DropTable("dbo.InterviewQuestions");
            DropTable("dbo.Days");
            DropTable("dbo.Klants");
            DropTable("dbo.Reserverings");
            DropTable("dbo.Talks");
            DropTable("dbo.Languages");
            DropTable("dbo.Guides");
            DropTable("dbo.Tours");
            DropTable("dbo.Cuisines");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Activities");
            DropTable("dbo.BesteldeActiviteits");
            DropTable("dbo.Artists");
            DropTable("dbo.Accounts");
        }
    }
}
