using AutoBogus;
using Bogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientBoundaryBuilder : AutoFaker<RecipientBoundary>
{
    public RecipientBoundaryBuilder()
    {
        // CustomInstantiator(faker => new(faker.Random.Float(0, 50), faker.Random.Float(51, 100)));
        RuleFor(rb => rb.MinHeight, faker => faker.Random.Float(0, 50));
        RuleFor(rb => rb.MinHeight, faker => faker.Random.Float(51, 100));
    }
}