namespace CentralAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_InternalLog_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InternalLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        HttpMethod = c.String(),
                        siteId = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SiteRegisters", "SiteName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SiteRegisters", "SiteName");
            DropTable("dbo.InternalLogs");
        }
    }
}
