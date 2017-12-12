namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Historics", "Guide_GuideId", "dbo.Guides");
            DropForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages");
            DropIndex("dbo.Historics", new[] { "Guide_GuideId" });
            DropIndex("dbo.Historics", new[] { "Language_LanguageId" });
            RenameColumn(table: "dbo.Historics", name: "Guide_GuideId", newName: "GuideId");
            RenameColumn(table: "dbo.Historics", name: "Language_LanguageId", newName: "LanguageId");
            AlterColumn("dbo.Historics", "GuideId", c => c.Int(nullable: false));
            AlterColumn("dbo.Historics", "LanguageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Historics", "LanguageId");
            CreateIndex("dbo.Historics", "GuideId");
            AddForeignKey("dbo.Historics", "GuideId", "dbo.Guides", "GuideId", cascadeDelete: true);
            AddForeignKey("dbo.Historics", "LanguageId", "dbo.Languages", "LanguageId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Historics", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.Historics", "GuideId", "dbo.Guides");
            DropIndex("dbo.Historics", new[] { "GuideId" });
            DropIndex("dbo.Historics", new[] { "LanguageId" });
            AlterColumn("dbo.Historics", "LanguageId", c => c.Int());
            AlterColumn("dbo.Historics", "GuideId", c => c.Int());
            RenameColumn(table: "dbo.Historics", name: "LanguageId", newName: "Language_LanguageId");
            RenameColumn(table: "dbo.Historics", name: "GuideId", newName: "Guide_GuideId");
            CreateIndex("dbo.Historics", "Language_LanguageId");
            CreateIndex("dbo.Historics", "Guide_GuideId");
            AddForeignKey("dbo.Historics", "Language_LanguageId", "dbo.Languages", "LanguageId");
            AddForeignKey("dbo.Historics", "Guide_GuideId", "dbo.Guides", "GuideId");
        }
    }
}
