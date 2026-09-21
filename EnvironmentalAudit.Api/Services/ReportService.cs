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

                // HEADER
                page.Header()
                .Row(row =>
                {
                    row.Spacing(25);

                    row.ConstantItem(80)
                        .Image("Assets/Logo.jpg");

                    row.RelativeItem()
                        .Column(column =>
                        {
                            column.Item()
                                .Text("ENVIRONMENTAL AUDIT")
                                .FontSize(24)
                                .Bold();

                            column.Item()
                                .Text("Environmental Audit Report")
                                .FontSize(12)
                                .FontColor(Colors.Grey.Darken1);
                        });
                });

                // CONTENT
                page.Content()
                    .PaddingVertical(15)
                    .Column(column =>
                    {
                        column.Spacing(18);

                        // Audit information
                        column.Item()
                            .Text("Audit Information")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(130);
                                    columns.RelativeColumn();
                                });

                                AddInfoRow(
                                    table,
                                    "Company",
                                    audit.CompanyName);

                                AddInfoRow(
                                    table,
                                    "Facility",
                                    audit.FacilityName);

                                AddInfoRow(
                                    table,
                                    "Responsible",
                                    audit.Responsible);

                                AddInfoRow(
                                    table,
                                    "Period",
                                    $"{audit.StartDate:yyyy-MM-dd} - {audit.EndDate:yyyy-MM-dd}");

                                AddInfoRow(
                                    table,
                                    "Status",
                                    audit.Status);
                            });

                        // Environmental data
                        column.Item()
                            .Text("Environmental Data")
                            .FontSize(18)
                            .Bold();

                        // Energy
                        AddSection(
                            column,
                            "Energy",
                            new[]
                            {
                                ("Electricity",
                                    $"{audit.Data.ElectricityKwh:N0} kWh"),

                                ("Natural Gas",
                                    $"{audit.Data.NaturalGasM3:N0} m³")
                            });

                        // Water
                        AddSection(
                            column,
                            "Water",
                            new[]
                            {
                                ("Water Used",
                                    $"{audit.Data.WaterUsedM3:N0} m³"),

                                ("Wastewater",
                                    $"{audit.Data.WasteWaterM3:N0} m³")
                            });

                        // Waste
                        AddSection(
                            column,
                            "Waste",
                            new[]
                            {
                                ("Hazardous Waste",
                                    $"{audit.Data.HazardousWasteKg:N0} kg"),

                                ("Non-Hazardous Waste",
                                    $"{audit.Data.NonHazardousWasteKg:N0} kg"),

                                ("Recycled Waste",
                                    $"{audit.Data.RecycledWasteKg:N0} kg")
                            });

                        // Fuel
                        AddSection(
                            column,
                            "Fuel",
                            new[]
                            {
                                ("Diesel",
                                    $"{audit.Data.DieselLiters:N0} L"),

                                ("Gasoline",
                                    $"{audit.Data.GasolineLiters:N0} L")
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

                                AddResultRow(
                                    table,
                                    "Total Emissions",
                                    $"{audit.Result.TotalEmissions:N2}");

                                AddResultRow(
                                    table,
                                    "Total Waste",
                                    $"{audit.Result.TotalWaste:N2} kg");

                                AddResultRow(
                                    table,
                                    "Recycling Rate",
                                    $"{audit.Result.RecyclingRate:N2}%");
                            });

                        // Overall score
                        column.Item()
                            .PaddingTop(10)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten2)
                            .Padding(20)
                            .AlignCenter()
                            .Column(scoreColumn =>
                            {
                                scoreColumn.Item()
                                    .Text("OVERALL SCORE")
                                    .FontSize(14)
                                    .Bold();

                                scoreColumn.Item()
                                    .PaddingTop(5)
                                    .Text(
                                        $"{audit.Result.OverallScore:N1} / 100")
                                    .FontSize(36)
                                    .Bold();

                                scoreColumn.Item()
                                    .PaddingTop(5)
                                    .Text(GetScoreDescription(
                                        audit.Result.OverallScore))
                                    .FontSize(12)
                                    .FontColor(Colors.Grey.Darken1);
                            });

                        // Score breakdown
                        column.Item()
                            .Text("Score Breakdown")
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

                                AddScoreRow(
                                    table,
                                    "Energy",
                                    audit.Result.EnergyScore);

                                AddScoreRow(
                                    table,
                                    "Water",
                                    audit.Result.WaterScore);

                                AddScoreRow(
                                    table,
                                    "Waste",
                                    audit.Result.WasteScore);

                                AddScoreRow(
                                    table,
                                    "Emissions",
                                    audit.Result.EmissionsScore);
                            });

                        // Conclusion
                        column.Item()
                            .PaddingTop(10)
                            .Text("Conclusion")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Text(GetConclusion(
                                audit.Result.OverallScore))
                            .FontSize(11);
                    });

                // FOOTER
                page.Footer()
                    .AlignCenter()
                    .Column(footer =>
                    {
                        footer.Item()
                            .Text(
                                "Environmental Audit POC - Generated automatically")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);

                        footer.Item()
                            .Text(text =>
                            {
                                text.Span("Page ");
                                text.CurrentPageNumber();
                                text.Span(" of ");
                                text.TotalPages();
                            });
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static void AddInfoRow(
        TableDescriptor table,
        string label,
        string value)
    {
        table.Cell()
            .Background(Colors.Grey.Lighten3)
            .Padding(8)
            .Text(label)
            .Bold();

        table.Cell()
            .Padding(8)
            .Text(value);
    }

    private static void AddSection(
        ColumnDescriptor column,
        string title,
        (string Label, string Value)[] rows)
    {
        column.Item()
            .Text(title)
            .FontSize(14)
            .Bold();

        column.Item()
            .Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                foreach (var row in rows)
                {
                    table.Cell()
                        .Padding(7)
                        .Text(row.Label);

                    table.Cell()
                        .Padding(7)
                        .AlignRight()
                        .Text(row.Value);
                }
            });
    }

    private static void AddResultRow(
        TableDescriptor table,
        string label,
        string value)
    {
        table.Cell()
            .Padding(8)
            .Text(label)
            .Bold();

        table.Cell()
            .Padding(8)
            .AlignRight()
            .Text(value);
    }

    private static void AddScoreRow(
        TableDescriptor table,
        string label,
        decimal score)
    {
        table.Cell()
            .Padding(8)
            .Text(label);

        table.Cell()
            .Padding(8)
            .AlignRight()
            .Text($"{score:N1} / 100");
    }

    private static string GetScoreDescription(decimal score)
    {
        if (score >= 80)
        {
            return "Strong environmental performance";
        }

        if (score >= 60)
        {
            return "Moderate environmental performance";
        }

        return "Environmental performance requires improvement";
    }

    private static string GetConclusion(decimal score)
    {
        if (score >= 80)
        {
            return
                "The audit indicates strong overall environmental performance " +
                "based on the calculated indicators.";
        }

        if (score >= 60)
        {
            return
                "The audit indicates moderate environmental performance. " +
                "There are opportunities for improvement in some environmental indicators.";
        }

        return
            "The audit indicates that several environmental indicators " +
            "could be improved. Further analysis and corrective actions are recommended.";
    }
}