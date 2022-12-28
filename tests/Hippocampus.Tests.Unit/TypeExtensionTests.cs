using Hippocampus.Types;

namespace Hippocampus.Tests.Unit;

public class TypeExtensionTests : BaseTest
{
    [TestCase("123456789ABC", "####_####_####", "1234_5678_9ABC")]
    [TestCase("000000000000", "####_####_####", "0000_0000_0000")]
    [TestCase("AAAAAAAAAAAA", "####_####_####", "AAAA_AAAA_AAAA")]
    [TestCase("123456789111", "###.###.###.###", "123.456.789.111")]
    [TestCase("123456789ABC", "###.###.###.###", "123.456.789.ABC")]
    [TestCase("AAAAAAAAAAAA", "###.###.###.###", "AAA.AAA.AAA.AAA")]
    [TestCase("123456789", "#########", "123456789")]
    [TestCase("ABCDEFGHI", "#########", "ABCDEFGHI")]
    [TestCase("12345678912", "###.###.###-##", "123.456.789-12")]
    [TestCase("123456789AB", "###.###.###-##", "123.456.789-AB")]
    [TestCase("ABCDEFGHIJK", "###.###.###-##", "ABC.DEF.GHI-JK")]
    public void FormatMaskShouldReturnWithCorrectFormat(string value, string format, string expected)
    {
        string maskedString = value.FormatMask(format);
        maskedString.Should().Be(expected);
    }

    [Test]
    public void FormatMaskShoulReturnSameStringAsPassedWhenPassedStringIsEmpty()
    {
        string maskedString = "".FormatMask("###.###");
        maskedString.Should().Be("");
    }

    [Test]
    public void FormatMaskShouldReturnEmptyStringWhenPassedFormatIsEmptyString()
    {
        string maskedString = "123456789".FormatMask("");
        maskedString.Should().Be("");
    }

    [TestCase("-.-.-")]
    [TestCase("...")]
    [TestCase("...-")]
    [TestCase("::::")]
    [TestCase("-.!@$^&*()")]
    public void FormatMaskShouldReturnFormatStringWithoutSubstituteCharWhenPassedFormatDoesntContainSubstituteChar(string format)
    {
        string maskedString = "123456789".FormatMask(format);
        maskedString.Should().Be(format);
    }
}
