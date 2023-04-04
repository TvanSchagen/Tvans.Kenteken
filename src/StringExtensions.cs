namespace Tvans.Kenteken;

public static class StringExtensions
{
    /// <summary>
    /// Creates a new instance of a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static Kenteken ToKenteken(this string input) => new(input);
}