using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Task.Core.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTask_Executors_ExecutorId",
                table: "ExecutorTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTask_Task_TaskId",
                table: "ExecutorTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExecutorTask",
                table: "ExecutorTask");

            migrationBuilder.RenameTable(
                name: "ExecutorTask",
                newName: "ExecutorTasks");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutorTask_TaskId",
                table: "ExecutorTasks",
                newName: "IX_ExecutorTasks_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutorTask_ExecutorId",
                table: "ExecutorTasks",
                newName: "IX_ExecutorTasks_ExecutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExecutorTasks",
                table: "ExecutorTasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TaskStatusData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    IsUserSet = table.Column<bool>(nullable: false),
                    ExecutorTaskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatusData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskStatusData_ExecutorTasks_ExecutorTaskId",
                        column: x => x.ExecutorTaskId,
                        principalTable: "ExecutorTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskStatusData_ExecutorTaskId",
                table: "TaskStatusData",
                column: "ExecutorTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTasks_Executors_ExecutorId",
                table: "ExecutorTasks",
                column: "ExecutorId",
                principalTable: "Executors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTasks_Task_TaskId",
                table: "ExecutorTasks",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTasks_Executors_ExecutorId",
                table: "ExecutorTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutorTasks_Task_TaskId",
                table: "ExecutorTasks");

            migrationBuilder.DropTable(
                name: "TaskStatusData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExecutorTasks",
                table: "ExecutorTasks");

            migrationBuilder.RenameTable(
                name: "ExecutorTasks",
                newName: "ExecutorTask");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutorTasks_TaskId",
                table: "ExecutorTask",
                newName: "IX_ExecutorTask_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ExecutorTasks_ExecutorId",
                table: "ExecutorTask",
                newName: "IX_ExecutorTask_ExecutorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExecutorTask",
                table: "ExecutorTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTask_Executors_ExecutorId",
                table: "ExecutorTask",
                column: "ExecutorId",
                principalTable: "Executors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutorTask_Task_TaskId",
                table: "ExecutorTask",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
