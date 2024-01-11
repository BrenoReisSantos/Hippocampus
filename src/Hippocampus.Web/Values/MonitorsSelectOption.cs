using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Web.Values;

public record MonitorsSelectOption
{
    public RecipientMonitorId RecipientMonitorId { get; init; }
    public string Name { get; init; } = "";
    public string MacAddress { get; init; } = "";

    public override string ToString()
    {
        return Name;
    }
}