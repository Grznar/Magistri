using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magistri.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class addClassToStudentFKClasRenameOkdsadsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Classes_StudentClassId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "StudentClassId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Classes_StudentClassId",
                table: "AspNetUsers",
                column: "StudentClassId",
                principalTable: "Classes",
                principalColumn: "IdKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Classes_StudentClassId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "StudentClassId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Classes_StudentClassId",
                table: "AspNetUsers",
                column: "StudentClassId",
                principalTable: "Classes",
                principalColumn: "IdKey",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
