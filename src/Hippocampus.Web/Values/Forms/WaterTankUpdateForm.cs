using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values.Forms;

public class WaterTankUpdateForm
{
    public WaterTankId WaterTankId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int LevelWhenEmpty { get; set; }
    public int LevelWhenFull { get; set; }
    public WaterTankType WaterTankType { get; set; }
    public WaterTankId? WaterTankLinkedToId { get; set; }
}