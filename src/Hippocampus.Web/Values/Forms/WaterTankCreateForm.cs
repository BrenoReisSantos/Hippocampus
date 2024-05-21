using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values.Forms;

public record WaterTankCreateForm
{
    public string Name { get; set; } = string.Empty;
    public int LevelWhenEmpty { get; set; }
    public int LevelWhenFull { get; set; }
    public WaterTankId? WaterTankLikedToId { get; set; }
}