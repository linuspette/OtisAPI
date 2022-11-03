using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OtisAPI.Migrations
{
    public partial class Inititial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elevators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elevators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Errands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ElevatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Errands_Elevators_ElevatorId",
                        column: x => x.ElevatorId,
                        principalTable: "Elevators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEntityErrandEntity",
                columns: table => new
                {
                    AssignedErrandsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedTechniciansId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEntityErrandEntity", x => new { x.AssignedErrandsId, x.AssignedTechniciansId });
                    table.ForeignKey(
                        name: "FK_EmployeeEntityErrandEntity_Employees_AssignedTechniciansId",
                        column: x => x.AssignedTechniciansId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEntityErrandEntity_Errands_AssignedErrandsId",
                        column: x => x.AssignedErrandsId,
                        principalTable: "Errands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErrandUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    DateOfUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrandEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrandUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ErrandUpdates_Errands_ErrandEntityId",
                        column: x => x.ErrandEntityId,
                        principalTable: "Errands",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEntityErrandEntity_AssignedTechniciansId",
                table: "EmployeeEntityErrandEntity",
                column: "AssignedTechniciansId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Errands_ElevatorId",
                table: "Errands",
                column: "ElevatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrandUpdates_ErrandEntityId",
                table: "ErrandUpdates",
                column: "ErrandEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeEntityErrandEntity");

            migrationBuilder.DropTable(
                name: "ErrandUpdates");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Errands");

            migrationBuilder.DropTable(
                name: "Elevators");
        }
    }
}
