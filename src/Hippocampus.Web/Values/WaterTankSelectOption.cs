using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values;

public record WaterTankSelectOption
{
    public WaterTankId WaterTankId { get; init; }
    public string Name { get; init; } = "";
    public WaterTankId WaterTankLinkedToId { get; init; }

    public override string ToString()
    {
        return Name;
    }
}
