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
    private readonly IReportService _reportService;

    public AuditsController(
        IAuditService auditService,
        ICalculationService calculationService,
        IReportService reportService)
    {
        _auditService = auditService;
        _calculationService = calculationService;
        _reportService = reportService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuditResponse>>> GetAudits()
    {
        var audits = await _auditService.GetAuditsAsync();

        return Ok(audits);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(StatusCodes.Status201Created)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [HttpGet("{id:guid}/report")]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetReport(Guid id)
    {
        try
        {
            var pdf = await _reportService.GenerateAuditReportAsync(id);

            return File(
                pdf,
                "application/pdf",
                $"environmental-audit-{id}.pdf");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}