using EnvironmentalAudit.Api.DTOs;

namespace EnvironmentalAudit.Api.Services;

public interface IAuditService
{
    Task<IEnumerable<AuditResponse>> GetAuditsAsync();

    Task<AuditResponse?> GetAuditByIdAsync(Guid id);

    Task<AuditResponse> CreateAuditAsync(CreateAuditRequest request);
}