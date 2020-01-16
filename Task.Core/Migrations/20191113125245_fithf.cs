using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Core.Migrations
{
    public partial class fithf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "GlobalTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ExecutorTasks",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caption",
                table: "GlobalTasks");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ExecutorTasks");
        }
    }
}
