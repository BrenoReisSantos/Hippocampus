using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Domain.Diplomat.HttpOut;

public class RecipientMonitorDto
{
    public RecipientMonitorId RecipientMonitorId { get; init; } = RecipientMonitorId.New();
    public MacAddress MacAddress { get; init; } = new();
    public string Name { get; set; } = string.Empty;
    public RecipientType RecipientType { get; set; } = RecipientType.Caixa;
    public int MinHeight { get; set; }
    public int MaxHeight { get; set; }
    public MacAddress? MonitorLinkedToMacAddress { get; set; }
}