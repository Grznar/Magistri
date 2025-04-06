using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magistri.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addtimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimetableDayEntry_TimetableEntries_TimetableEntryId",
                table: "TimetableDayEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimetableDayEntry",
                table: "TimetableDayEntry");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TimetableDayEntry");

            migrationBuilder.RenameTable(
                name: "TimetableDayEntry",
                newName: "TimeTableDayEntry");

            migrationBuilder.RenameIndex(
                name: "IX_TimetableDayEntry_TimetableEntryId",
                table: "TimeTableDayEntry",
                newName: "IX_TimeTableDayEntry_TimetableEntryId");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "TimeTableDayEntry",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeTableDayEntry",
                table: "TimeTableDayEntry",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableDayEntry_LessonId",
                table: "TimeTableDayEntry",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTableDayEntry_TimetableEntries_TimetableEntryId",
                table: "TimeTableDayEntry",
                column: "TimetableEntryId",
                principalTable: "TimetableEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableDayEntry_Lessons_LessonId",
                table: "TimeTableDayEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeTableDayEntry_TimetableEntries_TimetableEntryId",
                table: "TimeTableDayEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeTableDayEntry",
                table: "TimeTableDayEntry");

            migrationBuilder.DropIndex(
                name: "IX_TimeTableDayEntry_LessonId",
                table: "TimeTableDayEntry");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "TimeTableDayEntry");

            migrationBuilder.RenameTable(
                name: "TimeTableDayEntry",
                newName: "TimetableDayEntry");

            migrationBuilder.RenameIndex(
                name: "IX_TimeTableDayEntry_TimetableEntryId",
                table: "TimetableDayEntry",
                newName: "IX_TimetableDayEntry_TimetableEntryId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TimetableDayEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimetableDayEntry",
                table: "TimetableDayEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimetableDayEntry_TimetableEntries_TimetableEntryId",
                table: "TimetableDayEntry",
                column: "TimetableEntryId",
                principalTable: "TimetableEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
