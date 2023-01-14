using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hippocampus.Models.Values;

[JsonConverter(typeof(RecipientLevelJsonConverter))]
public class RecipientLevel
{
    public static readonly byte MaximumLevel = 100;
    public static readonly byte MinimumLevel = 0;
    public byte Value { get; }

    public RecipientLevel() => Value = 50;

    public RecipientLevel(byte value)
    {
        if (!InBounds(value)) throw RecipientLevelOutOfBounds(value);
        Value = value;
    }

    bool InBounds(byte value) => value >= MinimumLevel && value <= MaximumLevel;

    static Exception RecipientLevelOutOfBounds(int value) =>
        new ArgumentException($"RecipientLevel accepts values between {MinimumLevel} and {MaximumLevel}");

    public static implicit operator byte(RecipientLevel recipientLevel) => recipientLevel.Value;
    public static implicit operator RecipientLevel(byte value) => new(value);
}

public class RecipientLevelJsonConverter : JsonConverter<RecipientLevel>
{
    public override RecipientLevel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var level = reader.GetByte();
        return new RecipientLevel(level);
    }

    public override void Write(Utf8JsonWriter writer, RecipientLevel value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}