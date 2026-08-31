using EnvironmentalAudit.Api.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EnvironmentalAudit.Api.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> GenerateAuditReportAsync(Guid auditId)
    {
        var audit = await _context.Audits
            .Include(a => a.Data)
            .Include(a => a.Result)
            .FirstOrDefaultAsync(a => a.Id == auditId);

        if (audit is null)
        {
            throw new KeyNotFoundException(
                $"Audit with id {auditId} was not found.");
        }

        if (audit.Data is null || audit.Result is null)
        {
            throw new InvalidOperationException(
                "The audit has not been calculated yet.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);

                page.Header()
                    .Text("ENVIRONMENTAL AUDIT REPORT")
                    .FontSize(24)
                    .Bold();

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Spacing(15);

                        // Audit information
                        column.Item()
                            .Text("Audit Information")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Text(text =>
                            {
                                text.Span("Company: ").Bold();
                                text.Span(audit.CompanyName);
                            });

                        column.Item()
                            .Text(text =>
                            {
                                text.Span("Facility: ").Bold();
                                text.Span(audit.FacilityName);
                            });

                        column.Item()
                            .Text(text =>
                            {
                                text.Span("Responsible: ").Bold();
                                text.Span(audit.Responsible);
                            });

                        column.Item()
                            .Text(text =>
                            {
                                text.Span("Period: ").Bold();
                                text.Span(
                                    $"{audit.StartDate:yyyy-MM-dd} - {audit.EndDate:yyyy-MM-dd}");
                            });

                        // Energy
                        column.Item()
                            .Text("Energy")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Electricity");
                                table.Cell().Text(
                                    $"{audit.Data.ElectricityKwh:N0} kWh");

                                table.Cell().Text("Natural Gas");
                                table.Cell().Text(
                                    $"{audit.Data.NaturalGasM3:N0} m³");
                            });

                        // Water
                        column.Item()
                            .Text("Water")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Water Used");
                                table.Cell().Text(
                                    $"{audit.Data.WaterUsedM3:N0} m³");

                                table.Cell().Text("Wastewater");
                                table.Cell().Text(
                                    $"{audit.Data.WasteWaterM3:N0} m³");
                            });

                        // Waste
                        column.Item()
                            .Text("Waste")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Hazardous Waste");
                                table.Cell().Text(
                                    $"{audit.Data.HazardousWasteKg:N0} kg");

                                table.Cell().Text("Non-Hazardous Waste");
                                table.Cell().Text(
                                    $"{audit.Data.NonHazardousWasteKg:N0} kg");

                                table.Cell().Text("Recycled Waste");
                                table.Cell().Text(
                                    $"{audit.Data.RecycledWasteKg:N0} kg");
                            });

                        // Fuel
                        column.Item()
                            .Text("Fuel")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Diesel");
                                table.Cell().Text(
                                    $"{audit.Data.DieselLiters:N0} L");

                                table.Cell().Text("Gasoline");
                                table.Cell().Text(
                                    $"{audit.Data.GasolineLiters:N0} L");
                            });

                        // Results
                        column.Item()
                            .Text("Audit Results")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Text("Total Emissions");
                                table.Cell().Text(
                                    $"{audit.Result.TotalEmissions:N2}");

                                table.Cell().Text("Total Waste");
                                table.Cell().Text(
                                    $"{audit.Result.TotalWaste:N2} kg");

                                table.Cell().Text("Recycling Rate");
                                table.Cell().Text(
                                    $"{audit.Result.RecyclingRate:N2}%");

                                table.Cell().Text("Energy Score");
                                table.Cell().Text(
                                    $"{audit.Result.EnergyScore:N2}");

                                table.Cell().Text("Water Score");
                                table.Cell().Text(
                                    $"{audit.Result.WaterScore:N2}");

                                table.Cell().Text("Waste Score");
                                table.Cell().Text(
                                    $"{audit.Result.WasteScore:N2}");

                                table.Cell().Text("Emissions Score");
                                table.Cell().Text(
                                    $"{audit.Result.EmissionsScore:N2}");
                            });

                        // Overall score
                        column.Item()
                            .PaddingTop(20)
                            .AlignCenter()
                            .Text(
                                $"OVERALL SCORE: {audit.Result.OverallScore:N2} / 100")
                            .FontSize(22)
                            .Bold();
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(
                        "Environmental Audit POC - Generated automatically");
            });
        });

        return document.GeneratePdf();
    }
}