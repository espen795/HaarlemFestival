namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Guides", "Language_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guides", "Language_Id", c => c.Int(nullable: false));
        }
    }
}
