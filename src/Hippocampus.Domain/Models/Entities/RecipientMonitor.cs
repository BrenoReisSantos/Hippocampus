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
    public RecipientType RecipientType { get; init; }
    public string WifiSsid { get; init; } = string.Empty;
    public string WifiPassword { get; init; } = string.Empty;
    public RecipientBoundary RecipientBoundary { get; init; } = new();
    public bool IsActive { get; init; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public IEnumerable<RecipientLog> RecipientLogs { get; init; } = Enumerable.Empty<RecipientLog>();
}

public enum RecipientType : byte
{
    Cisterna,
    Caixa,
}