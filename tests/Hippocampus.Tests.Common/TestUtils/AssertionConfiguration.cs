using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Hippocampus.Tests.Common.TestUtils;

public class AssertionConfiguration
{
    public static void ConfigureOptions()
    {
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx =>
                    ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(900)))
                .WhenTypeIs<DateTime>());
    }
}