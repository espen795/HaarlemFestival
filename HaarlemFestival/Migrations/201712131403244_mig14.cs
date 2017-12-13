namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages");
            DropIndex("dbo.Historics", new[] { "Language_LanguageId" });
            DropColumn("dbo.Historics", "Language_LanguageId");
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
            
            AddColumn("dbo.Historics", "Language_LanguageId", c => c.Int());
            CreateIndex("dbo.Historics", "Language_LanguageId");
            AddForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages", "LanguageId");
        }
    }
}
