using System.Text.Json.Serialization;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Models.Entities;

public record WaterTankLog
{
    public long WaterTankLogId { get; init; }
    public int Level { get; init; }
    public WaterTankState State { get; init; } = WaterTankState.Empty;
    public bool? PumpingWater { get; init; }
    public DateTime LogDate { get; init; }
    public WaterTankId WaterTankId { get; init; } = WaterTankId.Empty;
    public WaterTank? WaterTank { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WaterTankState
{
    Empty,
    Average,
    Full
}