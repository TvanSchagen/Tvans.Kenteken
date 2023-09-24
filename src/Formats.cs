using System.Text.RegularExpressions;

namespace Tvans.Kenteken;

internal static class Formats
{
    public static int? GetSidecode(string input) =>
        Sidecodes.FirstOrDefault(r => Regex.IsMatch(input, r.Regex, RegexOptions.IgnoreCase)) switch
        {
            (var sidecode and >= 13, _) => DisallowedFromSidecode13Onwards.Any(input.Contains) ? null : sidecode,
            (var sidecode and >= 8, _) => DisallowedFromSidecode8Onwards.Any(input.Contains) ? null : sidecode,
            (var sidecode and >= 1, _) => Disallowed.Any(input.Contains) ? null : sidecode,
            (_, _) => null
        };

    public static int? GetSidecode(ReadOnlySpan<char> input)
    {
        for (var i = 0; i < Sidecodes.Length; i++)
        {
            if (Regex.IsMatch(input, Sidecodes[i].Regex, RegexOptions.IgnoreCase))
            {
                if (Sidecodes[i].Sidecode >= 13)
                {
                    if (ContainsDisallowedSequence(input, DisallowedFromSidecode13Onwards)) return null;
                    return Sidecodes[i].Sidecode;
                }

                if (Sidecodes[i].Sidecode >= 8)
                {
                    if (ContainsDisallowedSequence(input, DisallowedFromSidecode8Onwards)) return null;
                    return Sidecodes[i].Sidecode;
                }

                if (ContainsDisallowedSequence(input, Disallowed)) return null;
                return Sidecodes[i].Sidecode;
            }
        }

        return null;
    }

    private static bool ContainsDisallowedSequence(ReadOnlySpan<char> input, string[] disallowed)
    {
        for (var i = 0; i < disallowed.Length; i++)
        {
            var span = disallowed[i].AsSpan();
            if (input.Contains(span, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public static readonly (int Sidecode, string Regex)[] Sidecodes =
    {
        new(1, $"([{AllowedInSidecode1}]{{2}})-?([0-9]{{2}})-?([0-9]{{2}})"), // AB-99-99
        new(2, $"([0-9]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode2}]{{2}})"), // 89-67-NR
        new(3, $"([0-9]{{2}})-?([{AllowedInSidecode3}]{{2}})-?([0-9]{{2}})"), // 00-FB-63
        new(4, $"([{AllowedInSidecode4}]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode4}]{{2}})"), // MG-51-TH
        new(5, $"([{AllowedInSidecode5}]{{2}})-?([{AllowedInSidecode5}]{{2}})-?([0-9]{{2}})"), // RR-HH-02
        new(6, $"([0-9]{{2}})-?([{AllowedInSidecode6}]{{2}})-?([{AllowedInSidecode6}]{{2}})"), // 88-XD-VV
        new(7, $"([0-9]{{2}})-?([{AllowedInSidecode7}]{{3}})-?([0-9]{{1}})"), // 46-GZB-8
        new(8, $"([0-9]{{1}})-?([{AllowedInSidecode8}]{{3}})-?([0-9]{{2}})"), // 6-VGF-86
        new(9, $"([{AllowedInSidecode9}]{{2}})-?([0-9]{{3}})?-([{AllowedInSidecode9}]{{1}})"), // FX-149-H
        new(10, $"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{3}})?-([{AllowedInSidecode10Onwards}]{{2}})"), // V-221-FX
        new(11, $"([{AllowedInSidecode10Onwards}]{{3}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{1}})"), // DHN-19-D
        new(12, $"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{3}})"), // T-00-XXX
        new(13, $"([0-9]{{1}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{3}})"), // 9-XX-999
        new(14, $"([0-9]{{3}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{1}})"), // 999-XX-9
    };

    private const string AllowedInSidecode1 = "ABDEFGHJKLMNOPRSTUVXZ";
    private const string AllowedInSidecode2 = "ABDEFGHIJMNOPRSTUVXZ";
    private const string AllowedInSidecode3 = "ABDEFGHIJMNOPRSTUVXYZ";
    private const string AllowedInSidecode4 = "BDFGHJKLMNOPRSTVWXYZ";
    private const string AllowedInSidecode5 = "BDFGHJLMNOPRSTVWXZ";
    private const string AllowedInSidecode6 = "BDFGHJLMNOPRSTVWXZ";
    private const string AllowedInSidecode7 = "BDFGHJKLMNOPRSTVWXZ";
    private const string AllowedInSidecode8 = "BDFGHJKMNOPRSTVWXZ";
    private const string AllowedInSidecode9 = "BDFGHJKMNOPRSTVWXZ";
    private const string AllowedInSidecode10Onwards = "BDFGHJKLMNOPRSTVWXZ";

    private static readonly string[] Disallowed =
        {"SD", "SS", "SA", "WA", "GVD", "KKK", "LPF", "NSB", "PKK", "PSV", "PVV", "SGP", "TBS"};

    private static readonly string[] DisallowedFromSidecode8Onwards = Disallowed.Concat(new[] {"KVT, VVD"}).ToArray();

    private static readonly string[] DisallowedFromSidecode13Onwards =
        DisallowedFromSidecode8Onwards.Concat(new[] {"SP"}).ToArray();
}