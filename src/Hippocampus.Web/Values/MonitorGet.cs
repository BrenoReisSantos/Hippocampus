using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values;

public class MonitorGet
{
    public Guid RecipientMonitorId { get; init; }
    public string Name { get; init; } = string.Empty;
    public MacAddress MacAddress { get; init; } = MacAddress.Empty;
    public RecipientType RecipientType { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public MacAddress? MonitorLinkedToMacAddress { get; init; }
}