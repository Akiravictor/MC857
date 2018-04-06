namespace CentralAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updating_SiteRegisters : DbMigration
    {
        public override void Up()
        {
			string sql = @"
				update SiteRegisters set SiteName = 'Site Teste 1' where SiteKey = 'teste1';
update SiteRegisters set SiteName = 'Site Teste 2' where SiteKey = 'teste2';
update SiteRegisters set SiteName = 'Site Teste 3' where SiteKey = 'teste3';
";

			Sql(sql);
        }
        
        public override void Down()
        {
			string sql = @"
				update SiteRegisters set SiteName = NULL where SiteKey = 'teste1';
update SiteRegisters set SiteName = NULL where SiteKey = 'teste2';
update SiteRegisters set SiteName = NULL where SiteKey = 'teste3';
";

			Sql(sql);
		}
    }
}
