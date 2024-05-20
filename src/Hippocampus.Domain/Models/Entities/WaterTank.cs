using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Hippocampus.Domain.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Domain.Models.Entities;

[StronglyTypedId]
public partial struct WaterTankId
{
    public static bool TryParse(string value, out WaterTankId monitorId)
    {
        var couldConvert = Guid.TryParse(value, out var guid);
        monitorId = new WaterTankId(guid);
        return couldConvert;
    }

    public static implicit operator WaterTankId(string guidText)
    {
        var success = Guid.TryParse(guidText, out var guid);
        if (success) return new WaterTankId(guid);
        throw new Exception($"\"{guidText}\" não é um GUID válido");
    }
}

public record WaterTank
{
    public WaterTankId WaterTankId { get; init; } = WaterTankId.New();
    public string Name { get; set; } = string.Empty;
    public WaterTankType Type { get; set; } = WaterTankType.OnTop;
    public int CurrentLevel { get; init; }
    public WaterTankState State { get; init; } = WaterTankState.Empty;
    public int LevelWhenEmpty { get; init; }
    public int LevelWhenFull { get; init; }
    public bool? PumpingWater { get; init; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public WaterTank? PumpsTo { get; init; }
    public List<WaterTank>? PumpedFrom { get; init; }
    public IList<WaterTankLog>? WaterTankLogs { get; init; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WaterTankType : byte
{
    Cistern,
    OnTop
}