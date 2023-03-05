using System.Text.Json;
using FakeItEasy;
using Hippocampus.Models.Values;

namespace Hippocampus.Tests.Unit.Specs;

public record Sut<T>(T Value);

public class RecipientLevelTests
{
    Faker _fake = new("pt_BR");

    [Test]
    [Repeat(10)]
    public void RecipientLevel_Should_Instatiate_With_Valid_Value()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelToTest = new LevelPercentage(recipientLevel);
        recipientLevelToTest
            .Should()
            .BeEquivalentTo(new LevelPercentage(recipientLevel));
    }

    [Test]
    [Repeat(10)]
    public void RecipientLevel_Should_Have_Valid_Value()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelToTest = new LevelPercentage(recipientLevel);
        recipientLevelToTest
            .Value.Should().Be(recipientLevel);
    }

    [Test]
    public void RecipientLevel_Should_Not_Instatiate_With_Bigger_Out_Of_Bounds_Value()
    {
        var recipientLevel = _fake.Random.Byte(100);
        var recipientLevelToTest = () => new LevelPercentage(recipientLevel);
        recipientLevelToTest
            .Should()
            .Throw<ArgumentException>()
            .WithMessage($"RecipientLevel accepts values between {0} and {100}");
    }

    [Test]
    public void RecipientLevel_Empty_Constructor_Should_Instantiate_With_Level_50()
    {
        var sut = new LevelPercentage();
        sut.Value.Should().Be(50);
    }

    [Test]
    public void RecipientLevel_Should_Implicit_Cast_To_Byte()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelToTest = new LevelPercentage(recipientLevel);
        byte sut = recipientLevelToTest;
        sut.Should().Be(recipientLevel);
    }

    [Test]
    public void Byte_Should_Implicit_Cast_To_RecipientLevel()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        LevelPercentage sut = recipientLevel;
        sut.Value.Should().Be(recipientLevel);
    }

    [Test]
    public void RecipientLevel_JsonConverter_Should_Convert_To_Just_The_Value()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelToTest = new LevelPercentage(recipientLevel);
        var recipientValueSerializedToJson = JsonSerializer.Serialize(recipientLevelToTest);
        recipientValueSerializedToJson.Should().Be($"{recipientLevelToTest.Value}");
    }

    [Test]
    public void RecipientLevel_JsonConverter_Should_Convert_To_Json_Object_With_Property_Name_And_RecipientLevel_Value()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelToTest = new LevelPercentage(recipientLevel);
        var anonymousObjectWithRecipientValue = new { RecipientLevel = recipientLevel };
        var serializedAnonymousObjectWithMacAddress = JsonSerializer.Serialize(anonymousObjectWithRecipientValue);
        var propertyName = anonymousObjectWithRecipientValue.GetType().GetProperties()[0].Name;
        serializedAnonymousObjectWithMacAddress.Should().Be(
            $"{{\"{propertyName}\":{recipientLevelToTest.Value}}}"
        );
    }

    // FIXME
    [Test]
    public void RecipientLevel_JsonConverter_Should_Convert_To_Sut_Object()
    {
        var recipientLevel = _fake.Random.Byte(0, 100);
        var recipientLevelJson = @$"{{""Value"": {recipientLevel}}}";
        var serializedStub = JsonSerializer.Deserialize<Sut<LevelPercentage>>(recipientLevelJson);
        var expected = new Sut<LevelPercentage>(new LevelPercentage(recipientLevel));
        serializedStub.Should().BeEquivalentTo(expected);
    }
}