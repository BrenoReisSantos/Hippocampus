using System.Text.Json;
using Hippocampus.Domain.Models.Values;

namespace Hippocampus.Tests.Unit.Specs;

public class MacAddressTests : BaseTest
{
    private Faker _fake = new("pt_BR");

    [TestCase("123456789abc")]
    [TestCase("123456789123")]
    [TestCase("ffffffffffff")]
    [TestCase("ff:ff:ff:ff:ff:ff")]
    [TestCase("ff-ff-ff-ff-ff-ff")]
    [TestCase("ffff.ffff.ffff")]
    public void MacAddress_Should_Instatiate_With_Valid_MacAddress_String(string macAddrressString)
    {
        var testingMacAddress = new MacAddress(macAddrressString);
        testingMacAddress.Should().BeEquivalentTo(new MacAddress(macAddrressString));
    }

    [TestCase("123456789abc", "123456789abc")]
    [TestCase("123456789123", "123456789123")]
    [TestCase("ffffffffffff", "ffffffffffff")]
    [TestCase("ff:ff:ff:ff:ff:ff", "ffffffffffff")]
    [TestCase("ff-ff-ff-ff-ff-ff", "ffffffffffff")]
    [TestCase("ffff.ffff.ffff", "ffffffffffff")]
    public void MacAddress_Should_Have_Valid_Value(
        string macAddrressString,
        string expectedMacAddress
    )
    {
        var testingMacAddress = new MacAddress(macAddrressString);
        testingMacAddress.Value.Should().Be(expectedMacAddress);
    }

    [TestCase("123456789abc", "12:34:56:78:9a:bc")]
    [TestCase("123456789123", "12:34:56:78:91:23")]
    [TestCase("ffffffffffff", "ff:ff:ff:ff:ff:ff")]
    [TestCase("ff:ff:ff:ff:ff:ff", "ff:ff:ff:ff:ff:ff")]
    [TestCase("ff-ff-ff-ff-ff-ff", "ff:ff:ff:ff:ff:ff")]
    [TestCase("ffff.ffff.ffff", "ff:ff:ff:ff:ff:ff")]
    public void MacAddress_ToString_Should_Return_Colon_Format(
        string macAddrressString,
        string expected
    )
    {
        var testingMacAddress = new MacAddress(macAddrressString);
        testingMacAddress.ToString(MacAddress.Mask.Colon).Should().Be(expected);
    }

    [TestCase("")]
    [TestCase("ff:ff:ff")]
    [TestCase("123456789abcaaa")]
    [TestCase("12g456789123")]
    [TestCase("ffffffffgfff")]
    [TestCase("ff:ff:ff:ff:fg:ff")]
    [TestCase("ff:ff:ff:ff:ff:fff")]
    [TestCase("ff-ff-ff-ff-fg-ff")]
    [TestCase("ff-ff-ff-ff-ff-fff")]
    [TestCase("ffff.ffff.fffff")]
    [TestCase("ffff.ffff ffff")]
    public void MacAddress_Should_Not_Instatiate_With_Invalid_MacAddress_String(
        string macAddrressString
    )
    {
        var testingMacAddress = () => new MacAddress(macAddrressString);
        testingMacAddress
            .Should()
            .Throw<FormatException>()
            .WithMessage($"Invalid Mac Address: {macAddrressString}");
    }

    [Test]
    public void MacAddress_JsonConverter_Should_Convert_To_Just_The_Value()
    {
        var macAddressText = faker.Internet.Mac();
        var macAddress = new MacAddress(macAddressText);
        var macAddressSerializedToJson = JsonSerializer.Serialize(macAddress);
        macAddressSerializedToJson.Should().Be($"\"{macAddress.ToString(MacAddress.Mask.Colon)}\"");
    }

    [Test]
    public void MacAddress_JsonConverter_Should_Convert_To_Json_Object_With_Property_Name_And_MacAddress_Value()
    {
        var macAddressText = faker.Internet.Mac();
        var macAddress = new MacAddress(macAddressText);
        var anonymousObjectWithMacAddress = new { MacAddress = macAddress };
        var serializedAnonymousObjectWithMacAddress = JsonSerializer.Serialize(
            anonymousObjectWithMacAddress
        );
        var propertyName = anonymousObjectWithMacAddress.GetType().GetProperties()[0].Name;
        serializedAnonymousObjectWithMacAddress
            .Should()
            .Be($"{{\"{propertyName}\":\"{macAddress.ToString(MacAddress.Mask.Colon)}\"}}");
    }

    [Test]
    public void MacAddress_JsonConverter_Should_Convert_To_MacAddress_Sut_Object()
    {
        var macAddressText = _fake.Internet.Mac();
        var recipientLevelJson = @$"{{""Value"": ""{macAddressText}""}}";
        var serializedMacAddressSut = JsonSerializer.Deserialize<Sut<MacAddress>>(
            recipientLevelJson
        );
        var expected = new Sut<MacAddress>(new MacAddress(macAddressText));
        serializedMacAddressSut.Should().BeEquivalentTo(expected);
    }
}
