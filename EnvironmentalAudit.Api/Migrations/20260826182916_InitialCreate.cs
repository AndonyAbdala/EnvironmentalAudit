using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnvironmentalAudit.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    FacilityName = table.Column<string>(type: "text", nullable: false),
                    Responsible = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElectricityKwh = table.Column<decimal>(type: "numeric", nullable: false),
                    NaturalGasM3 = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterUsedM3 = table.Column<decimal>(type: "numeric", nullable: false),
                    WasteWaterM3 = table.Column<decimal>(type: "numeric", nullable: false),
                    HazardousWasteKg = table.Column<decimal>(type: "numeric", nullable: false),
                    NonHazardousWasteKg = table.Column<decimal>(type: "numeric", nullable: false),
                    RecycledWasteKg = table.Column<decimal>(type: "numeric", nullable: false),
                    DieselLiters = table.Column<decimal>(type: "numeric", nullable: false),
                    GasolineLiters = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditData_Audits_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalEmissions = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalWaste = table.Column<decimal>(type: "numeric", nullable: false),
                    RecyclingRate = table.Column<decimal>(type: "numeric", nullable: false),
                    EnergyScore = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterScore = table.Column<decimal>(type: "numeric", nullable: false),
                    WasteScore = table.Column<decimal>(type: "numeric", nullable: false),
                    EmissionsScore = table.Column<decimal>(type: "numeric", nullable: false),
                    OverallScore = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResults_Audits_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditData_AuditId",
                table: "AuditData",
                column: "AuditId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResults_AuditId",
                table: "AuditResults",
                column: "AuditId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditData");

            migrationBuilder.DropTable(
                name: "AuditResults");

            migrationBuilder.DropTable(
                name: "Audits");
        }
    }
}
