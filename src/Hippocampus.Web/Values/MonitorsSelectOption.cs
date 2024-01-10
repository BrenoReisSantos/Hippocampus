namespace HippocampusWeb.Values;

public record MonitorsSelectOption
{
    public Guid RecipientMonitorId { get; init; }
    public string Name { get; init; } = "";
    public string MacAddress { get; init; } = "";

    public override string ToString()
    {
        return Name;
    }
}