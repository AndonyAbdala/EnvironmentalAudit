using EnvironmentalAudit.Api.DTOs;
using EnvironmentalAudit.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnvironmentalAudit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditsController : ControllerBase
{
    private readonly IAuditService _auditService;
    private readonly ICalculationService _calculationService;

    public AuditsController(IAuditService auditService, ICalculationService calculationService)
    {
        _auditService = auditService;
        _calculationService = calculationService;
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

    [HttpPost("{id:guid}/calculate")]
    public async Task<ActionResult<AuditResultResponse>> Calculate(
    Guid id,
    CreateAuditDataRequest request)
    {
        try
        {
            var result = await _calculationService.CalculateAsync(
                id,
                request);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}