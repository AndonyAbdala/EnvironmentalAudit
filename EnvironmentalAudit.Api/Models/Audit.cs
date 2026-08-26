using EnvironmentalAudit.Api.Models;

namespace EnvironmentalAudit.Api.Models;

public class Audit
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string FacilityName { get; set; } = string.Empty;

    public string Responsible { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "Draft";

    public DateTime CreatedAt { get; set; }

    public AuditData? Data { get; set; }

    public AuditResult? Result { get; set; }
}