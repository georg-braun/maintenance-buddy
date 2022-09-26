using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace maintenance_buddy_api.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserVehicleConnections",
                columns: table => new
                {
                    NameIdentifier = table.Column<string>(type: "text", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVehicleConnections", x => new { x.NameIdentifier, x.VehicleId });
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Kilometer = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    KilometerInterval = table.Column<int>(type: "integer", nullable: false),
                    TimeInterval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionTemplate_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Kilometer = table.Column<int>(type: "integer", nullable: false),
                    ActionTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceAction_ActionTemplate_ActionTemplateId",
                        column: x => x.ActionTemplateId,
                        principalTable: "ActionTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionTemplate_VehicleId",
                table: "ActionTemplate",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceAction_ActionTemplateId",
                table: "MaintenanceAction",
                column: "ActionTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceAction");

            migrationBuilder.DropTable(
                name: "UserVehicleConnections");

            migrationBuilder.DropTable(
                name: "ActionTemplate");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
