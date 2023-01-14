using Bogus;
using FakeItEasy;
using Hippocampus.Services.ApplicationValues;

namespace Hippocampus.Tests.Common.Mocks;

public static class ClockMocker
{
    public static IClock MockClock()
    {
        var faker = new Faker("pt_BR");
        var recentDate = faker.Date.Recent();
        var fakeClock = A.Fake<IClock>();
        A.CallTo(() => fakeClock.Now).Returns(recentDate);
        A.CallTo(() => fakeClock.Today).Returns(DateOnly.FromDateTime(recentDate));
        return fakeClock;
    }
}