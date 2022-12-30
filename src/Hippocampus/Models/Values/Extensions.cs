namespace Hippocampus.Models.Values;
public static class TypesExtensions
{
    public static string FormatMask(this string value, string mask)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value.Mask(mask);
    }

    static string Mask(this string value, string mask, char substituteChar = '#')
    {
        char[] result = mask.Copy(mask.Length);
        int step = 0;
        for (int index = 0; index < mask.Length; index++)
        {
            if (result[index].CompareTo(substituteChar) == 0)
                result[index] = value[step++];
            else result[index] = mask[index];
        }
        return new string(result);
    }

    public static char[] Copy(this string value, int size)
    {
        char[] result = new char[size];
        for (int index = 0; index < size; index++)
            result[index] = value[index];
        return result;
    }
}
