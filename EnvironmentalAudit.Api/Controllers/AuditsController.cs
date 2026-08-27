using EnvironmentalAudit.Api.DTOs;
using EnvironmentalAudit.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentalAudit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditsController : ControllerBase
{
    private readonly IAuditService _auditService;

    public AuditsController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditResponse>>> GetAudits()
    {
        var audits = await _auditService.GetAuditsAsync();

        return Ok(audits);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditResponse>> GetAudit(Guid id)
    {
        var audit = await _auditService.GetAuditByIdAsync(id);

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
        var audit = await _auditService.CreateAuditAsync(request);

        return CreatedAtAction(
            nameof(GetAudit),
            new { id = audit.Id },
            audit);
    }
}