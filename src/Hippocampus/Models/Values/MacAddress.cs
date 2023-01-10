using System.Text.RegularExpressions;

namespace Hippocampus.Models.Values;
public partial class MacAddress
{
    public const int DefaultLength = 12;
    public string Value { get; }

    const string _pattern = @"^(?:[0-9A-Fa-f]{2}([:-]?)[0-9A-Fa-f]{2})(?:(?:\1|\.)(?:[0-9A-Fa-f]{2}([:-]?)[0-9A-Fa-f]{2})){2}$";

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
        Value = Format(Mask.Colon);
    }

    static Exception MacAddressException(string macAddress) => new FormatException($"Invalid Mac Address: {macAddress}");

    public MacAddress Empty() => new();

    [GeneratedRegex(_pattern, RegexOptions.IgnoreCase, "pt-BR")]
    private static partial Regex MacAddressRegex();

    public static bool Validate(string macAddress) => MacAddressRegex().Match(macAddress).Success;

    public static implicit operator string(MacAddress macAddress) => macAddress;

    public override string ToString() => Value;

    public string Format(Mask mask = Mask.None) =>
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
}
