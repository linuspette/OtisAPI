using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtisAPI.Migrations
{
    public partial class updated_elevator_errandnumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Errands",
                type: "nvarchar(99)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "ErrandNumber",
                table: "Errands",
                type: "nvarchar(18)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Errands_ErrandNumber",
                table: "Errands",
                column: "ErrandNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Errands_ErrandNumber",
                table: "Errands");

            migrationBuilder.DropColumn(
                name: "ErrandNumber",
                table: "Errands");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Errands",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(99)");
        }
    }
}
