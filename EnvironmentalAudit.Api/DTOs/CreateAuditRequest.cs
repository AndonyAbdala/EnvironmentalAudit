namespace EnvironmentalAudit.Api.DTOs;

public class CreateAuditRequest
{
    public string CompanyName { get; set; } = string.Empty;

    public string FacilityName { get; set; } = string.Empty;

    public string Responsible { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }
 
    public DateTime EndDate { get; set; }
}