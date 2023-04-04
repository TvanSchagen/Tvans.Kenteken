using FluentAssertions;
using Xunit;
using FluentAssertions.Execution;

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
        Formats.GetSidecode(kenteken).Should().Be(expectedSidecode);
    }

    [Fact]
    public void Can_create_new_kenteken()
    {
        var kenteken = new Kenteken("GJ-55-55");

        kenteken.Should().NotBeNull();
    }
    
    [Fact]
    public void Can_parse()
    {
        var kenteken = Kenteken.Parse("GJ-55-55");
        
        kenteken.Should().NotBeNull();
    }
    
    [Fact]
    public void Can_create_new_kenteken_using_extension_method()
    {
        var kenteken = "GJ-55-55".ToKenteken();

        kenteken.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData("GJ5555", "GJ-55-55")]
    public void Can_format(string input, string expectedOutput)
    {
        var output = new Kenteken(input).Formatted;
        
        output.Should().Be(expectedOutput);
    }

    [Fact]
    public void To_string_will_return_formatted()
    {
        var output = new Kenteken("gj5555").ToString();
        
        output.Should().Be("GJ-55-55");
    }

    [Fact]
    public void Kenteken_with_same_format_is_considered_equal()
    {
        var kenteken1 = new Kenteken("GJ-55-55");
        var kenteken2 = new Kenteken("gj5555");

        using (new AssertionScope())
        {
            kenteken1.Equals(kenteken2).Should().BeTrue();
            (kenteken1 == kenteken2).Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    public void Try_parse_will_not_throw_on_invalid_format(string? input)
    {
        var parsed = Kenteken.TryParse(input, out var kenteken);

        using (new AssertionScope())
        {
            parsed.Should().BeFalse();
            kenteken.Should().BeNull();
        }
    }
}