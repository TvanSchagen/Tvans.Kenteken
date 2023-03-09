namespace Tvans.Kenteken;

public static class StringExtensions
{
    public static Kenteken ToKenteken(this string input) => new(input);
}