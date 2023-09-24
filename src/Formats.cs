using System.Text.RegularExpressions;

namespace Tvans.Kenteken;

internal static class Formats
{
    public static int? GetSidecode(ReadOnlySpan<char> input)
    {
        for (var i = 0; i < Sidecodes.Length; i++)
        {
            if (Regex.IsMatch(input, Sidecodes[i], RegexOptions.IgnoreCase))
            {
                var sidecode = i + 1;
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
}