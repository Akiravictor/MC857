namespace CentralAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteRegisters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SiteKey = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tickets", "ClienteId", c => c.String());
            DropColumn("dbo.Tickets", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "CustomerId", c => c.String());
            DropColumn("dbo.Tickets", "ClienteId");
            DropTable("dbo.SiteRegisters");
        }
    }
}
