using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Hippocampus.Domain.Models.Values;

[JsonConverter(typeof(MacAddressJsonConverter))]
public partial class MacAddress : IEquatable<MacAddress>
{
    public const int DefaultLength = 12;
    public string Value { get; }

    const string _pattern =
        @"^(?:[0-9A-Fa-f]{2}([:-]?)[0-9A-Fa-f]{2})(?:(?:\1|\.)(?:[0-9A-Fa-f]{2}([:-]?)[0-9A-Fa-f]{2})){2}$";

    public enum Mask
    {
        Colon,
        Hyphen,
        Dot,
        None
    }

    public MacAddress() => Value = new string('0', DefaultLength);

    public MacAddress(string macAddress)
    {
        if (!Validate(macAddress))
            throw MacAddressException(macAddress);
        Value = macAddress.CleanMask();
    }

    static Exception MacAddressException(string macAddress) =>
        new FormatException($"Invalid Mac Address: {macAddress}");

    public MacAddress Empty() => new();

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "pt-BR")]
    private static partial Regex MacAddressRegex();

    public static bool Validate(string macAddress) => MacAddressRegex().Match(macAddress).Success;

    public static implicit operator string(MacAddress macAddress) => macAddress.ToString();

    public string ToString(Mask mask = Mask.None) => Format(mask);
    public override string ToString() => Format(Mask.Colon);

    private string Format(Mask mask) =>
        mask switch
        {
            Mask.Colon =>
                Value.FormatMask("##:##:##:##:##:##"),
            Mask.Dot =>
                Value.FormatMask("####.####.####"),
            Mask.Hyphen =>
                Value.FormatMask("##-##-##-##-##-##"),
            Mask.None =>
                Value.FormatMask("############"),
            _ =>
                Value,
        };

    public bool Equals(MacAddress? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MacAddress)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}

public class MacAddressJsonConverter : JsonConverter<MacAddress>
{
    public override MacAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var macAddressString = reader.GetString();
        return macAddressString is null ? new MacAddress() : new MacAddress(macAddressString);
    }

    public override void Write(Utf8JsonWriter writer, MacAddress value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(MacAddress.Mask.Colon));
    }
}