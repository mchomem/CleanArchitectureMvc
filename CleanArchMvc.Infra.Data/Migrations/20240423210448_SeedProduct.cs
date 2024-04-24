using Microsoft.EntityFrameworkCore.Migrations;

namespace CleanArchMvc.Infra.Data.Migrations
{
    public partial class SeedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // É possível via comando 'add-migration <nome_migration>', gerar uma migration vazia e editar com comandos sql para popular tabelas do banco de dados, mas deve-se ter cuidado nesta ação.
            migrationBuilder.Sql("insert into Product(Name, Description, Price, Stock, Image, CategoryId) values('Caderno espiral', 'Caderno espiral 100 folhas', 7.45,50, 'caderno1.png', 1)");
            
            migrationBuilder.Sql("insert into Product(Name, Description, Price, Stock, Image, CategoryId) values('Estojo escolar', 'Estojo escolar cinza', 5.65,70, 'estojo1.png', 1)");
            
            migrationBuilder.Sql("insert into Product(Name, Description, Price, Stock, Image, CategoryId) values('Borracha escolar', 'Borracha escolar pequena', 3.25,80, 'borracha1.png', 1)");
            
            migrationBuilder.Sql("insert into Product(Name, Description, Price, Stock, Image, CategoryId) values('Calculadora escolar', 'Calculadora simples', 15.39,20, 'calculadora1.png', 2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Product");
        }
    }
}
