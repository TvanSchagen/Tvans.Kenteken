using BenchmarkDotNet.Attributes;

namespace Tvans.Kenteken.PerformanceTests;

[MemoryDiagnoser]
public class PerformanceTests
{
    [Params("99-XX-XX", "99-99-XX", "99-XX-99")]
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