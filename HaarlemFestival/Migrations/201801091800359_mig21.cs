namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BesteldeActiviteits", "TotalBoughtTickets", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BesteldeActiviteits", "TotalBoughtTickets");
        }
    }
}
