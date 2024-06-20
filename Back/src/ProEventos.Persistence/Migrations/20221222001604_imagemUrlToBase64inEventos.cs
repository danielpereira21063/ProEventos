using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEventos.Persistence.Migrations
{
    public partial class imagemUrlToBase64inEventos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemURL",
                table: "Eventos");

            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Eventos",
                type: "longblob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Eventos");

            migrationBuilder.AddColumn<string>(
                name: "ImagemURL",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
