using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devchat3.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "VARCHAR(30)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    confirmar = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    fullname = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    nac = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isFacebook = table.Column<bool>(type: "bit", nullable: false),
                    isGoogle = table.Column<bool>(type: "bit", nullable: false),
                    photo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
