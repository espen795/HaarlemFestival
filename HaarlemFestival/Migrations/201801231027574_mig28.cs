namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig28 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId", "dbo.Reserverings");
            DropIndex("dbo.BesteldeActiviteits", new[] { "Reservering_ReserveringId" });
            AddColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId1", c => c.Int());
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int(nullable: false));
            CreateIndex("dbo.BesteldeActiviteits", "Reservering_ReserveringId1");
            AddForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId1", "dbo.Reserverings", "ReserveringId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId1", "dbo.Reserverings");
            DropIndex("dbo.BesteldeActiviteits", new[] { "Reservering_ReserveringId1" });
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int());
            DropColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId1");
            CreateIndex("dbo.BesteldeActiviteits", "Reservering_ReserveringId");
            AddForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId", "dbo.Reserverings", "ReserveringId");
        }
    }
}
