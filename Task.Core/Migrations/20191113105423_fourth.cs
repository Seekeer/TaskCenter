using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Core.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTasks_Task_TaskId",
                table: "ExecutorTasks");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.CreateTable(
                name: "GlobalTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalTasks", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTasks_GlobalTasks_TaskId",
                table: "ExecutorTasks",
                column: "TaskId",
                principalTable: "GlobalTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTasks_GlobalTasks_TaskId",
                table: "ExecutorTasks");

            migrationBuilder.DropTable(
                name: "GlobalTasks");

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTasks_Task_TaskId",
                table: "ExecutorTasks",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
