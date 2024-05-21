using System.Text.Json.Serialization;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Models.Entities;

public record WaterTankLog
{
    public long WaterTankLogId { get; init; }
    public int Level { get; init; }
    public bool? PumpingWater { get; init; }
    public bool? BypassMode { get; init; }
    public DateTime LogDate { get; init; }
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public WaterTank? WaterTank { get; init; }
}