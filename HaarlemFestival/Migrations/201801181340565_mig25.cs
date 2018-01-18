namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContactMessages", "Subject", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactMessages", "Subject", c => c.String());
        }
    }
}
