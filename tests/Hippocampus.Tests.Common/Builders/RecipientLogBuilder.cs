using AutoBogus;
using Bogus;
using Hippocampus.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientLogBuilder : AutoFaker<RecipientLog>
{
    public RecipientLogBuilder()
    {
        RuleFor(rlog => rlog.State, faker => faker.PickRandom<State>());
        RuleFor(rlog => rlog.RegisterDate, faker => faker.Date.Recent());
    }
}