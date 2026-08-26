using EnvironmentalAudit.Api.Data;
using EnvironmentalAudit.Api.DTOs;
using EnvironmentalAudit.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentalAudit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuditsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditResponse>>> GetAudits()
    {
        var audits = await _context.Audits
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

        return Ok(audits);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditResponse>> GetAudit(Guid id)
    {
        var audit = await _context.Audits
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

        if (audit is null)
        {
            return NotFound();
        }

        return Ok(audit);
    }

    [HttpPost]
    public async Task<ActionResult<AuditResponse>> CreateAudit(
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

        var response = new AuditResponse
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

        return CreatedAtAction(
            nameof(GetAudit),
            new { id = audit.Id },
            response);
    }
}