namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Guides", "Language_LanguageId", "dbo.Languages");
            DropIndex("dbo.Guides", new[] { "Language_LanguageId" });
            AddColumn("dbo.Guides", "LanguageName", c => c.String());
            AddColumn("dbo.Guides", "LanguageAbbr", c => c.String());
            DropColumn("dbo.Guides", "Language_LanguageId");
            DropTable("dbo.Languages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageId = c.Int(nullable: false, identity: true),
                        LanguageName = c.String(),
                        LanguageAbbr = c.String(),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            AddColumn("dbo.Guides", "Language_LanguageId", c => c.Int());
            DropColumn("dbo.Guides", "LanguageAbbr");
            DropColumn("dbo.Guides", "LanguageName");
            CreateIndex("dbo.Guides", "Language_LanguageId");
            AddForeignKey("dbo.Guides", "Language_LanguageId", "dbo.Languages", "LanguageId");
        }
    }
}
