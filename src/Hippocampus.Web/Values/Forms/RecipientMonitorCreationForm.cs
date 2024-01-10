namespace HippocampusWeb.Values.Forms;

public class RecipientMonitorCreationForm
{
    public string Name { get; set; } = string.Empty;
    public string MacAddress { get; set; } = string.Empty;
    public float MinHeight { get; set; }
    public float MaxHeight { get; set; }
    public RecipientType RecipientType { get; set; }
    public string? RecipientMonitorLinkedToMacAddress { get; set; } = null;
}