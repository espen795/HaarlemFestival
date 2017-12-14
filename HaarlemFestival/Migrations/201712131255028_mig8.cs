namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig8 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Historics", "LanguageId", "dbo.Languages");
            DropIndex("dbo.Historics", new[] { "LanguageId" });
            AddColumn("dbo.Guides", "Language_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Guides", "Language_LanguageId", c => c.Int());
            CreateIndex("dbo.Guides", "Language_LanguageId");
            AddForeignKey("dbo.Guides", "Language_LanguageId", "dbo.Languages", "LanguageId");
            DropColumn("dbo.Historics", "LanguageId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Historics", "LanguageId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Guides", "Language_LanguageId", "dbo.Languages");
            DropIndex("dbo.Guides", new[] { "Language_LanguageId" });
            DropColumn("dbo.Guides", "Language_LanguageId");
            DropColumn("dbo.Guides", "Language_Id");
            CreateIndex("dbo.Historics", "LanguageId");
            AddForeignKey("dbo.Historics", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
        }
    }
}
