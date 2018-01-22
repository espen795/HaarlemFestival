namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Accounts", "RoleId", c => c.Int());
            CreateIndex("dbo.Accounts", "RoleId");
            AddForeignKey("dbo.Accounts", "RoleId", "dbo.Roles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropColumn("dbo.Accounts", "RoleId");
            DropTable("dbo.Roles");
        }
    }
}
