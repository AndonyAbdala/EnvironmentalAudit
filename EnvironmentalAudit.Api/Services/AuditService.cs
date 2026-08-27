using EnvironmentalAudit.Api.Data;
using EnvironmentalAudit.Api.DTOs;
using EnvironmentalAudit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentalAudit.Api.Services;

public class AuditService : IAuditService
{
    private readonly AppDbContext _context;

    public AuditService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AuditResponse>> GetAuditsAsync()
    {
        return await _context.Audits
            .AsNoTracking()
            .Select(a => new AuditResponse
            {
                Id = a.Id,
                CompanyName = a.CompanyName,
                FacilityName = a.FacilityName,
                Responsible = a.Responsible,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<AuditResponse?> GetAuditByIdAsync(Guid id)
    {
        return await _context.Audits
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AuditResponse
            {
                Id = a.Id,
                CompanyName = a.CompanyName,
                FacilityName = a.FacilityName,
                Responsible = a.Responsible,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                Status = a.Status,
                CreatedAt = a.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AuditResponse> CreateAuditAsync(
        CreateAuditRequest request)
    {
        var audit = new Audit
        {
            Id = Guid.NewGuid(),
            CompanyName = request.CompanyName,
            FacilityName = request.FacilityName,
            Responsible = request.Responsible,
            StartDate = request.StartDate.ToUniversalTime(),
            EndDate = request.EndDate.ToUniversalTime(),
            Status = "Draft",
            CreatedAt = DateTime.UtcNow.ToUniversalTime()
        };

        _context.Audits.Add(audit);

        await _context.SaveChangesAsync();

        return new AuditResponse
        {
            Id = audit.Id,
            CompanyName = audit.CompanyName,
            FacilityName = audit.FacilityName,
            Responsible = audit.Responsible,
            StartDate = audit.StartDate,
            EndDate = audit.EndDate,
            Status = audit.Status,
            CreatedAt = audit.CreatedAt
        };
    }
}