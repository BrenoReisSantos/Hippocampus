﻿using AutoBogus;
using Hippocampus.Domain.Models.Entities;

namespace Hippocampus.Tests.Common.Builders;

public sealed class RecipientMonitorBuilder : AutoFaker<RecipientMonitor>
{
    public RecipientMonitorBuilder()
    {
        RuleFor(r => r.RecipientMonitorId, RecipientMonitorId.New());
        RuleFor(r => r.Name, faker => faker.Random.Words(5));
        RuleFor(r => r.RecipientType, faker => faker.PickRandom<RecipientType>());
        RuleFor(r => r.MacAddress, faker => new(faker.Internet.Mac()));
        RuleFor(r => r.CreatedAt, faker => faker.Date.Past().ToUniversalTime());
        RuleFor(r => r.UpdatedAt, _ => null);
        RuleFor(r => r.RecipientBoundary, new RecipientBoundaryBuilder().Generate());
        RuleFor(r => r.IsActive, true);
        RuleFor(r => r.RecipientLogs, Enumerable.Empty<RecipientLog>());
        RuleFor(r => r.MonitorLinkedTo, _ => null);
    }

    public RecipientMonitorBuilder WithLinkedMonitor(RecipientMonitor linkedMonitor)
    {
        RuleFor(r => r.MonitorLinkedTo, linkedMonitor);
        return this;
    }
}