﻿using Hippocampus.Domain.Models.Entities;
using Hippocampus.Domain.Models.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hippocampus.Domain.Repository.Context.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<WaterTank>
{
    private static readonly ValueConverter<MacAddress, string> RecipientLogMacAddressConverter = new(
        macAddress => macAddress.ToString(MacAddress.Mask.Colon),
        stringMacAddress => new MacAddress(stringMacAddress));

    public void Configure(EntityTypeBuilder<WaterTank> builder)
    {
        builder.Property(r => r.WaterTankId).HasConversion<WaterTankId.EfCoreValueConverter>();
        builder.Property(r => r.WaterTankType).HasConversion<string>();
        builder.Property(r => r.Name).HasMaxLength(100);
    }
}