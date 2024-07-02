using Bogus;
using Hippocampus.Tests.Common.TestUtils;

namespace Hippocampus.Tests.Common;

public abstract class BaseTest
{
    public static readonly Faker Faker = new Faker("pt_BR");

    public BaseTest()
    {
        AssertionConfiguration.ConfigureOptions();
    }
}
