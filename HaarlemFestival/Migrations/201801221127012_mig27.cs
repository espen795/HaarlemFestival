namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig27 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BesteldeActiviteits", "ReserveringId", "dbo.Reserverings");
            DropForeignKey("dbo.Reserverings", "KlantId", "dbo.Klants");
            DropIndex("dbo.BesteldeActiviteits", new[] { "ReserveringId" });
            DropIndex("dbo.Reserverings", new[] { "KlantId" });
            RenameColumn(table: "dbo.BesteldeActiviteits", name: "ReserveringId", newName: "Reservering_ReserveringId");
            RenameColumn(table: "dbo.Reserverings", name: "KlantId", newName: "Klant_KlantId");
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int());
            AlterColumn("dbo.Reserverings", "Klant_KlantId", c => c.Int());
            CreateIndex("dbo.BesteldeActiviteits", "Reservering_ReserveringId");
            CreateIndex("dbo.Reserverings", "Klant_KlantId");
            AddForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId", "dbo.Reserverings", "ReserveringId");
            AddForeignKey("dbo.Reserverings", "Klant_KlantId", "dbo.Klants", "KlantId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reserverings", "Klant_KlantId", "dbo.Klants");
            DropForeignKey("dbo.BesteldeActiviteits", "Reservering_ReserveringId", "dbo.Reserverings");
            DropIndex("dbo.Reserverings", new[] { "Klant_KlantId" });
            DropIndex("dbo.BesteldeActiviteits", new[] { "Reservering_ReserveringId" });
            AlterColumn("dbo.Reserverings", "Klant_KlantId", c => c.Int(nullable: false));
            AlterColumn("dbo.BesteldeActiviteits", "Reservering_ReserveringId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Reserverings", name: "Klant_KlantId", newName: "KlantId");
            RenameColumn(table: "dbo.BesteldeActiviteits", name: "Reservering_ReserveringId", newName: "ReserveringId");
            CreateIndex("dbo.Reserverings", "KlantId");
            CreateIndex("dbo.BesteldeActiviteits", "ReserveringId");
            AddForeignKey("dbo.Reserverings", "KlantId", "dbo.Klants", "KlantId", cascadeDelete: true);
            AddForeignKey("dbo.BesteldeActiviteits", "ReserveringId", "dbo.Reserverings", "ReserveringId", cascadeDelete: true);
        }
    }
}
