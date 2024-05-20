using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Hippocampus.Tests.Common.TestUtils;

public static class AssertionConfiguration
{
    public static void ConfigureOptions()
    {
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(500)))
                .WhenTypeIs<DateTime>());
    }
}