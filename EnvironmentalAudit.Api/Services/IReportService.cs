using EnvironmentalAudit.Api.Models;

namespace EnvironmentalAudit.Api.Services;

public interface IReportService
{
    Task<byte[]> GenerateAuditReportAsync(Guid auditId);
}