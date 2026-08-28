using System.ComponentModel.DataAnnotations;

namespace EnvironmentalAudit.Api.DTOs;

public class CreateAuditRequest
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public string FacilityName { get; set; } = string.Empty;

    [Required]
    public string Responsible { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}