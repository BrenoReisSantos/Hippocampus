using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hippocampus.Domain.Models.Values;

[JsonConverter(typeof(RecipientLevelJsonConverter))]
public class LevelPercentage
{
    public static readonly byte MaximumLevel = 100;
    public static readonly byte MinimumLevel = 0;
    public byte Value { get; }

    public LevelPercentage() => Value = 50;

    public LevelPercentage(byte value)
    {
        if (!InBounds(value)) throw RecipientLevelOutOfBounds(value);
        Value = value;
    }

    bool InBounds(byte value) => value >= MinimumLevel && value <= MaximumLevel;

    static Exception RecipientLevelOutOfBounds(int value) =>
        new ArgumentException($"RecipientLevel accepts values between {MinimumLevel} and {MaximumLevel}");

    public static implicit operator byte(LevelPercentage levelPercentage) => levelPercentage.Value;
    public static implicit operator LevelPercentage(byte value) => new(value);
}

public class RecipientLevelJsonConverter : JsonConverter<LevelPercentage>
{
    public override LevelPercentage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var level = reader.GetByte();
        return new LevelPercentage(level);
    }

    public override void Write(Utf8JsonWriter writer, LevelPercentage value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}