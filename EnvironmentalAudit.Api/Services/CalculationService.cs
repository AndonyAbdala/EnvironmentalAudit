using EnvironmentalAudit.Api.Data;
using EnvironmentalAudit.Api.DTOs;
using EnvironmentalAudit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentalAudit.Api.Services;

public class CalculationService : ICalculationService
{
    private readonly AppDbContext _context;

    public CalculationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AuditResultResponse> CalculateAsync(
        Guid auditId,
        CreateAuditDataRequest request)
    {
        var audit = await _context.Audits
            .Include(a => a.Data)
            .Include(a => a.Result)
            .FirstOrDefaultAsync(a => a.Id == auditId);

        if (audit is null)
        {
            throw new KeyNotFoundException(
                $"Audit with id {auditId} was not found.");
        }

        // Save environmental data
        var auditData = new AuditData
        {
            Id = Guid.NewGuid(),
            AuditId = auditId,

            ElectricityKwh = request.ElectricityKwh,
            NaturalGasM3 = request.NaturalGasM3,

            WaterUsedM3 = request.WaterUsedM3,
            WasteWaterM3 = request.WasteWaterM3,

            HazardousWasteKg = request.HazardousWasteKg,
            NonHazardousWasteKg = request.NonHazardousWasteKg,
            RecycledWasteKg = request.RecycledWasteKg,

            DieselLiters = request.DieselLiters,
            GasolineLiters = request.GasolineLiters
        };

        if (audit.Data is not null)
        {
            audit.Data.ElectricityKwh = request.ElectricityKwh;
            audit.Data.NaturalGasM3 = request.NaturalGasM3;
            audit.Data.WaterUsedM3 = request.WaterUsedM3;
            audit.Data.WasteWaterM3 = request.WasteWaterM3;
            audit.Data.HazardousWasteKg = request.HazardousWasteKg;
            audit.Data.NonHazardousWasteKg = request.NonHazardousWasteKg;
            audit.Data.RecycledWasteKg = request.RecycledWasteKg;
            audit.Data.DieselLiters = request.DieselLiters;
            audit.Data.GasolineLiters = request.GasolineLiters;

            auditData = audit.Data;
        }
        else
        {
            _context.AuditData.Add(auditData);
        }

        // ------------------------------------------------
        // Fake calculations for the POC
        // ------------------------------------------------

        var totalEnergy =
            request.ElectricityKwh +
            request.NaturalGasM3 * 10;

        var totalWaste =
            request.HazardousWasteKg +
            request.NonHazardousWasteKg;

        var recyclingRate =
            totalWaste > 0
                ? (request.RecycledWasteKg / totalWaste) * 100
                : 0;

        var totalEmissions =
            request.ElectricityKwh * 0.0004m +
            request.NaturalGasM3 * 0.002m +
            request.DieselLiters * 0.0027m +
            request.GasolineLiters * 0.0023m;

        // Fake scores between 0 and 100
        var energyScore = Math.Max(
            0,
            Math.Min(100, 100 - totalEnergy / 10000));

        var waterScore = Math.Max(
            0,
            Math.Min(
                100,
                100 - request.WaterUsedM3 / 100));

        var wasteScore = Math.Max(
            0,
            Math.Min(
                100,
                recyclingRate));

        var emissionsScore = Math.Max(
            0,
            Math.Min(
                100,
                100 - totalEmissions));

        var overallScore =
            (energyScore +
             waterScore +
             wasteScore +
             emissionsScore) / 4;

        // ------------------------------------------------
        // Save result
        // ------------------------------------------------

        AuditResult result;

        if (audit.Result is not null)
        {
            result = audit.Result;

            result.TotalEmissions = totalEmissions;
            result.TotalWaste = totalWaste;
            result.RecyclingRate = recyclingRate;
            result.EnergyScore = energyScore;
            result.WaterScore = waterScore;
            result.WasteScore = wasteScore;
            result.EmissionsScore = emissionsScore;
            result.OverallScore = overallScore;
        }
        else
        {
            result = new AuditResult
            {
                Id = Guid.NewGuid(),
                AuditId = auditId,
                TotalEmissions = totalEmissions,
                TotalWaste = totalWaste,
                RecyclingRate = recyclingRate,
                EnergyScore = energyScore,
                WaterScore = waterScore,
                WasteScore = wasteScore,
                EmissionsScore = emissionsScore,
                OverallScore = overallScore
            };

            _context.AuditResults.Add(result);
        }

        audit.Status = "Completed";

        await _context.SaveChangesAsync();

        return new AuditResultResponse
        {
            AuditId = auditId,
            TotalEmissions = result.TotalEmissions,
            TotalWaste = result.TotalWaste,
            RecyclingRate = result.RecyclingRate,
            EnergyScore = result.EnergyScore,
            WaterScore = result.WaterScore,
            WasteScore = result.WasteScore,
            EmissionsScore = result.EmissionsScore,
            OverallScore = result.OverallScore
        };
    }
}