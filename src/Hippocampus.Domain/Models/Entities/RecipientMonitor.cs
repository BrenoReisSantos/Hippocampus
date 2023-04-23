using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Hippocampus.Domain.Models.Values;
using StronglyTypedIds;

namespace Hippocampus.Domain.Models.Entities;

[StronglyTypedId]
public partial struct RecipientMonitorId
{
    public static bool TryParse(string value, out RecipientMonitorId monitorId)
    {
        var couldConvert = Guid.TryParse(value, out var guid);
        monitorId = new RecipientMonitorId(guid);
        return couldConvert;
    }
}

public class RecipientMonitor
{
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.New();
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; init; } = string.Empty;
    public RecipientType RecipientType { get; init; } = RecipientType.Caixa;
    public int MinHeight { get; init; }
    public int MaxHeight { get; init; }
    public bool IsActive { get; init; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public RecipientMonitor? MonitorLinkedTo { get; set; }
    public ICollection<RecipientLog> RecipientLogs { get; init; } = new List<RecipientLog>();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecipientType : byte
{
    Cisterna,
    Caixa,
}