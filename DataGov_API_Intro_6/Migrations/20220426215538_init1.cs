using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataGov_API_Intro_6.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Food_Items",
                columns: table => new
                {
                    fdcId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food_Items", x => x.fdcId);
                });

            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    number = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    unitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    nutrient_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.number);
                });

            migrationBuilder.CreateTable(
                name: "Food_Nutrients",
                columns: table => new
                {
                    fdcId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    number = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food_Nutrients", x => new { x.fdcId, x.number });
                    table.ForeignKey(
                        name: "FK_Food_Nutrients_Food_Items_fdcId",
                        column: x => x.fdcId,
                        principalTable: "Food_Items",
                        principalColumn: "fdcId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Food_Nutrients_Nutrients_number",
                        column: x => x.number,
                        principalTable: "Nutrients",
                        principalColumn: "number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Food_Nutrients_number",
                table: "Food_Nutrients",
                column: "number");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Food_Nutrients");

            migrationBuilder.DropTable(
                name: "Food_Items");

            migrationBuilder.DropTable(
                name: "Nutrients");
        }
    }
}
