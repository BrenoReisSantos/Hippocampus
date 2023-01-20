using Hippocampus.Models.Entities;
using Hippocampus.Models.Values;
using Hippocampus.Services.ApplicationValues;

namespace Hippocampus.Models.Dto;

public class RecipientLogRegisterDto
{
    public required MacAddress MacAddress { get; init; }

    public required RecipientLevel Level { get; init; }

    public required State State { get; init; }
}

public static class RecipientLogRegisterDtoExtensions
{
    public static RecipientLog ToEntity(this RecipientLogRegisterDto recipientDto, IClock clock) => new RecipientLog
    {
        MacAddress = recipientDto.MacAddress,
        State = recipientDto.State,
        Level = recipientDto.Level,
        RecipientLogId = Entities.RecipientLogId.New(),
        RegisterDate = clock.Now
    };
}