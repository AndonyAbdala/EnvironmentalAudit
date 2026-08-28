using EnvironmentalAudit.Api.DTOs;

namespace EnvironmentalAudit.Api.Services;

public interface ICalculationService
{
    Task<AuditResultResponse> CalculateAsync(
        Guid auditId,
        CreateAuditDataRequest request);
}