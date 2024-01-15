using Hippocampus.Domain.Models.Values;
using RecipientType = Hippocampus.Domain.Models.Entities.RecipientType;

namespace Hippocampus.Web.Values.Forms;

public class RecipientMonitorCreationForm
{
    public string Name { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public int MinHeight { get; set; }
    public int MaxHeight { get; set; }
    public RecipientType RecipientType { get; set; }
    public string? RecipientMonitorLinkedToMacAddress { get; set; }
}