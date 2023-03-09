using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Tvans.Kenteken;

public sealed class Kenteken : IEquatable<Kenteken>
{
    public int Sidecode { get; }
    
    public string Formatted { get; }

    
    private static readonly (int Sidecode, string Regex)[] Sidecodes =
    {
        new(1, "([GHJKLNPRSTXZ]{2})-?([0-9]{2})-?([0-9]{2})")
        //@TODO: add all other sidecode regex here
    };

    public Kenteken(string input)
    {
        Sidecode =  GetSidecode(input);
        Formatted = Format(input);
    }

    private string Format(string input)
    {
        var sidecode = Sidecodes
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

    public static Kenteken Parse(string input) => new Kenteken(input);

    public static bool Validate(string input) => 
        Sidecodes.Any(r => Regex.IsMatch(input, r.Regex, RegexOptions.IgnoreCase));

    public static int GetSidecode(string input) => 
        Sidecodes.FirstOrDefault(r => Regex.IsMatch(input, r.Regex, RegexOptions.IgnoreCase)) is var match
            ? match.Sidecode
            : throw new FormatException("Invalid format");

    public override string ToString() => Formatted;

    public bool Equals(Kenteken other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Sidecode == other.Sidecode && string.Equals(Formatted, other.Formatted, StringComparison.OrdinalIgnoreCase);
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