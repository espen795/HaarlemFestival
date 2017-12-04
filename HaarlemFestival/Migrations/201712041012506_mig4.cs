namespace HaarlemFestival.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cuisines", "Restaurant_RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.Talkings", "Talk_TalkId", "dbo.Talks");
            DropIndex("dbo.Cuisines", new[] { "Restaurant_RestaurantId" });
            DropIndex("dbo.Talkings", new[] { "Talk_TalkId" });
            RenameColumn(table: "dbo.Talkings", name: "Talk_TalkId", newName: "TalkId");
            AddColumn("dbo.Talks", "Person1", c => c.String());
            AddColumn("dbo.Talks", "Person2", c => c.String());
            AddColumn("dbo.Talks", "MaxTickets", c => c.Int(nullable: false));
            AddColumn("dbo.InterviewQuestions", "Content", c => c.String());
            AddColumn("dbo.InterviewQuestions", "Receiver", c => c.String());
            AlterColumn("dbo.Talkings", "TalkId", c => c.Int(nullable: false));
            CreateIndex("dbo.Talkings", "TalkId");
            AddForeignKey("dbo.Talkings", "TalkId", "dbo.Talks", "TalkId", cascadeDelete: true);
            DropColumn("dbo.Cuisines", "Restaurant_RestaurantId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cuisines", "Restaurant_RestaurantId", c => c.Int());
            DropForeignKey("dbo.Talkings", "TalkId", "dbo.Talks");
            DropIndex("dbo.Talkings", new[] { "TalkId" });
            AlterColumn("dbo.Talkings", "TalkId", c => c.Int());
            DropColumn("dbo.InterviewQuestions", "Receiver");
            DropColumn("dbo.InterviewQuestions", "Content");
            DropColumn("dbo.Talks", "MaxTickets");
            DropColumn("dbo.Talks", "Person2");
            DropColumn("dbo.Talks", "Person1");
            RenameColumn(table: "dbo.Talkings", name: "TalkId", newName: "Talk_TalkId");
            CreateIndex("dbo.Talkings", "Talk_TalkId");
            CreateIndex("dbo.Cuisines", "Restaurant_RestaurantId");
            AddForeignKey("dbo.Talkings", "Talk_TalkId", "dbo.Talks", "TalkId");
            AddForeignKey("dbo.Cuisines", "Restaurant_RestaurantId", "dbo.Restaurants", "RestaurantId");
        }
    }
}
