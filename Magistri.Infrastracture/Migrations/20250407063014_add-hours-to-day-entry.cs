using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magistri.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addhourstodayentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "TimeTableDayEntry",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HourNumber",
                table: "TimeTableDayEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry");

            migrationBuilder.DropColumn(
                name: "HourNumber",
                table: "TimeTableDayEntry");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "TimeTableDayEntry",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
