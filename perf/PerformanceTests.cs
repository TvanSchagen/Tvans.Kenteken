using BenchmarkDotNet.Attributes;

namespace Tvans.Kenteken.PerformanceTests;

[MemoryDiagnoser]
public class PerformanceTests
{
    [Params(
        "99-XX-XX", 
        "99-99-XX",
        "99-XX-99",
        "XX-99-XX",
        "XX-XX-99",
        "99-XX-XX",
        "99-XXX-9",
        "9-XXX-99",
        "XX-999-X",
        "X-999-XX",
        "XXX-99-X",
        "X-99-XXX",
        "9-XX-999",
        "999-XX-9"
    )]
    public string Input { get; set; } = default!;
    
    [Benchmark]
    public int? GetSidecode()
    {
        return Kenteken.GetSidecode(Input);
    }
    
    [Benchmark]
    public Kenteken New()
    {
        return new Kenteken(Input);
    }
}