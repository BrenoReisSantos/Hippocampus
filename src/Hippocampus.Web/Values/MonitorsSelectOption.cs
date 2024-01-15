using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values;

public record MonitorsSelectOption
{
    public RecipientMonitorId RecipientMonitorId { get; init; }
    public string Name { get; init; } = "";
    public MacAddress MacAddress { get; init; } = MacAddress.Empty;

    public override string ToString()
    {
        return Name;
    }
}