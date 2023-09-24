using System.Text.RegularExpressions;

namespace Tvans.Kenteken;

internal static partial class Formats
{
    public static int? GetSidecode(ReadOnlySpan<char> input)
    {
        var sidecode = GetsidecodeFromRegex(input);
        if (sidecode is null) return null;
        
        if (sidecode >= 13)
        {
            if (ContainsDisallowedSequence(input, DisallowedFromSidecode13Onwards)) return null;
            return sidecode;
        }

        if (sidecode >= 8)
        {
            if (ContainsDisallowedSequence(input, DisallowedFromSidecode8Onwards)) return null;
            return sidecode;
        }

        if (ContainsDisallowedSequence(input, Disallowed)) return null;
        return sidecode;
    }

    private static int? GetsidecodeFromRegex(ReadOnlySpan<char> input)
    {
        if (SideCode1Regex().IsMatch(input)) return 1;
        if (SideCode2Regex().IsMatch(input)) return 2;
        if (SideCode3Regex().IsMatch(input)) return 3;
        if (SideCode4Regex().IsMatch(input)) return 4;
        if (SideCode5Regex().IsMatch(input)) return 5;
        if (SideCode6Regex().IsMatch(input)) return 6;
        if (SideCode7Regex().IsMatch(input)) return 7;
        if (SideCode8Regex().IsMatch(input)) return 8;
        if (SideCode9Regex().IsMatch(input)) return 9;
        if (SideCode10Regex().IsMatch(input)) return 10;
        if (SideCode11Regex().IsMatch(input)) return 11;
        if (SideCode12Regex().IsMatch(input)) return 12;
        if (SideCode13Regex().IsMatch(input)) return 13;
        if (SideCode14Regex().IsMatch(input)) return 14;
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

    public static readonly string[] Sidecodes =
    {
        $"([{AllowedInSidecode1}]{{2}})-?([0-9]{{2}})-?([0-9]{{2}})", // AB-99-99
        $"([0-9]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode2}]{{2}})", // 89-67-NR
        $"([0-9]{{2}})-?([{AllowedInSidecode3}]{{2}})-?([0-9]{{2}})", // 00-FB-63
        $"([{AllowedInSidecode4}]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode4}]{{2}})", // MG-51-TH
        $"([{AllowedInSidecode5}]{{2}})-?([{AllowedInSidecode5}]{{2}})-?([0-9]{{2}})", // RR-HH-02
        $"([0-9]{{2}})-?([{AllowedInSidecode6}]{{2}})-?([{AllowedInSidecode6}]{{2}})", // 88-XD-VV
        $"([0-9]{{2}})-?([{AllowedInSidecode7}]{{3}})-?([0-9]{{1}})", // 46-GZB-8
        $"([0-9]{{1}})-?([{AllowedInSidecode8}]{{3}})-?([0-9]{{2}})", // 6-VGF-86
        $"([{AllowedInSidecode9}]{{2}})-?([0-9]{{3}})?-([{AllowedInSidecode9}]{{1}})", // FX-149-H
        $"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{3}})?-([{AllowedInSidecode10Onwards}]{{2}})", // V-221-FX
        $"([{AllowedInSidecode10Onwards}]{{3}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{1}})", // DHN-19-D
        $"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{3}})", // T-00-XXX
        $"([0-9]{{1}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{3}})", // 9-XX-999
        $"([0-9]{{3}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{1}})", // 999-XX-9
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

    private static readonly string[] DisallowedFromSidecode8Onwards =
        {"SD", "SS", "SA", "WA", "GVD", "KKK", "LPF", "NSB", "PKK", "PSV", "PVV", "SGP", "TBS", "KVT", "VVD"};

    private static readonly string[] DisallowedFromSidecode13Onwards =
        {"SD", "SS", "SA", "WA", "GVD", "KKK", "LPF", "NSB", "PKK", "PSV", "PVV", "SGP", "TBS", "KVT", "VVD", "SP"};

    [GeneratedRegex($"([{AllowedInSidecode1}]{{2}})-?([0-9]{{2}})-?([0-9]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode1Regex();

    [GeneratedRegex($"([0-9]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode2}]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode2Regex();

    [GeneratedRegex($"([0-9]{{2}})-?([{AllowedInSidecode3}]{{2}})-?([0-9]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode3Regex();

    [GeneratedRegex($"([{AllowedInSidecode4}]{{2}})-?([0-9]{{2}})-?([{AllowedInSidecode4}]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode4Regex();

    [GeneratedRegex($"([{AllowedInSidecode5}]{{2}})-?([{AllowedInSidecode5}]{{2}})-?([0-9]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode5Regex();

    [GeneratedRegex($"([0-9]{{2}})-?([{AllowedInSidecode6}]{{2}})-?([{AllowedInSidecode6}]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode6Regex();

    [GeneratedRegex($"([0-9]{{2}})-?([{AllowedInSidecode7}]{{3}})-?([0-9]{{1}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode7Regex();

    [GeneratedRegex($"([0-9]{{1}})-?([{AllowedInSidecode8}]{{3}})-?([0-9]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode8Regex();

    [GeneratedRegex($"([{AllowedInSidecode9}]{{2}})-?([0-9]{{3}})?-([{AllowedInSidecode9}]{{1}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode9Regex();

    [GeneratedRegex($"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{3}})?-([{AllowedInSidecode10Onwards}]{{2}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode10Regex();

    [GeneratedRegex($"([{AllowedInSidecode10Onwards}]{{3}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{1}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode11Regex();

    [GeneratedRegex($"([{AllowedInSidecode10Onwards}]{{1}})-?([0-9]{{2}})?-([{AllowedInSidecode10Onwards}]{{3}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode12Regex();

    [GeneratedRegex($"([0-9]{{1}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{3}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode13Regex();

    [GeneratedRegex($"([0-9]{{3}})-?(([{AllowedInSidecode10Onwards}]{{2}})?-[0-9]{{1}})",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex SideCode14Regex();
}