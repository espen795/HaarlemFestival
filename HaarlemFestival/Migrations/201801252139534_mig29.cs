namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig29 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BesteldeActiviteits", new[] { "Reservering_ReserveringId1" });
            DropColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId");
            RenameColumn(table: "dbo.BesteldeActiviteits", name: "Reservering_ReserveringId1", newName: "Reservering_ReserveringId");
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int());
            CreateIndex("dbo.BesteldeActiviteits", "Reservering_ReserveringId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BesteldeActiviteits", new[] { "Reservering_ReserveringId" });
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.BesteldeActiviteits", name: "Reservering_ReserveringId", newName: "Reservering_ReserveringId1");
            AddColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int(nullable: false));
            CreateIndex("dbo.BesteldeActiviteits", "Reservering_ReserveringId1");
        }
    }
}
