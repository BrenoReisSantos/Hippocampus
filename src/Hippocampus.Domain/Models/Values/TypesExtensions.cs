using System.Text.RegularExpressions;

namespace Hippocampus.Domain.Models.Values;

public static class TypesExtensions
{
    public static string FormatMask(this string value, string mask)
    {
        return string.IsNullOrEmpty(value) ? string.Empty : value.Mask(mask);
    }

    private static string Mask(this string value, string mask, char substituteChar = '#')
    {
        var result = mask.Copy(mask.Length);
        var step = 0;
        for (var index = 0; index < mask.Length; index++)
            if (result[index].CompareTo(substituteChar) == 0)
                result[index] = value[step++];
            else
                result[index] = mask[index];

        return new string(result);
    }

    public static char[] Copy(this string value, int size)
    {
        var result = new char[size];
        for (var index = 0; index < size; index++)
            result[index] = value[index];
        return result;
    }

    public static string CleanMask(this string value)
    {
        var cleanedString = "";
        foreach (var character in value)
            if (!character.HasSymbol())
                cleanedString += character;

        return cleanedString;
    }

    private static bool HasSymbol(this string value)
    {
        return new Regex(@"\W").Match(value).Success;
    }

    private static bool HasSymbol(this char value)
    {
        return new Regex(@"\W").Match(value.ToString()).Success;
    }
}
