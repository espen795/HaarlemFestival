namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Jazzs", "ArtistId", "dbo.Artists");
            DropIndex("dbo.Jazzs", new[] { "ArtistId" });
            AlterColumn("dbo.Jazzs", "ArtistId", c => c.Int(nullable: true));
            CreateIndex("dbo.Jazzs", "ArtistId");
            AddForeignKey("dbo.Jazzs", "ArtistId", "dbo.Artists", "ArtistId");
            DropColumn("dbo.Jazzs", "AllDayPassPartout");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jazzs", "AllDayPassPartout", c => c.Single(nullable: false));
            DropForeignKey("dbo.Jazzs", "ArtistId", "dbo.Artists");
            DropIndex("dbo.Jazzs", new[] { "ArtistId" });
            AlterColumn("dbo.Jazzs", "ArtistId", c => c.Int(nullable: false));
            CreateIndex("dbo.Jazzs", "ArtistId");
            AddForeignKey("dbo.Jazzs", "ArtistId", "dbo.Artists", "ArtistId", cascadeDelete: true);
        }
    }
}
