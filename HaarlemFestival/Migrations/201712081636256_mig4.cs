namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Historics", "HistoricId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Historics", "HistoricId");
        }
    }
}
