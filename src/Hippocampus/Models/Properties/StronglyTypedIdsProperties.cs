using StronglyTypedIds;

[assembly: StronglyTypedIdDefaults(
    StronglyTypedIdBackingType.Guid,
    StronglyTypedIdConverter.SystemTextJson
    | StronglyTypedIdConverter.EfCoreValueConverter
    | StronglyTypedIdConverter.TypeConverter)]
