using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ywa_tracc.Data.Migrations
{
    public partial class UserActivityAddDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "WatchDate",
                table: "UserActivity",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WatchDate",
                table: "UserActivity");
        }
    }
}
