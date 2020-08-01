using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ywa_tracc.Data.Migrations
{
    public partial class LastRefresh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastRefresh",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    RefreshDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastRefresh", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastRefresh");
        }
    }
}
