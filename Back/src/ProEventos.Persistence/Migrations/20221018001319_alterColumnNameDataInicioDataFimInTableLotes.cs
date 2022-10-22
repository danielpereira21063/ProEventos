using Microsoft.EntityFrameworkCore.Migrations;

namespace ProEventos.Persistence.Migrations
{
    public partial class alterColumnNameDataInicioDataFimInTableLotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateInicio",
                table: "Lotes",
                newName: "DataInicio");

            migrationBuilder.RenameColumn(
                name: "DateFim",
                table: "Lotes",
                newName: "DataFim");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataInicio",
                table: "Lotes",
                newName: "DateInicio");

            migrationBuilder.RenameColumn(
                name: "DataFim",
                table: "Lotes",
                newName: "DateFim");
        }
    }
}
