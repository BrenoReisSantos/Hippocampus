using System.Text.Json.Serialization;

namespace HippocampusWeb.Values;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RecipientType : byte
{
    Caixa,
    Cisterna
}