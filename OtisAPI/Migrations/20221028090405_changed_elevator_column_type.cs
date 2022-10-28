using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtisAPI.Migrations
{
    public partial class changed_elevator_column_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nvarchar(200)",
                table: "Elevators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nvarchar(200)",
                table: "Elevators");
        }
    }
}
