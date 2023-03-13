using AutoBogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorBuilder : AutoFaker<RecipientMonitor>
{
    public RecipientMonitorBuilder()
    {
        RuleFor(r => r.RecipientMonitorId, RecipientMonitorId.New());
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.CreatedAt, faker => faker.Date.Past());
        RuleFor(r => r.RecipientBoundary, new RecipientBoundaryBuilder().Generate());
        RuleFor(r => r.IsActive, true);
        RuleFor(r => r.RecipientLogs, Enumerable.Empty<RecipientLog>());
    }
}