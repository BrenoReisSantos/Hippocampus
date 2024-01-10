namespace HippocampusWeb.Values.Forms;

public class RecipientMonitorUpdateForm
{
    public Guid RecipientMonitorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public float MinHeight { get; set; }
    public float MaxHeight { get; set; }
    public RecipientType RecipientType { get; set; }
    public string? RecipientMonitorLinkedToMacAddress { get; set; } = null;
}