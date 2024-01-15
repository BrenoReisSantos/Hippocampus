using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Web.Values.Forms;

public class RecipientMonitorUpdateForm
{
    public RecipientMonitorId RecipientMonitorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinHeight { get; set; }
    public int MaxHeight { get; set; }
    public RecipientType RecipientType { get; set; }
    public MacAddress? RecipientMonitorLinkedToMacAddress { get; set; } = null;
}