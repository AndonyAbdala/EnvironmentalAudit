namespace EnvironmentalAudit.Api.DTOs;

public class AuditResponse
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string FacilityName { get; set; } = string.Empty;

    public string Responsible { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}