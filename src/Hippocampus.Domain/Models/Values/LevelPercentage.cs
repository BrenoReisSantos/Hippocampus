using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hippocampus.Domain.Models.Values;

[JsonConverter(typeof(RecipientLevelJsonConverter))]
public class LevelPercentage
{
    public static readonly int MaximumLevel = 100;
    public static readonly int MinimumLevel = 0;
    public int Value { get; }

    public LevelPercentage()
    {
        Value = 50;
    }

    public LevelPercentage(int value)
    {
        if (!InBounds(value))
            throw RecipientLevelOutOfBounds(value);
        Value = value;
    }

    private bool InBounds(int value)
    {
        return value >= MinimumLevel && value <= MaximumLevel;
    }

    private static Exception RecipientLevelOutOfBounds(int value)
    {
        return new ArgumentException(
            $"RecipientLevel accepts values between {MinimumLevel} and {MaximumLevel}"
        );
    }

    public static implicit operator int(LevelPercentage levelPercentage)
    {
        return levelPercentage.Value;
    }

    public static implicit operator LevelPercentage(int value)
    {
        return new LevelPercentage(value);
    }
}

public class RecipientLevelJsonConverter : JsonConverter<LevelPercentage>
{
    public override LevelPercentage Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var level = reader.GetByte();
        return new LevelPercentage(level);
    }

    public override void Write(
        Utf8JsonWriter writer,
        LevelPercentage value,
        JsonSerializerOptions options
    )
    {
        writer.WriteNumberValue(value.Value);
    }
}
