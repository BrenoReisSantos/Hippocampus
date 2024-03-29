﻿using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<RecipientMonitor>
{
    private static readonly ValueConverter<MacAddress, string> RecipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<RecipientMonitor> builder)
    {
        builder.Property(r => r.RecipientMonitorId).HasConversion<RecipientMonitorId.EfCoreValueConverter>();
        builder.Property(r => r.MacAddress).HasConversion(RecipientLogMacAddressConverter);
        builder.HasIndex(r => r.MacAddress).IsUnique();
        builder.Property(r => r.RecipientType).HasConversion<string>();
        builder.Property(r => r.Name).HasMaxLength(100);
    }
}