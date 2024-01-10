namespace HippocampusWeb.Values;

public class MonitorGet
{
    public Guid RecipientMonitorId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string MacAddress { get; init; } = string.Empty;
    public RecipientType RecipientType { get; init; }
    public int MaxHeight { get; init; }
    public int MinHeight { get; init; }
    public string monitorLinkedToMacAddress { get; init; } = string.Empty;
}