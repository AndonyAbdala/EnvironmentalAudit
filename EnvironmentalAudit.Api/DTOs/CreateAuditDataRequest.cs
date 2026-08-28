using System.ComponentModel.DataAnnotations;

namespace EnvironmentalAudit.Api.DTOs;

public class CreateAuditDataRequest
{
    [Range(0, double.MaxValue)]
    public decimal ElectricityKwh { get; set; }

    [Range(0, double.MaxValue)]
    public decimal NaturalGasM3 { get; set; }

    [Range(0, double.MaxValue)]
    public decimal WaterUsedM3 { get; set; }

    [Range(0, double.MaxValue)]
    public decimal WasteWaterM3 { get; set; }

    [Range(0, double.MaxValue)]
    public decimal HazardousWasteKg { get; set; }

    [Range(0, double.MaxValue)]
    public decimal NonHazardousWasteKg { get; set; }

    [Range(0, double.MaxValue)]
    public decimal RecycledWasteKg { get; set; }

    [Range(0, double.MaxValue)]
    public decimal DieselLiters { get; set; }

    [Range(0, double.MaxValue)]
    public decimal GasolineLiters { get; set; }
}