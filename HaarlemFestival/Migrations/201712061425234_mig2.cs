namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tours", "Guide_GuideId", "dbo.Guides");
            DropForeignKey("dbo.Tours", "Language_LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Historics", "TourId", "dbo.Tours");
            DropIndex("dbo.Tours", new[] { "Guide_GuideId" });
            DropIndex("dbo.Tours", new[] { "Language_LanguageId" });
            DropIndex("dbo.Historics", new[] { "TourId" });
            AddColumn("dbo.Historics", "Guide_GuideId", c => c.Int());
            AddColumn("dbo.Historics", "Language_LanguageId", c => c.Int());
            CreateIndex("dbo.Historics", "Guide_GuideId");
            CreateIndex("dbo.Historics", "Language_LanguageId");
            AddForeignKey("dbo.Historics", "Guide_GuideId", "dbo.Guides", "GuideId");
            AddForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages", "LanguageId");
            DropColumn("dbo.Historics", "TourId");
            DropTable("dbo.Tours");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        TourId = c.Int(nullable: false, identity: true),
                        Guide_GuideId = c.Int(),
                        Language_LanguageId = c.Int(),
                    })
                .PrimaryKey(t => t.TourId);
            
            AddColumn("dbo.Historics", "TourId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Historics", "Guide_GuideId", "dbo.Guides");
            DropIndex("dbo.Historics", new[] { "Language_LanguageId" });
            DropIndex("dbo.Historics", new[] { "Guide_GuideId" });
            DropColumn("dbo.Historics", "Language_LanguageId");
            DropColumn("dbo.Historics", "Guide_GuideId");
            CreateIndex("dbo.Historics", "TourId");
            CreateIndex("dbo.Tours", "Language_LanguageId");
            CreateIndex("dbo.Tours", "Guide_GuideId");
            AddForeignKey("dbo.Historics", "TourId", "dbo.Tours", "TourId", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "Language_LanguageId", "dbo.Languages", "LanguageId");
            AddForeignKey("dbo.Tours", "Guide_GuideId", "dbo.Guides", "GuideId");
        }
    }
}
