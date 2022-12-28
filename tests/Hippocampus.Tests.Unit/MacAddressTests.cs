using Hippocampus.Types;

namespace Hippocampus.Tests.Unit;

public class MacAddressTests : BaseTest
{
    [TestCase("123456789abc")]
    [TestCase("123456789123")]
    [TestCase("ffffffffffff")]
    [TestCase("ff:ff:ff:ff:ff:ff")]
    [TestCase("ff-ff-ff-ff-ff-ff")]
    [TestCase("ffff.ffff.ffff")]
    public void MacAddressShouldInstatiateWithInvalidMacAddressString(string macAddrressString)
    {
        var testingMacAddress = new MacAddress(macAddrressString);
        testingMacAddress
            .Should()
            .BeEquivalentTo(new MacAddress(macAddrressString));
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
    public void MacAddressShouldNotInstatiateWithInvalidMacAddressString(string macAddrressString)
    {
        var testingMacAddress = () => new MacAddress(macAddrressString);
        testingMacAddress
            .Should()
            .Throw<FormatException>()
            .WithMessage($"Invalid Mac Address: {macAddrressString}");
    }
}
