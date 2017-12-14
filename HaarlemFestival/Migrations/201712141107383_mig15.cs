namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig15 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Historics", "HistoricId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Historics", "HistoricId", c => c.Int(nullable: false));
        }
    }
}
