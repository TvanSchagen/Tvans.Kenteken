using Shouldly;
using Xunit;

namespace Tvans.Kenteken.FunctionalTests;

public class KentekenTests
{
    [Theory]
    [InlineData("GJ-55-55")]
    [InlineData("gj-55-55")]
    [InlineData("GJ5555")]
    [InlineData("gj5555")]
    public bool Is_a_valid_kenteken(string kenteken)
    {
        return Kenteken.Validate(kenteken);
    }

    [Theory]
    [InlineData("GJ-55-55", 1)]
    [InlineData("89-67-NR", 2)]
    [InlineData("00-FB-63", 3)]
    [InlineData("MG-51-TH", 4)]
    [InlineData("RR-HH-02", 5)]
    [InlineData("88-XD-VV", 6)]
    [InlineData("46-GZB-8", 7)]
    [InlineData("6-VGF-86", 8)]
    [InlineData("FX-149-H", 9)]
    [InlineData("V-221-FX", 10)]
    [InlineData("DHN-19-D", 11)]
    [InlineData("T-00-XXX", 12)]
    [InlineData("9-XX-999", 13)]
    [InlineData("999-XX-9", 14)]
    public void Returns_correct_sidecode(string kenteken, int expectedSidecode)
    {
        Formats.GetSidecode(kenteken).ShouldBe(expectedSidecode);
    }

    [Fact]
    public void Can_create_new_kenteken()
    {
        var kenteken = new Kenteken("GJ-55-55");

        kenteken.ShouldNotBeNull();
    }
    
    [Fact]
    public void Can_create_new_kenteken_using_extension_method()
    {
        var kenteken = "GJ-55-55".ToKenteken();

        kenteken.ShouldNotBeNull();
    }
    
    [Theory]
    [InlineData("GJ5555", "GJ-55-55")]
    public void Can_format(string input, string expectedOutput)
    {
        var output = new Kenteken(input).Formatted;
        
        output.ShouldBe(expectedOutput);
    }

    [Fact]
    public void To_string_will_return_formatted()
    {
        var output = new Kenteken("gj5555").ToString();
        
        output.ShouldBe("GJ-55-55");
    }

    [Fact]
    public void Kenteken_with_same_format_is_considered_equal()
    {
        var kenteken1 = new Kenteken("GJ-55-55");
        var kenteken2 = new Kenteken("gj5555");

        kenteken1.Equals(kenteken2).ShouldBe(true);
        (kenteken1 == kenteken2).ShouldBe(true);
    }

    [Fact]
    public void Try_parse_will_not_throw_on_invalid_format()
    {
        var parsed = Kenteken.TryParse("", out var kenteken);
        
        parsed.ShouldBeFalse();
        kenteken.ShouldBeNull();
    }
}