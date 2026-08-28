namespace EnvironmentalAudit.Api.DTOs;

public class AuditResultResponse
{
    public Guid AuditId { get; set; }

    public decimal TotalEmissions { get; set; }

    public decimal TotalWaste { get; set; }

    public decimal RecyclingRate { get; set; }

    public decimal EnergyScore { get; set; }

    public decimal WaterScore { get; set; }

    public decimal WasteScore { get; set; }

    public decimal EmissionsScore { get; set; }

    public decimal OverallScore { get; set; }
}