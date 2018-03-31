namespace CentralAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Db_Structure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Sender = c.String(),
                        Message = c.String(nullable: false),
                        Tickets_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tickets", t => t.Tickets_Id)
                .Index(t => t.Tickets_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.String(),
                        CustomerId = c.String(nullable: false),
                        CompraId = c.String(),
                        SiteId = c.String(),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "Tickets_Id", "dbo.Tickets");
            DropIndex("dbo.Messages", new[] { "Tickets_Id" });
            DropTable("dbo.Tickets");
            DropTable("dbo.Messages");
        }
    }
}
