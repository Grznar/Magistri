using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magistri.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addtimeentry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Classes_ClassIdKey",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ClassIdKey",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ClassIdKey",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "TimetableEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "IdKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimetableDayEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimetableEntryId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableDayEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableDayEntry_TimetableEntries_TimetableEntryId",
                        column: x => x.TimetableEntryId,
                        principalTable: "TimetableEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableDayEntry_TimetableEntryId",
                table: "TimetableDayEntry",
                column: "TimetableEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_ClassId",
                table: "TimetableEntries",
                column: "ClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimetableDayEntry");

            migrationBuilder.DropTable(
                name: "TimetableEntries");

            migrationBuilder.AddColumn<int>(
                name: "ClassIdKey",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassIdKey",
                table: "Lessons",
                column: "ClassIdKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Classes_ClassIdKey",
                table: "Lessons",
                column: "ClassIdKey",
                principalTable: "Classes",
                principalColumn: "IdKey",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
