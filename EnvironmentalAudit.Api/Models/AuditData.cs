namespace EnvironmentalAudit.Api.Models;

public class AuditData
{
    public Guid Id { get; set; }

    public Guid AuditId { get; set; }

    // Energy
    public decimal ElectricityKwh { get; set; }

    public decimal NaturalGasM3 { get; set; }

    // Water
    public decimal WaterUsedM3 { get; set; }

    public decimal WasteWaterM3 { get; set; }

    // Waste
    public decimal HazardousWasteKg { get; set; }

    public decimal NonHazardousWasteKg { get; set; }

    public decimal RecycledWasteKg { get; set; }

    // Fuel
    public decimal DieselLiters { get; set; }

    public decimal GasolineLiters { get; set; }

    public Audit Audit { get; set; } = null!;
}