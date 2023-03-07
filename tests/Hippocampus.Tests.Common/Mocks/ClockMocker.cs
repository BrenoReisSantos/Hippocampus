using Bogus;
using FakeItEasy;
using Hippocampus.Domain.Services.ApplicationValues;

namespace Hippocampus.Tests.Common.Mocks;

public static class ClockMocker
{
    public static IClock Mock()
    {
        var faker = new Faker("pt_BR");
        var fakeClock = A.Fake<IClock>();
        A.CallTo(() => fakeClock.Now).Returns(faker.Date.Recent());
        return fakeClock;
    }
}