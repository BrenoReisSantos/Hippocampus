using System.Text.Json;
using System.Text.Json.Serialization;
using Hippocampus.Models.Values;

namespace Hippocampus.Api.Configurations;

public class JsonConfiguration
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        Converters = { new MacAddressJsonConverter(), new RecipientLevelJsonConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };
}