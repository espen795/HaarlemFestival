namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BesteldeActiviteits", "AantalAlternatief", c => c.Int(nullable: false));
            DropColumn("dbo.BesteldeActiviteits", "ActiviteitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BesteldeActiviteits", "ActiviteitId", c => c.Int(nullable: false));
            DropColumn("dbo.BesteldeActiviteits", "AantalAlternatief");
        }
    }
}
