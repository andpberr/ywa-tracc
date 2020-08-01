using Microsoft.EntityFrameworkCore.Migrations;

namespace ywa_tracc.Data.Migrations
{
    public partial class UserActivityAddVideoObj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VideoId",
                table: "UserActivity",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivity_VideoId",
                table: "UserActivity",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivity_Video_VideoId",
                table: "UserActivity",
                column: "VideoId",
                principalTable: "Video",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserActivity_Video_VideoId",
                table: "UserActivity");

            migrationBuilder.DropIndex(
                name: "IX_UserActivity_VideoId",
                table: "UserActivity");

            migrationBuilder.AlterColumn<string>(
                name: "VideoId",
                table: "UserActivity",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
