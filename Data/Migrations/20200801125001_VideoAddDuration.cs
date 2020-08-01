using Microsoft.EntityFrameworkCore.Migrations;

namespace ywa_tracc.Data.Migrations
{
    public partial class VideoAddDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Video",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Video");
        }
    }
}
