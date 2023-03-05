using AutoBogus;
using Hippocampus.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientBuilder : AutoFaker<Recipient>
{
    public RecipientBuilder()
    {
        RuleFor(r => r.RecipientId, RecipientId.New());
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.RecipientLogs, Enumerable.Empty<RecipientLog>());
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.CreatedAt, faker => faker.Date.Past());
        RuleFor(r => r.IsActive, true);
    }
}