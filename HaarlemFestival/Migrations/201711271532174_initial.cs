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
                        RestaurantId = c.Int(),
                        TourId = c.Int(),
                        ConcertLocation = c.String(),
                        ConcertHall = c.String(),
                        AllDayPassPartout = c.Single(),
                        ArtistId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Talk_TalkId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .ForeignKey("dbo.Tours", t => t.TourId, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.Talks", t => t.Talk_TalkId)
                .Index(t => t.RestaurantId)
                .Index(t => t.TourId)
                .Index(t => t.ArtistId)
                .Index(t => t.Talk_TalkId);
            
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
                "dbo.Talks",
                c => new
                    {
                        TalkId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.TalkId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserverings", "KlantId", "dbo.Klants");
            DropForeignKey("dbo.BesteldeActiviteits", "ReserveringId", "dbo.Reserverings");
            DropForeignKey("dbo.BesteldeActiviteits", "Activiteit_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.Activities", "Talk_TalkId", "dbo.Talks");
            DropForeignKey("dbo.Activities", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.Activities", "TourId", "dbo.Tours");
            DropForeignKey("dbo.Tours", "Language_LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Tours", "Guide_GuideId", "dbo.Guides");
            DropForeignKey("dbo.Activities", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Cuisines", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropIndex("dbo.Reserverings", new[] { "KlantId" });
            DropIndex("dbo.BesteldeActiviteits", new[] { "Activiteit_ActivityId" });
            DropIndex("dbo.BesteldeActiviteits", new[] { "ReserveringId" });
            DropIndex("dbo.Tours", new[] { "Language_LanguageId" });
            DropIndex("dbo.Tours", new[] { "Guide_GuideId" });
            DropIndex("dbo.Cuisines", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.Activities", new[] { "Talk_TalkId" });
            DropIndex("dbo.Activities", new[] { "ArtistId" });
            DropIndex("dbo.Activities", new[] { "TourId" });
            DropIndex("dbo.Activities", new[] { "RestaurantId" });
            DropTable("dbo.InterviewQuestions");
            DropTable("dbo.Days");
            DropTable("dbo.Klants");
            DropTable("dbo.Reserverings");
            DropTable("dbo.BesteldeActiviteits");
            DropTable("dbo.Talks");
            DropTable("dbo.Artists");
            DropTable("dbo.Languages");
            DropTable("dbo.Guides");
            DropTable("dbo.Tours");
            DropTable("dbo.Cuisines");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Activities");
            DropTable("dbo.Accounts");
        }
    }
}
