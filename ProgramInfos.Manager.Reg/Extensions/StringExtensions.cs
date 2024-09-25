using System.Text;
using System.Text.RegularExpressions;

namespace ProgramInfos.Manager.Reg.Extensions;

/// <summary>
/// Extensions for strings.
/// </summary>
public static partial class StringExtensions
{
    /// <summary>
    /// Converts a string to a readable format by removing control characters.
    /// </summary>
    /// <param name="input">The input string to be converted.</param>
    /// <returns>A new string with control characters removed up to the first occurrence of such a character.</returns>
    public static string GetReadable(this string input)
    {
        var result = new StringBuilder();

        foreach (var c in input)
        {
            if (!char.IsControl(c))
                result.Append(c);
            else
                break;
        }

        return result.ToString();
    }

    /// <summary>
    /// Removes invalid characters from a file name.
    /// </summary>
    /// <param name="fileName">The file name to sanitize.</param>
    /// <returns>A sanitized file name.</returns>
    public static string SanitizeFileName(this string fileName)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars());
        var pattern = "[" + Regex.Escape(invalidChars) + "]";
        return Regex.Replace(fileName, pattern, "_");
    }

    /// <summary>
    /// Checks if two strings are similar after removing certain characters and converting to lower case.
    /// </summary>
    /// <param name="string1">The first string to compare.</param>
    /// <param name="string2">The second string to compare.</param>
    /// <returns><c>true</c> if either string contains the other after processing; otherwise, <c>false</c>.</returns>
    public static bool ContainsGeneralized(this string string1, string string2)
    {
        var generalized1 = RemoveChars().Replace(string1, "").ToLower();
        var generalized2 = RemoveChars().Replace(string2, "").ToLower();

        return generalized1.Contains(generalized2) || generalized2.Contains(generalized1);
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex RemoveChars();
}
