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
        Sidecode = GetSidecode(input);
        Formatted = Format(input);
    }

    private string Format(string input)
    {
        var sidecode = Formats.Sidecodes
            .Single(r => r.Sidecode == Sidecode);

        return Regex.Match(input, sidecode.Regex, RegexOptions.IgnoreCase)
            .Groups
            .Cast<Group>()
            .Skip(1)
            .Cast<Capture>()
            .Select(c => c.Value)
            .Aggregate(string.Empty, (a, b) => a.Length > 0 ? a + "-" + b : b)
            .ToUpperInvariant();
    }

    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent. A return value indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <param name="kenteken">Contains the Kenteken if parsing was successful, null otherwise.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string input, out Kenteken kenteken)
    {
        if (Validate(input))
        {
            kenteken = new Kenteken(input);
            return true;
        }
        
        kenteken = null;
        return false;
    }
    
    /// <summary>
    /// Converts the string representation of the Kenteken to a Kenteken equivalent.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static Kenteken Parse(string input) => new(input);

    /// <summary>
    /// Validates whether the input is a valid Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to parse.</param>
    /// <returns>true if <paramref name="input">input</paramref> was converted successfully; otherwise, false.</returns>
    public static bool Validate(string input) => input is not null && Formats.GetSidecode(input) is not null;

    /// <summary>
    /// Gets the sidecode associated with a Kenteken.
    /// </summary>
    /// <param name="input">A string containing the Kenteken to get the sidecode for.</param>
    /// <returns></returns>
    /// <exception cref="T:System.FormatException">When the Kenteken is invalid.</exception>
    public static int GetSidecode(string input) => Formats.GetSidecode(input) ?? throw new FormatException("Invalid format");

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