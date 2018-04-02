namespace CentralAtendimento.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_test_data : DbMigration
    {
        public override void Up()
        {
			string sql = @"
							insert into SiteRegisters values ('teste1');
							insert into SiteRegisters values ('teste2');
							insert into SiteRegisters values ('teste3');
							
							insert into Tickets values ('20180330203022','compra1','teste1', 1, 'cliente1');

							insert into Messages values ('2018-03-30 20:30','Akira','Produto recebido com defeito',1);
							insert into Messages values ('2018-03-30 21:30','Vendedor','Qual o defeito?',1);
							insert into Messages values ('2018-03-31 12:25','Akira','Produto não liga',1);
							insert into Messages values ('2018-04-01 12:30','Vendedor','Por favor me envie de volta',1);
							insert into Messages values ('2018-04-01 15:40','Akira','Produto enviado de volta',1);
							insert into Messages values ('2018-04-01 16:00','Administrador','Ticket Encerrado',1);
							
							insert into Tickets values ('20180320193310', NULL, 'teste1', 0, 'cliente1');
							
							insert into Messages values ('2018-03-20 19:33','Akira','Site com defeito!',2);
							insert into Messages values ('2018-03-20 19:33','Administrador','Por favor, descreva o problema',2);
							
							insert into Tickets values ('20180331120003','compra25','teste2', 2,'cliente15');
							
							insert into Messages values ('2018-03-31 12:00','João','Ainda não recebi minha encomenda!',3);
							insert into Messages values ('2018-03-31 21:55','João','Recebi minha encomenda!',3);
							
							insert into Tickets values ('20180331150023','compra3','teste2', 0,'cliente15');
							
							insert into Messages values ('2018-03-31 15:00','Akira','Comprei errado! Quero cancelar!', 4);
							insert into Messages values ('2018-03-31 21:55','Vendedor','Me envie de volta o produto', 4);
						";
							


			Sql(sql);

        }
        
        public override void Down()
        {
			string sql = @"
							delete from SiteRegisters;
							delete from Messages
							delete from Tickets";

			Sql(sql);
        }
    }
}
