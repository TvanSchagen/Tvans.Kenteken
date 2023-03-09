using BenchmarkDotNet.Attributes;

namespace Tvans.Kenteken.PerformanceTests;

[MemoryDiagnoser]
public class PerformanceTests
{
    [Benchmark]
    public int? GetSidecode()
    {
        return Kenteken.GetSidecode("55-GJ-GJ");
    }
    
    [Benchmark]
    public bool Validate()
    {
        return Kenteken.Validate("55-GJ-GJ");
    }

    [Benchmark]
    public string Format()
    {
        return new Kenteken("55GJGJ").Formatted;
    }
    
    [Benchmark]
    public Kenteken New()
    {
        return new Kenteken("55-GJ-GJ");
    }
    
    [Benchmark]
    public bool TryParse()
    {
        return Kenteken.TryParse("55-GJ-GJ", out _);
    }
}