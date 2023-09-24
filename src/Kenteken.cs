using System.Text.RegularExpressions;

namespace Tvans.Kenteken;

public sealed class Kenteken : IEquatable<Kenteken>
{
    /// <summary>
    /// Contains the sidecode associated with the kenteken. This represents in what series it was distributed.
    /// </summary>
    public int Sidecode { get; }
    
    /// <summary>
    /// Contains the formatted kenteken, meaning uppercase with hyphens.
    /// </summary>
    public string Formatted { get; }
    
    /// <summary>
    /// Creates a new instance of a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public Kenteken(string input)
    {
        var span = input.AsSpan();
        
        Sidecode = GetSidecode(span);
        Formatted = Format(span);
    }
    
    /// <summary>
    /// Creates a new instance of a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public Kenteken(ReadOnlySpan<char> input)
    {
        Sidecode = GetSidecode(input);
        Formatted = Format(input);
    }

    private string Format(ReadOnlySpan<char> input)
    {
        var regex = Formats.Sidecodes[Sidecode - 1];
        var captures = Regex.Match(input.ToString(), regex, RegexOptions.IgnoreCase).Groups;
        var result = new Span<char>(new char[8]);
        var curLen = 0;
        
        captures[1].ValueSpan.ToUpperInvariant(destination: result.Slice(0, captures[1].ValueSpan.Length));
        curLen = captures[1].ValueSpan.Length;
        
        for (var i = 2; i < captures.Count; i++)
        {
            var capture = captures[i];
            var valueSpan = capture.ValueSpan;
            result[curLen] = '-';
            valueSpan.ToUpperInvariant(destination: result.Slice(curLen + 1, capture.Length));
            curLen += capture.Length + 1;
        }
        
        return result.ToString();
    }

    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <param name="kenteken">Contains the Kenteken if parsing was successful, null otherwise.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string input, out Kenteken kenteken) => TryParse(input.AsSpan(), out kenteken);
    
    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <param name="kenteken">Contains the Kenteken if parsing was successful, null otherwise.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> input, out Kenteken kenteken)
    {
        kenteken = null;
        
        if (input.IsEmpty || input.IsWhiteSpace()) return false;

        var sidecode = Formats.GetSidecode(input);
        if (sidecode is null) return false;
        
        kenteken = new Kenteken(input);
        return true;
    }
    
    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static Kenteken Parse(string input) => new(input.AsSpan());
    
    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static Kenteken Parse(ReadOnlySpan<char> input) => new(input);

    /// <summary>
    /// Validates whether the input is a valid Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool Validate(string input) => Validate(input.AsSpan());
    
    /// <summary>
    /// Validates whether the input is a valid Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool Validate(ReadOnlySpan<char> input) => !input.IsEmpty && !input.IsWhiteSpace() && Formats.GetSidecode(input) is not null;

    /// <summary>
    /// Gets the sidecode associated with a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to get the sidecode for.</param>
    /// <returns>The sidecode</returns>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static int GetSidecode(string input) => GetSidecode(input.AsSpan());
    
    /// <summary>
    /// Gets the sidecode associated with a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to get the sidecode for.</param>
    /// <returns>The sidecode</returns>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static int GetSidecode(ReadOnlySpan<char> input) => Formats.GetSidecode(input) ?? throw new FormatException("Invalid format");

    public override string ToString() => Formatted;

    public bool Equals(Kenteken other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Formatted, other.Formatted, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Kenteken) obj);
    }

    public static bool operator ==(Kenteken a, Kenteken b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (ReferenceEquals(a, null)) return false;
        if (ReferenceEquals(b, null)) return false;
        return a.Equals(b);
    }

    public static bool operator !=(Kenteken a, Kenteken b) => !(a == b);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Sidecode * 397) ^ (Formatted != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Formatted) : 0);
        }
    }
}