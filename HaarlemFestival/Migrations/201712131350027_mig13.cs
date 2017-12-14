namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig13 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Historics", "Day_DayId", "dbo.Days");
            DropIndex("dbo.Historics", new[] { "Day_DayId" });
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageAbbr = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            AddColumn("dbo.Historics", "Language_LanguageId", c => c.Int());
            CreateIndex("dbo.Historics", "Language_LanguageId");
            AddForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages", "LanguageId");
            DropColumn("dbo.Historics", "Day_DayId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Historics", "Day_DayId", c => c.Int());
            DropForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages");
            DropIndex("dbo.Historics", new[] { "Language_LanguageId" });
            DropColumn("dbo.Historics", "Language_LanguageId");
            DropTable("dbo.Languages");
            CreateIndex("dbo.Historics", "Day_DayId");
            AddForeignKey("dbo.Historics", "Day_DayId", "dbo.Days", "DayId");
        }
    }
}
